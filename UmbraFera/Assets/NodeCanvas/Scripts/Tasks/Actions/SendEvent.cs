using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptCategory("Interop")]
	[AgentType(typeof(GraphOwner))]
	public class SendEvent : ActionTask {

		[RequiredField]
		public BBString eventName;
		public BBFloat delay;

		protected override string actionInfo{
			get{ return "Send [" + eventName + "]" + (delay.value > 0? " after " + delay + " sec." : "" );}
		}

		protected override void OnUpdate(){

			if (elapsedTime > delay.value){
				(agent as GraphOwner).SendEvent(eventName.value);
				EndAction();
			}
		}
	}
}