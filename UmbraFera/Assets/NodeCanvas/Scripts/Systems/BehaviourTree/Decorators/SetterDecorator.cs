using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.BehaviourTree{

	[AddComponentMenu("")]
	[ScriptName("Setter")]
	[ScriptCategory("Decorators")]
	public class SetterDecorator : BTDecoratorNode{

		public BBGameObject agentToSet= new BBGameObject();

		public override string nodeName{
			get{return "Setter";}
		}

		public override string nodeDescription{
			get{return "Setter will set another Agent for the rest of the Tree from it’s point and on. That agent will be set to the Transform Component of the gameobject directly assigned or taken from the blackboard value specified.";}
		}

		protected override NodeStates OnExecute(Component agent, Blackboard blackboard){

			if (!decoratedConnection)
				return NodeStates.Resting;

			if (agentToSet.value != null)
				agent = agentToSet.value.transform;

			return decoratedConnection.Execute(agent, blackboard);
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){

			if (!string.IsNullOrEmpty(agentToSet.dataName) || agentToSet.value != null)
				GUILayout.Label("Agent " + agentToSet);
			else
				GUILayout.Label("Identity Agent");
		}

		protected override void OnNodeInspectorGUI(){

			base.OnNodeInspectorGUI();
			agentToSet = EditorUtils.BBValueField("Agent From", agentToSet) as BBGameObject;
		}

		#endif
	}
}