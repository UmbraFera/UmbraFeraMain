#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
namespace NodeCanvas.DialogueTree{

	[AddComponentMenu("")]
	[ScriptName("Action")]
	public class DLGActionNode : DLGNodeBase{

		[SerializeField]
		private ActionTask _action;

		public ActionTask action{
			get {return _action;}
			set
			{
				_action = value;
				if (_action != null)
					_action.SetOwnerDefaults(this);
			}
		}

		public override string nodeName{
			get{return base.nodeName + " " + finalActorName;}
		}

		public override string nodeDescription{
			get{return "This node will execute an ActionTask with the DialogueActor selected. The BlackBoard will be taken from the selected Actor.";}
		}

		protected override NodeStates OnExecute(){

			if (!action){
				DLGTree.StopGraph();
				return Error("No Action Assigned to Dialogue Action Node", gameObject);
			}

			if (!finalActor){
				DLGTree.StopGraph();
				return NodeStates.Error;
			}

			nodeState = NodeStates.Running;
			action.ExecuteAction(finalActor, finalBlackboard, OnActionEnd);
			return nodeState;
		}

		private void OnActionEnd(System.ValueType success){

			if (!DLGTree.isRunning)
				return;

			nodeState = NodeStates.Success;
			if (! (bool)success || outConnections.Count == 0){
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

			base.OnNodeGUI();
			
			if (action == null){
				GUILayout.Label("No Action");
				return;
			}

			GUILayout.Label(action.taskInfo);
		}

		protected override void OnNodeInspectorGUI(){
			
			base.OnNodeInspectorGUI();

			if (action == null){
				EditorUtils.ShowComponentSelectionButton(gameObject, typeof(ActionTask), delegate(Component a){action = (ActionTask)a;} );
				return;
			}
			
			if (EditorUtils.TaskTitlebar(action))
				action.ShowTaskEditGUI();
		}

		#endif
	}
}