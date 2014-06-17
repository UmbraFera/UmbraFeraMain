using UnityEngine;
using System.Collections;

namespace NodeCanvas.BehaviourTree{

	[AddComponentMenu("")]
	///The actual Behaviour Tree
	public class BTContainer : NodeGraphContainer{

		///Should the tree repeat forever?
		public bool runForever = true;
		///The frequenct in seconds for the tree to repeat if set to run forever.
		public float updateInterval = 0;

		private float intervalCounter = 0;
		private NodeStates _rootState = NodeStates.Resting;

		///The state of the root
		public NodeStates rootState{
			get{return _rootState;}
			private set {_rootState = value;}
		}

		public override System.Type baseNodeType{
			get {return typeof(BTNodeBase);}
		}

		protected override void OnGraphStarted(){

			intervalCounter = updateInterval;
			rootState = primeNode.nodeState;
		}

		protected override void OnGraphUpdate(){

			if (intervalCounter >= updateInterval){

				intervalCounter = 0;

				Tick(agent, blackboard);

				if (!runForever && rootState != NodeStates.Running)
					StopGraph();
			}

			intervalCounter += Time.deltaTime;
		}

		///Tick the tree once for the provided agent and with the provided blackboard
		public void Tick(Component agent, Blackboard blackboard){

			if (rootState != NodeStates.Running)
				primeNode.ResetNode();

			rootState = primeNode.Execute(agent, blackboard);
		}


		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		[UnityEditor.MenuItem("NC/Create BehaviourTree")]
		public static void Create(){
			BTContainer newBT = new GameObject("BehaviourTree").AddComponent(typeof(BTContainer)) as BTContainer;
			UnityEditor.Selection.activeObject = newBT;
		}

		#endif
	}
}