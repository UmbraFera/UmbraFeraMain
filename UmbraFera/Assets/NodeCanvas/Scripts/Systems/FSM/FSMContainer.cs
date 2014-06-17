using UnityEngine;
using System.Collections.Generic;

namespace NodeCanvas.FSM{

	[AddComponentMenu("")]
	///The actual State Machine
	public class FSMContainer : NodeGraphContainer{

		private FSMNodeBase currentState;
		private FSMNodeBase lastState;
		private List<FSMAnyStateLink> anyStates = new List<FSMAnyStateLink>();

		///The current state name. Null if none
		public string currentStateName{
			get {return currentState != null? currentState.stateName : null; }
		}

		///The last state name. Not the current! Null if none
		public string lastStateName{
			get	{return lastState != null? lastState.stateName : null; }
		}

		public override System.Type baseNodeType{
			get {return typeof(FSMNodeBase);}
		}

		protected override void OnGraphStarted(){

			anyStates.Clear();
			foreach(NodeBase node in allNodes){

				if (node.GetType() == typeof(FSMConcurrentState))
					node.Execute(agent, blackboard);

				if (node.GetType() == typeof(FSMAnyStateLink))
					anyStates.Add(node as FSMAnyStateLink);
			}

			EnterState(lastState == null? primeNode as FSMNodeBase : lastState);
		}

		protected override void OnGraphUpdate(){

			foreach(FSMAnyStateLink anyState in anyStates)
				anyState.OnUpdate();

			currentState.OnUpdate();
		}

		protected override void OnGraphStoped(){

			lastState = null;
			currentState = null;
		}

		protected override void OnGraphPaused(){
			lastState = currentState;
			currentState = null;
		}

		///Enter a state providing the state itself
		public void EnterState(FSMNodeBase state){

			if (!isRunning){
				Debug.LogWarning("Tried to EnterState on an FSM that was not running", gameObject);
				return;
			}

			if (state == currentState)
				Debug.Log("Entered Same State");
				//return;

			if (currentState != null){
				
				currentState.ResetNode();
				
				//for editor..
				foreach (ConnectionBase inConnection in currentState.inConnections)
					inConnection.connectionState = NodeStates.Resting;
				///
			}

			lastState = currentState;
			currentState = state;
			state.Execute(agent, blackboard);
		}

		///Trigger a state to enter by it's name
		public void TriggerState(string stateName){

			foreach (NodeBase node in allNodes){
				if ((node as FSMNodeBase).stateName == stateName ){
					EnterState(node as FSMNodeBase);
					return;
				}
			}

			Debug.LogWarning("No State with name '" + stateName + "' found on FSM '" + graphName + "'");
		}

		///Get all State Names. Un-named states are not included.
		public List<string> GetStateNames(){

			var names = new List<string>();
			foreach(FSMNodeBase node in allNodes){
				if (!string.IsNullOrEmpty(node.stateName))
					names.Add(node.stateName);
			}
			return names;
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		[UnityEditor.MenuItem("NC/Create FSM")]
		public static void Create(){

			FSMContainer newFSM= new GameObject("FSM").AddComponent(typeof(FSMContainer)) as FSMContainer;
			UnityEditor.Selection.activeObject = newFSM;
		}
		
		#endif
	}
}