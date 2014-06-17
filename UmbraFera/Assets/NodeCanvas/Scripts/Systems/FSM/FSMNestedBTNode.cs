#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using NodeCanvas.BehaviourTree;

namespace NodeCanvas.FSM{

	[AddComponentMenu("")]
	[ScriptName("Nested Behavior Tree")]
	[ScriptCategory("Nested")]
	public class FSMNestedBTNode : FSMNodeBase{

		private enum ExecutionMode {RunOnce, RunForever}
		[SerializeField]
		private ExecutionMode executionMode = ExecutionMode.RunForever;
		[SerializeField]
		private float updateInterval = 0f;
		[SerializeField]
		private BTContainer _nestedBT;
		[SerializeField]
		private string successEvent;
		[SerializeField]
		private string failureEvent;
		
		private bool instanceChecked;

		private BTContainer nestedBT{
			get {return _nestedBT;}
			set {_nestedBT = value;}
		}

		public override string nodeName{
			get{return string.IsNullOrEmpty(stateName)? "Behaviour" : stateName;}
		}


		public override string nodeDescription{
			get{return "This will execute a Behaviour Tree graph on Enter. On Exit, the Behavior Tree graph will be stoped. You can optionaly specify a Success Event and a Failure Event which will be sent when the BT's root state returns either. Use alongside with a CheckEvent on Transition.";}
		}

		protected override NodeStates OnExecute(){

			if (!nestedBT) return Error("No nested Behavior Tree assigned", gameObject);

			CheckInstance();

			base.OnExecute();
			nodeState = NodeStates.Running;
			nestedBT.runForever = (executionMode == ExecutionMode.RunForever);
			nestedBT.updateInterval = updateInterval;
			nestedBT.StartGraph(graphAgent, graphBlackboard, OnNestedFinished);
			return nodeState;
		}

		override public void OnUpdate(){

			if (nestedBT == null)
				return;

			if (!string.IsNullOrEmpty(successEvent) && nestedBT.rootState == NodeStates.Success){
				graph.SendEvent(successEvent);
				if (nestedBT.isRunning)
					nestedBT.StopGraph();
			}

			if (!string.IsNullOrEmpty(failureEvent) && nestedBT.rootState == NodeStates.Failure){
				graph.SendEvent(failureEvent);
				if (nestedBT.isRunning)
					nestedBT.StopGraph();
			}

			base.OnUpdate();
		}

		private void OnNestedFinished(){
			nodeState = NodeStates.Success;
		}

		protected override void OnReset(){
			
			base.OnReset();
			if (nestedBT && nestedBT.isRunning)
				nestedBT.StopGraph();
		}

		public override void OnGraphPaused(){
			if (nestedBT)
				nestedBT.PauseGraph();
		}

		private void CheckInstance(){

			if (!instanceChecked && nestedBT != null && nestedBT.transform.parent != graph.transform){
				nestedBT = (BTContainer)Instantiate(nestedBT, transform.position, transform.rotation);
				nestedBT.transform.parent = graph.transform;
				instanceChecked = true;
			}
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){
			
			base.OnNodeGUI();
			if (nestedBT){
			
				GUILayout.Label(executionMode.ToString());
				if (GUILayout.Button("EDIT"))
					graph.nestedGraphView = nestedBT;

		    	var e = Event.current;
		    	if (e.type == EventType.MouseDown && e.clickCount == 2)
		    		graph.nestedGraphView = nestedBT;
			
			} else {

				if (GUILayout.Button("CREATE NEW"))
					nestedBT = CreateNewNestedTree();
			}
		}

		protected override void OnNodeInspectorGUI(){

			base.OnNodeInspectorGUI();
			nestedBT = EditorGUILayout.ObjectField("Behaviour Tree", nestedBT, typeof(BTContainer), true) as BTContainer;
			executionMode = (ExecutionMode)EditorGUILayout.EnumPopup("Execution Mode", executionMode);
			
			if (executionMode == ExecutionMode.RunForever)
				updateInterval = EditorGUILayout.FloatField("Update Interval", updateInterval);

			var alpha1 = string.IsNullOrEmpty(successEvent)? 0.5f : 1;
			var alpha2 = string.IsNullOrEmpty(failureEvent)? 0.5f : 1;
			GUILayout.BeginVertical("box");
			GUI.color = new Color(1,1,1,alpha1);
			successEvent = EditorGUILayout.TextField("Success Event", successEvent);
			GUI.color = new Color(1,1,1,alpha2);
			failureEvent = EditorGUILayout.TextField("Failure Event", failureEvent);
			GUILayout.EndVertical();
			GUI.color = Color.white;

			if (nestedBT != null)
				nestedBT.graphName = nodeName;
		}

		public BTContainer CreateNewNestedTree(){

			BTContainer newTree = new GameObject("NestedBT").AddComponent(typeof(BTContainer)) as BTContainer;
			newTree.transform.parent = graph != null? graph.transform : null;
			newTree.transform.localPosition = Vector3.zero;
			return newTree;
		}

		#endif
	}
}