using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[EventListener("OnCustomEvent")]
	public class CheckEvent : ConditionTask {

		[RequiredField]
		public BBString eventName;

		private bool isReceived = false;

		protected override string conditionInfo{
			get {return "[" + eventName + "]"; }
		}

		protected override bool OnCheck(){

			if (isReceived){
				isReceived = false;
				return true;
			}

			return false;
		}

		void OnCustomEvent(string receivedEvent){

			if (receivedEvent == eventName.value)
				isReceived = true;
		}
	}
}