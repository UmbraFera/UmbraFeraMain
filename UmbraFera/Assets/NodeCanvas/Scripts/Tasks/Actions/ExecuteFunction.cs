#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptName("Execute Function | Set Property")]
	[AgentType(typeof(Transform))]
	public class ExecuteFunction : ActionTask {

		public BBValueSet multiValue = new BBValueSet();

		[SerializeField]
		private string methodName;
		[SerializeField]
		private string scriptName;

		private Component script;
		private MethodInfo method;

		private System.Type selectedType{
			get { return multiValue.selectedType; }
		}

		private BBValue selectedBBValue{
			get	{ return multiValue.selectedBBValue; }
		}

		private System.Object paramValue{
			get { return multiValue.selectedObjectValue; }
		}

		protected override string actionInfo{
			get
			{
				if (string.IsNullOrEmpty(methodName))
					return "No Method Selected";

				return agentInfo + "." + methodName + "(" + (selectedBBValue != null? selectedBBValue.ToString() : "" ) + ")" ;
			}
		}

		//store the method info on init for performance
		protected override string OnInit(){
			script = agent.GetComponent(scriptName);
			if (script == null)
				return "Missing Component '" + scriptName + "' on Agent '" + agent.gameObject.name + "' . Did the agent changed at runtime?";
			method = script.GetType().GetMethod(methodName, selectedType != null? new System.Type[] {selectedType} : System.Type.EmptyTypes);
			return null;
		}

		//do it by invoking method
		protected override void OnExecute(){

			if (method != null){
				
				method.Invoke(script, selectedType != null? new System.Object[] { paramValue } : null);
				EndAction(true);
			
			} else {

				EndAction(false);
			}
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnActionEditGUI(){

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
			multiValue.selectedIndex = EditorGUILayout.Popup("Method Parameter Type", multiValue.selectedIndex, multiValue.availableTypeNames.ToArray());
			methodName = EditorUtils.StringPopup("Method Name", methodName, EditorUtils.GetAvailableMethods(script.GetType(), selectedType, typeof(void)));
			
			if (string.IsNullOrEmpty(methodName))
				return;

			if (selectedBBValue != null)
				EditorUtils.BBValueField("Value", selectedBBValue);

			GUI.color = new Color(1f,1f,1f,0.5f);
			if (Application.isPlaying && GUILayout.Button("Update Method Info"))
				OnInit();
			GUI.color = Color.white;
		}

		#endif
	}
}