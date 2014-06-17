using UnityEngine;
using System.Collections;

namespace NodeCanvas.FSM{

	[AddComponentMenu("")]
	[ScriptName("Any State")]
	public class FSMAnyStateLink : FSMNodeBase{

	public override string nodeName{
			get{return "Any State";}
		}

		public override string nodeDescription{
			get{return "The Transitions of this node will constantly be checked. If any becomes true, the target connected State will Enter regardless of the current State. This node can have no incomming transitions.";}
		}

		public override int maxInConnections{
			get {return 0;}
		}

		public override int maxOutConnections{
			get{return -1;}
		}

		public override bool allowAsPrime{
			get {return false;}
		}

		public override void OnUpdate(){

			if (outConnections.Count == 0)
				return;

			nodeState = NodeStates.Running;

			for (int i = 0; i < outConnections.Count; i++){

				var connection = outConnections[i] as FSMConnection;
				if (connection.condition == null)
					continue;

				if (connection.CheckCondition(graphAgent, graphBlackboard)){
					fsm.EnterState(connection.targetNode as FSMNodeBase);
					return;
				}
			}
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){

			GUILayout.BeginHorizontal("box");
			GUILayout.Label("Constant Check");
			GUILayout.EndHorizontal();
		}

		protected override void OnNodeInspectorGUI(){

			base.OnNodeInspectorGUI();

			var emptyFound = false;
			foreach(FSMConnection connection in outConnections){
				if (connection.condition == null)
					emptyFound = true;
			}

			if (emptyFound)
				UnityEditor.EditorGUILayout.HelpBox("This state never finish and as such OnFinish transitions are never called. Add a condition in all transitions of this node", UnityEditor.MessageType.Info);
		}

		#endif
	}
}