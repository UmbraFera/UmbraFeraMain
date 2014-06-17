#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[ScriptName("Check Function | Property")]
	[AgentType(typeof(Transform))]
	public class CheckFunction : ConditionTask {

		public BBBool boolCheck = new BBBool{value = true};

		[SerializeField]
		private string methodName;
		[SerializeField]
		private string scriptName;

		private Component script;
		private MethodInfo method;

		protected override string conditionInfo{
			get
			{
				if (string.IsNullOrEmpty(methodName))
					return "No Method Selected";

				return "'" + methodName + "' == " + boolCheck;
			}
		}

		//store the method info on agent set for performance
		protected override string OnInit(){
			script = agent.GetComponent(scriptName);
			if (script == null)
				return "Missing Component '" + scriptName + "' on Agent '" + agent.gameObject.name + "' . Did the agent changed at runtime?";
			method = script.GetType().GetMethod(methodName, System.Type.EmptyTypes);
			return null;
		}

		//do it by invoking method
		protected override bool OnCheck(){

			if (method != null)
				return (bool)method.Invoke(script, null) == boolCheck.value;

			return true;
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnConditionEditGUI(){

			if (agent == null){
				EditorGUILayout.HelpBox("This Action needs the Agent to be known. Currently the Agent is unknown.\nConsider overriding the Agent or using SendMessage instead.", MessageType.Error);
				return;
			}

			if (script == null || (script != null && agent.GetComponent(script.GetType()) != script) ){
				script = agent.GetComponent(scriptName);
				if (script == null)
					script = agent;
			}

			script = EditorUtils.ComponentField("Component", script, typeof(Component), false);
			scriptName = script.GetType().Name;
			methodName = EditorUtils.StringPopup("Method Name", methodName, EditorUtils.GetAvailableMethods(script.GetType(), null, typeof(bool)));
			
			if (string.IsNullOrEmpty(methodName))
				return;

			boolCheck = (BBBool)EditorUtils.BBValueField("Equal To", boolCheck);

			GUI.color = new Color(1f,1f,1f,0.5f);
			if (Application.isPlaying && GUILayout.Button("Update Method Info"))
				OnInit();
			GUI.color = Color.white;
		}

		#endif
	}
}