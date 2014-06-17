#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;

namespace NodeCanvas.FSM{

	[AddComponentMenu("")]
	[ScriptName("State")]
	public class FSMStateNode : FSMNodeBase{

		[SerializeField]
		private ActionList _actionList;

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
			get{return string.IsNullOrEmpty(stateName)? "State" : stateName;}
		}

		public override string nodeDescription{
			get{return "The State node holds an ActionList. Those actions will be executed simultaneously. The State will Exit either when a connection's condition is true, or when all the actions have finished and there is a connection without condition which is treated as 'OnFinish'. If there are no connections, the FSM will Finish.";}
		}


		protected override void OnCreate(){
			actionList = gameObject.AddComponent<ActionList>();
		}

		protected override NodeStates OnExecute(){

			if (!actionList){
				graph.StopGraph();
				return Error("No ActionList assigned!", gameObject);
			}

			base.OnExecute();

			actionList.runInParallel = true;
			nodeState = NodeStates.Running;
			actionList.ExecuteAction(graphAgent, graphBlackboard, OnActionListFinished);
			return nodeState;
		}

		private void OnActionListFinished(System.ValueType didSucceed){
			nodeState = NodeStates.Success;
		}

		protected override void OnReset(){
			actionList.EndAction(false);
		}

		public override void OnGraphPaused(){
			actionList.PauseAction();
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){

			base.OnNodeGUI();
			if (actionList)
				GUILayout.Label(actionList.taskInfo);
		}

		protected override void OnNodeInspectorGUI(){

			base.OnNodeInspectorGUI();

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