#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("✫ Script Control")]
	[Description("Execute a function on a script, of up to 3 parameters and save the return if any. If function is an IEnumerator it will start a coroutine and the action will run for as long as the coroutine is running. If the action stops, so will the coroutine.")]
	[AgentType(typeof(Transform))]
	public class ExecuteFunction : ActionTask {

		public BBVariableSet paramValue1 = new BBVariableSet();
		public BBVariableSet paramValue2 = new BBVariableSet();
		public BBVariableSet paramValue3 = new BBVariableSet();
		public BBVariableSet returnValue = new BBVariableSet{blackboardOnly = true};

		[SerializeField]
		private string methodName;
		[SerializeField]
		private string scriptName;

		private Component script;
		private MethodInfo method;
		private int paramCount;
		private bool routineRunning;

		protected override string info{
			get
			{
				if (string.IsNullOrEmpty(methodName))
					return "No Method Selected";

				string paramInfo = "";
				paramInfo += paramValue1.selectedType != null? paramValue1.selectedBBVariable.ToString() : "";
				paramInfo += paramValue2.selectedType != null? ", " + paramValue2.selectedBBVariable.ToString() : "";
				paramInfo += paramValue3.selectedType != null? ", " + paramValue3.selectedBBVariable.ToString() : "";
				return (returnValue.selectedType != null && !returnValue.selectedBBVariable.isNone? returnValue.selectedBBVariable.ToString() + "= ": "") + agentInfo + "." + methodName + "(" + paramInfo + ")" ;
			}
		}

		//store the method info on init for performance
		protected override string OnInit(){

			script = agent.GetComponent(scriptName);
			if (script == null)
				return "Missing Component '" + scriptName + "' on Agent '" + agent.gameObject.name + "'";
			
			var paramTypes = new List<System.Type>();

			if (paramValue1.selectedType != null){
				paramTypes.Add(paramValue1.selectedType);

				if (paramValue2.selectedType != null){
					paramTypes.Add(paramValue2.selectedType);
					
					if (paramValue3.selectedType != null)
						paramTypes.Add(paramValue3.selectedType);
				}
			}

			paramCount = paramTypes.Count;
			method = script.GetType().NCGetMethod(methodName, paramTypes.ToArray());

			if (method == null)
				return "Method not found";

			return null;
		}

		//do it by invoking method
		protected override void OnExecute(){

			object[] args = null;
			if (paramCount > 0){
				if (paramCount == 1)
					args = new object[]{paramValue1.objectValue};
				else if (paramCount == 2)
					args = new object[]{paramValue1.objectValue, paramValue2.objectValue};
				else if (paramCount == 3)
					args = new object[]{paramValue1.objectValue, paramValue2.objectValue, paramValue3.objectValue};
			}

			if (method.ReturnType == typeof(IEnumerator)){
				routineRunning = true;
				StartCoroutine( InternalCoroutine((IEnumerator)method.Invoke(script, args)) );
			} else {
				returnValue.objectValue = method.Invoke(script, args);
				EndAction(true);
			}
		}

		protected override void OnStop(){
			routineRunning = false;
		}


		IEnumerator InternalCoroutine(IEnumerator routine){

			while(routine.MoveNext()){
				
				yield return routine.Current;

				if (routineRunning == false)
					yield break;
			}

			EndAction(true);
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		[SerializeField]
		private List<string> paramNames = new List<string>{"Param1","Param2","Param3"}; //init for update
		private bool isIEnumerator;

		protected override void OnTaskInspectorGUI(){

			if (agent == null){
				EditorGUILayout.HelpBox("This Action needs the Agent to be known. Currently the Agent is unknown.\nConsider overriding the Agent.", MessageType.Error);
				return;
			}

			if (agent.GetComponent(scriptName) == null){
				scriptName = null;
				methodName = null;
				paramValue1.selectedType = null;
			}

			if (GUILayout.Button("Select Method")){
				
				var returnTypes = returnValue.availableTypes;
				returnTypes.Add(typeof(void));
				returnTypes.Add(typeof(IEnumerator));
				returnTypes.Add(typeof(Coroutine));
				var paramTypes = paramValue1.availableTypes;

				EditorUtils.ShowMethodSelectionMenu(agent.gameObject, returnTypes, paramTypes, delegate(MethodInfo method){
					scriptName = method.ReflectedType.Name;
					methodName = method.Name;
					var parameters = method.GetParameters();
					paramNames = parameters.Select(p => p.Name).ToList();
					paramValue1.selectedType = parameters.Length >= 1? parameters[0].ParameterType : null;
					paramValue2.selectedType = parameters.Length >= 2? parameters[1].ParameterType : null;
					paramValue3.selectedType = parameters.Length >= 3? parameters[2].ParameterType : null;
					if (method.ReturnType == typeof(IEnumerator) || method.ReturnType == typeof(void) || method.ReturnType == typeof(Coroutine)){
						returnValue.selectedType = null;
					} else {
						returnValue.selectedType = method.ReturnType;
					}

					//for gui
					isIEnumerator = method.ReturnType == typeof(IEnumerator);

					if (Application.isPlaying)
						OnInit();
				}, 3, false);
			}

			if (!string.IsNullOrEmpty(methodName)){
				GUILayout.BeginVertical("box");
				EditorGUILayout.LabelField("Selected Component", scriptName);
				EditorGUILayout.LabelField("Selected Method", methodName);
				if (paramValue1.selectedType != null)
					EditorGUILayout.LabelField(paramNames[0], EditorUtils.TypeName(paramValue1.selectedType));
				if (paramValue2.selectedType != null)
					EditorGUILayout.LabelField(paramNames[1], EditorUtils.TypeName(paramValue2.selectedType));
				if (paramValue3.selectedType != null)
					EditorGUILayout.LabelField(paramNames[2], EditorUtils.TypeName(paramValue3.selectedType));
				if (returnValue.selectedType != null)
					EditorGUILayout.LabelField("Return Type", EditorUtils.TypeName(returnValue.selectedType));
				
				if (isIEnumerator)
					GUILayout.Label("<b>This will execute as a Coroutine</b>");

				GUILayout.EndVertical();
			}

			if (paramValue1.selectedType != null)
				EditorUtils.BBVariableField(paramNames[0], paramValue1.selectedBBVariable);

			if (paramValue2.selectedType != null)
				EditorUtils.BBVariableField(paramNames[1], paramValue2.selectedBBVariable);

			if (paramValue3.selectedType != null)
				EditorUtils.BBVariableField(paramNames[2], paramValue3.selectedBBVariable);

			if (returnValue.selectedType != null)
				EditorUtils.BBVariableField("Save Return Value", returnValue.selectedBBVariable);
		}

		#endif
	}
}