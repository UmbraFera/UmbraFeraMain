using UnityEngine;
using System.Collections;
using NodeCanvas.FSM;

namespace NodeCanvas.BehaviourTree{

	[AddComponentMenu("")]
	[ScriptName("NestedFSM")]
	[ScriptCategory("Nested")]
	public class BTNestedFSMNode : BTNodeBase{

		[SerializeField]
		private FSMContainer _nestedFSM;
		private bool instanceChecked;

		public string successState;
		public string failureState;

		private FSMContainer nestedFSM{
			get {return _nestedFSM;}
			set
			{
				_nestedFSM = value;
				if (_nestedFSM != null){
					_nestedFSM.agent = graphAgent;
					_nestedFSM.blackboard = graphBlackboard;
				}
			}
		}

		public override string nodeName{
			get {return "FSM";}
		}

		public override string nodeDescription{
			get {return "NestedFSM can be assigned an entire FSM. This node will return Running for as long as the FSM is Running. If a Success or Failure State is selected, then it will return Success or Failure as soon as the Nested FSM enters that state at which point the FSM will also be stoped. Otherwise, if the Nested FSM ends this node will return Success.";}		}

		public override int maxOutConnections{
			get {return 0;}
		}

		public override System.Type outConnectionType{
			get {return typeof(ConnectionBase);}
		}

		/////////

		protected override NodeStates OnExecute(Component agent, Blackboard blackboard){

			if (nestedFSM == null || nestedFSM.primeNode == null)
				return NodeStates.Failure;

			CheckInstance();

			if (nodeState == NodeStates.Resting || nestedFSM.isPaused){
				nodeState = NodeStates.Running;
				nestedFSM.StartGraph(agent, blackboard, OnFSMFinish);
			}

			if (!string.IsNullOrEmpty(successState) && nestedFSM.currentStateName == successState){
				nestedFSM.StopGraph();
				return NodeStates.Success;
			}

			if (!string.IsNullOrEmpty(failureState) && nestedFSM.currentStateName == failureState){
				nestedFSM.StopGraph();
				return NodeStates.Failure;
			}

			return nodeState;
		}

		private void OnFSMFinish(){
			if (nodeState == NodeStates.Running)
				nodeState = NodeStates.Success;
		}

		protected override void OnReset(){

			if (nestedFSM)
				nestedFSM.StopGraph();
		}

		public override void OnGraphPaused(){
			if (nestedFSM)
				nestedFSM.PauseGraph();
		}

		private void CheckInstance(){

			if (!instanceChecked && nestedFSM != null && nestedFSM.transform.parent != graph.transform){
				nestedFSM = (FSMContainer)Instantiate(nestedFSM, transform.position, transform.rotation);
				nestedFSM.transform.parent = graph.transform;
				instanceChecked = true;	
			}
		}

		////////////////////////////
		//////EDITOR AND GUI////////
		////////////////////////////
		#if UNITY_EDITOR

		protected override void OnNodeGUI(){

		    if (nestedFSM){

		    	GUILayout.Label("'" + nestedFSM.graphName + "'");
		    	if (graph.isRunning)
			    	GUILayout.Label("State: " + nestedFSM.currentStateName);
			    	
		    	if (GUILayout.Button("EDIT"))
		    		graph.nestedGraphView = nestedFSM;

		    	var e = Event.current;
		    	if (e.type == EventType.MouseDown && e.clickCount == 2)
		    		graph.nestedGraphView = nestedFSM;

			} else {
				
				if (GUILayout.Button("CREATE NEW"))
					nestedFSM = CreateNewNestedTree();
			}
		}

		protected override void OnNodeInspectorGUI(){

			base.OnNodeInspectorGUI();
		    nestedFSM = UnityEditor.EditorGUILayout.ObjectField("FSM", nestedFSM, typeof(FSMContainer), true) as FSMContainer;

		    if (nestedFSM == null)
		    	return;

		    successState = EditorUtils.StringPopup("Success State", successState, nestedFSM.GetStateNames(), false, true);
		    failureState = EditorUtils.StringPopup("Failure State", failureState, nestedFSM.GetStateNames(), false, true);
		}

		public FSMContainer CreateNewNestedTree(){

			FSMContainer newTree = new GameObject("NestedFSM").AddComponent(typeof(FSMContainer)) as FSMContainer;
			newTree.transform.parent = graph != null? graph.transform : null;
			newTree.transform.localPosition = Vector3.zero;
			return newTree;
		}

		#endif
	}
}