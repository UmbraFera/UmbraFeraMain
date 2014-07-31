#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[Category("✫ Script Control")]
	[Description("Check a boolean property on a script and return if it's true or false")]
	[AgentType(typeof(Transform))]
	public class CheckProperty : ConditionTask {

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
				return string.Format("{0}.{1}{2}", agentInfo, methodName, checkSet.selectedType == typeof(bool)? "" : " == " + checkSet.ToString());
			}
		}

		//store the method info on agent set for performance
		protected override string OnInit(){
			script = agent.GetComponent(scriptName);
			if (script == null)
				return "Missing Component '" + scriptName + "' on Agent '" + agent.gameObject.name + "'";
			method = script.GetType().NCGetMethod(methodName);
			if (method == null)
				return "Missing Property Method Info";
			return null;
		}

		//do it by invoking method
		protected override bool OnCheck(){

			return method.Invoke(script, null).Equals( checkSet.objectValue );
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnTaskInspectorGUI(){

			if (agent == null){
				EditorGUILayout.HelpBox("This Condition needs the Agent to be known. Currently the Agent is unknown.\nConsider overriding the Agent.", MessageType.Error);
				return;
			}

			if (agent.GetComponent(scriptName) == null){
				scriptName = null;
				methodName = null;
			}

			if (GUILayout.Button("Select Property")){
				EditorUtils.ShowMethodSelectionMenu(agent.gameObject, checkSet.availableTypes, null, delegate(MethodInfo method){
					scriptName = method.ReflectedType.Name;
					methodName = method.Name;
					checkSet.selectedType = method.ReturnType;
					if (Application.isPlaying)
						OnInit();
				}, 0, true);
			}

			if (!string.IsNullOrEmpty(methodName)){
				GUILayout.BeginVertical("box");
				EditorGUILayout.LabelField("Selected Component", scriptName);
				EditorGUILayout.LabelField("Selected Property", methodName);
				GUILayout.EndVertical();
			}

			if (checkSet.selectedType != null)
				EditorUtils.BBVariableField("Is Equal To", checkSet.selectedBBVariable);
		}

		#endif
	}
}