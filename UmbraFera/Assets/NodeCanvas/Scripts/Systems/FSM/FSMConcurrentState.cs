#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;

namespace NodeCanvas.FSM{

	[AddComponentMenu("")]
	[ScriptName("Concurrent State")]
	public class FSMConcurrentState : FSMNodeBase{

		[SerializeField]
		private ActionList _actionList;

		private bool hasBeenExecuted;

		public ActionList actionList{
			get {return _actionList;}
			set
			{
				_actionList = value;
				if (_actionList != null)
					_actionList.SetOwnerDefaults(graph);
			}
		}

		public override string nodeName{
			get{return "Concurrent";}
		}

		public override string nodeDescription{
			get{return "This node will execute as soon as the graph is started and in parallel to any other state. This is not a state per se and thus it has no transitions as well as it can't be Entered by transitions.";}
		}

		public override int maxInConnections{
			get {return 0;}
		}

		public override int maxOutConnections{
			get {return 0;}
		}

		public override bool allowAsPrime{
			get {return false;}
		}

		protected override void OnCreate(){
			actionList = gameObject.AddComponent<ActionList>();
		}

		public override void OnGraphStarted(){
			hasBeenExecuted = false;
		}

		public override void OnGraphPaused(){
			actionList.PauseAction();
		}

		protected override NodeStates OnExecute(){

			if (hasBeenExecuted)
				return NodeStates.Success;

			if (!actionList){
				graph.StopGraph();
				return Error("No ActionList assigned!", gameObject);
			}

			actionList.runInParallel = true;
			actionList.ExecuteAction(graphAgent, graphBlackboard, OnActionListFinished);
			return NodeStates.Running;
		}

		private void OnActionListFinished(System.ValueType didSucceed){
			nodeState = NodeStates.Success;
			hasBeenExecuted = true;
		}

		protected override void OnReset(){
			actionList.EndAction(false);
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){

			if (Application.isPlaying)
				GUILayout.Label(elapsedTime.ToString());

			if (actionList)
				GUILayout.Label(actionList.taskInfo);
		}

		protected override void OnNodeInspectorGUI(){

			if (!actionList)
				return;

			GUI.color = Color.yellow;
			GUILayout.Label("Actions");
			GUI.color = Color.white;
			actionList.ShowListGUI();
			actionList.ShowNestedActionsGUI();
		}

		#endif
	}
}