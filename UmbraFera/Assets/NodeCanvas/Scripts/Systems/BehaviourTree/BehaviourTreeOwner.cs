using UnityEngine;

namespace NodeCanvas.BehaviourTree{

	///This class is essentially a front-end that wraps the execution of a BT (BTContainer)
	[AddComponentMenu("NodeCanvas/Behaviour Tree Owner")]
	public class BehaviourTreeOwner : GraphOwner {

		public BTContainer BT;

		public override NodeGraphContainer graph{
			get { return BT;}
			set { BT = (BTContainer)value;}
		}
		
		public override System.Type graphType{
			get {return typeof(BTContainer);}
		}

		///Should the BT reset and rexecute after a cycle? Sets the BTContainer's runForever
		public bool runForever{
			get {return BT != null? BT.runForever : true;}
			set {if (BT != null) BT.runForever = value;}
		}

		///The interval in seconds to update the BT. 0 for every frame. Sets the BTContainer's updateInterval
		public float updateInterval{
			get {return BT != null? BT.updateInterval : 0;}
			set {if (BT != null) BT.updateInterval = value;}
		}


		///Tick the assigned Behaviour Tree for this owner. Same as BTContainer.Tick()
		public void Tick(){
			
			if (BT == null){
				Debug.LogWarning("There is no Behaviour Tree assigned", gameObject);
				return;
			}

			BT.Tick(this, blackboard);
		}

		[System.Obsolete("Use PauseGraph() instead")]
		public void Pause(){
			PauseGraph();
		}
	}
}