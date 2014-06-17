using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptCategory("GameObject")]
	[AgentType(typeof(Transform))]
	public class SendUnityMessage : ActionTask{

		[RequiredField]
		public BBString methodName;

		protected override string actionInfo{
			get {return "Message " + methodName;}
		}

		protected override void OnExecute(){

			agent.SendMessage(methodName.value);
			EndAction();
		}
	}
}