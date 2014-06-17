using UnityEngine;
using System.Collections.Generic;

namespace NodeCanvas.FSM{

	///This class essentially a front-end to executing a FSM (FSMContainer)
	[AddComponentMenu("NodeCanvas/FSM Owner")]
	public class FSMOwner : GraphOwner {

		public FSMContainer FSM;

		public override NodeGraphContainer graph{
			get{return FSM;}
			set {FSM = (FSMContainer)value;}
		}

		public override System.Type graphType{
			get {return typeof(FSMContainer);}
		}

		///The current state name
		public string currentStateName{
			get {return FSM != null? FSM.currentStateName : null;}
		}

		///The last state name
		public string lastStateName{
			get {return FSM != null? FSM.lastStateName : null;}
		}

		///Enter an FSM State by it's name
		public void TriggerState(string stateName){

			if (FSM != null)
				FSM.TriggerState(stateName);
		}

		///Get all state names, excluding non-named states
		public List<string> GetStateNames(){
			if (FSM != null)
				return FSM.GetStateNames();
			return null;
		}
	}
}