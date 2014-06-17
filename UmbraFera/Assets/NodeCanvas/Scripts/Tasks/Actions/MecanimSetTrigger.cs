using UnityEngine;

namespace NodeCanvas.Actions{

	[ScriptName("Set Mecanim Trigger")]
	[ScriptCategory("Mecanim")]
	[AgentType(typeof(Animator))]
	public class MecanimSetTrigger : ActionTask{

		[RequiredField]
		public string mecanimParameter;

		[GetFromAgent]
		private Animator animator;

		protected override string actionInfo{
			get{return "Mec.SetTrigger '" + mecanimParameter + "'";}
		}

		protected override void OnExecute(){

			animator.SetTrigger(mecanimParameter);
			EndAction();
		}
	}
}