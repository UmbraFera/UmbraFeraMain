#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[Category("✫ Script Control")]
	[Description("Call a boolean function on a script and return whether it returned true or false")]
	[AgentType(typeof(Transform))]
	public class CheckFunction : ConditionTask {

		public BBVariableSet paramValue1 = new BBVariableSet();
		public BBVariableSet checkSet = new BBVariableSet();

		[SerializeField]
		private string methodName;
		[SerializeField]
		private string scriptName;

		private Component script;
		private MethodInfo method;

		protected override string info{
			get
			{
				if (string.IsNullOrEmpty(methodName))
					return "No Method Selected";

				string paramInfo = "";
				paramInfo += paramValue1.selectedType != null? paramValue1.selectedBBVariable.ToString() : "";
				return string.Format("{0}.{1}({2}){3}", agentInfo, methodName, paramInfo, checkSet.selectedType == typeof(bool)? "" : " == " + checkSet.selectedBBVariable.ToString());
			}
		}

		//store the method info on agent set for performance
		protected override string OnInit(){
			script = agent.GetComponent(scriptName);
			if (script == null)
				return "Missing Component '" + scriptName + "' on Agent '" + agent.gameObject.name + "'";

			var paramTypes = new List<System.Type>();
			if (paramValue1.selectedType != null)
				paramTypes.Add(paramValue1.selectedType);

			method = script.GetType().NCGetMethod(methodName, paramTypes.ToArray());

			if (method == null)
				return "Missing Method Info";
			
			return null;
		}

		//do it by invoking method
		protected override bool OnCheck(){

			object[] args = null;
			if (paramValue1.selectedType != null)
				args = new object[]{paramValue1.objectValue};

			return method.Invoke(script, args).Equals( checkSet.objectValue );
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		[SerializeField]
		private List<string> paramNames = new List<string>{"Param1"}; //init for update
		
		protected override void OnTaskInspectorGUI(){

			if (agent == null){
				EditorGUILayout.HelpBox("This Condition needs the Agent to be known. Currently the Agent is unknown.\nConsider overriding the Agent.", MessageType.Error);
				return;
			}

			if (agent.GetComponent(scriptName) == null){
				scriptName = null;
				methodName = null;
				paramValue1.selectedType = null;
			}

			if (GUILayout.Button("Select Method")){
				EditorUtils.ShowMethodSelectionMenu(agent.gameObject, checkSet.availableTypes, paramValue1.availableTypes, delegate(MethodInfo method){
					scriptName = method.ReflectedType.Name;
					methodName = method.Name;
					var parameters = method.GetParameters();
					paramNames = parameters.Select(p => p.Name).ToList();
					paramValue1.selectedType = parameters.Length >= 1? parameters[0].ParameterType : null;
					checkSet.selectedType = method.ReturnType;
					if (Application.isPlaying)
						OnInit();
				}, 1, false);
			}

			if (!string.IsNullOrEmpty(methodName)){
				GUILayout.BeginVertical("box");
				EditorGUILayout.LabelField("Selected Component", scriptName);
				EditorGUILayout.LabelField("Selected Method", methodName);
				if (paramValue1.selectedType != null)
					EditorGUILayout.LabelField(paramNames[0], EditorUtils.TypeName(paramValue1.selectedType));
				GUILayout.EndVertical();
			}

			if (paramValue1.selectedType != null)
				EditorUtils.BBVariableField(paramNames[0], paramValue1.selectedBBVariable);

			if (checkSet.selectedType != null)
				EditorUtils.BBVariableField("Is Equal To", checkSet.selectedBBVariable);
		}

		#endif
	}
}