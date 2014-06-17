#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;

namespace NodeCanvas.FSM{

	[AddComponentMenu("")]
	[ScriptName("Nested FSM")]
	[ScriptCategory("Nested")]
	public class FSMNestedFSMNode : FSMNodeBase{

		[SerializeField]
		private FSMContainer _nestedFSM;
		private bool instanceChecked;

		private FSMContainer nestedFSM{
			get {return _nestedFSM;}
			set {_nestedFSM = value;}
		}

		public override string nodeName{
			get {return string.IsNullOrEmpty(stateName)? "NestedFSM" : stateName;}
		}

		public override string nodeDescription{
			get{return "This will execute a nested FSM on Enter and Stop that FSM on Exit.";}
		}

		protected override NodeStates OnExecute(){

			if (!nestedFSM)
				return Error("No nested FSM assigned to node", gameObject);

			CheckInstance();

			nodeState = NodeStates.Running;
			nestedFSM.StartGraph(graphAgent, graphBlackboard, OnNestedFinished);
			return nodeState;
		}

		private void OnNestedFinished(){
			nodeState = NodeStates.Success;
		}

		protected override void OnReset(){

			base.OnReset();
			if (nestedFSM && nestedFSM.isRunning)
				nestedFSM.StopGraph();
		}

		private void CheckInstance(){

			if (!instanceChecked && nestedFSM != null && nestedFSM.transform.parent != graph.transform){
				nestedFSM = (FSMContainer)Instantiate(nestedFSM, transform.position, transform.rotation);
				nestedFSM.transform.parent = graph.transform;
				instanceChecked = true;
			}
		}

		public override void OnGraphPaused(){
			if (nestedFSM)
				nestedFSM.PauseGraph();
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){

			base.OnNodeGUI();
			if (nestedFSM){
				
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
			nestedFSM = EditorGUILayout.ObjectField("FSM", nestedFSM, typeof(FSMContainer), true) as FSMContainer;
			if (nestedFSM == this.fsm){
				Debug.LogWarning("Nested FSM can't be itself!");
				nestedFSM = null;
			}

			if (nestedFSM != null)
				nestedFSM.graphName = nodeName;
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