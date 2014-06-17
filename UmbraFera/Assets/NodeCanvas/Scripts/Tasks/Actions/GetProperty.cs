#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[AgentType(typeof(Transform))]
	public class GetProperty : ActionTask {

		public BBValueSet multiValue = new BBValueSet{blackboardOnly = true};

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

		private System.Object storeValue{
			set {multiValue.selectedObjectValue = value; }
		}

		protected override string actionInfo{
			get
			{
				if (string.IsNullOrEmpty(methodName))
					return "No Method Selected";

				return (selectedBBValue != null? selectedBBValue.ToString() : "" ) + " = " + agentInfo + "." + methodName;
			}
		}

		//store the method info on init for performance
		protected override string OnInit(){
			script = agent.GetComponent(scriptName);
			if (script == null)
				return "Missing Component '" + scriptName + "' on Agent '" + agent.gameObject.name + "' . Did the agent changed at runtime?";
			method = script.GetType().GetMethod(methodName, System.Type.EmptyTypes);
			return null;
		}

		//do it by invoking method
		protected override void OnExecute(){
			
			if (method != null){
				
				storeValue = method.Invoke(script, null);
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
			multiValue.selectedIndex = EditorGUILayout.Popup("Return Type", multiValue.selectedIndex, multiValue.availableTypeNames.ToArray());
			methodName = EditorUtils.StringPopup("Name", methodName, EditorUtils.GetAvailableMethods(script.GetType(), null, selectedType, true));
			
			if (string.IsNullOrEmpty(methodName))
				return;

			if (selectedBBValue != null)
				EditorUtils.BBValueField("Save As", selectedBBValue);

			GUI.color = new Color(1f,1f,1f,0.5f);
			if (Application.isPlaying && GUILayout.Button("Update Method Info"))
				OnInit();
			GUI.color = Color.white;
		}

		#endif
	}
}