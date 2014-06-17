#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;

namespace NodeCanvas.DialogueTree{

	[AddComponentMenu("")]
	[ScriptName("Nested Dialogue")]
	public class DLGNestedDLG : DLGNodeBase{

		public DialogueTreeContainer nestedDLG;

		public override string nodeName{
			get {return base.nodeName + " " + "Nested Dialogue";}
		}

		public override string nodeDescription{
			get{return "The Nested Dialogue Tree will execute. When that nested Dialogue Tree is finished, this node will continue instead, if it has a connection. Useful for making reusable and contained Dialogue Trees.";}
		}

		protected override NodeStates OnExecute(){

			if (!nestedDLG){
				DLGTree.StopGraph();
				return Error("No Nested Dialogue Tree assigned!", gameObject);
			}

			nestedDLG.StartGraph(graphAgent, graphBlackboard, OnDialogueFinished);
			return NodeStates.Running;
		}

		private void OnDialogueFinished(){

			if (!DLGTree.isRunning)
				return;

			if (outConnections.Count == 0){
				DLGTree.StopGraph();
				return;
			}

			outConnections[0].Execute();
		}


		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){

			if (nestedDLG){

				GUILayout.Label(nestedDLG.graphName);

				if (GUILayout.Button("EDIT"))
					DLGTree.nestedGraphView = nestedDLG;
			} else {

				GUILayout.Label("No Dialogue Tree selected");
			}

		}

		protected override void OnNodeInspectorGUI(){

			nestedDLG = EditorGUILayout.ObjectField("Nested Dialogue Tree", nestedDLG, typeof(DialogueTreeContainer), true) as DialogueTreeContainer;
			if (nestedDLG == DLGTree){
				Debug.LogWarning("Nested DialogueTree can't be itself! Please select another");
				nestedDLG = null;
			}
			

		}

		#endif
	}
}