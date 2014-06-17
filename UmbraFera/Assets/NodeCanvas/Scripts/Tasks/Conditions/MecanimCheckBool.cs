using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[ScriptName("Check Mecanim Bool")]
	[ScriptCategory("Mecanim")]
	[AgentType(typeof(Animator))]
	public class MecanimCheckBool : ConditionTask {

		[RequiredField]
		public string mecanimParameter;
		public BBBool value;

		[GetFromAgent]
		private Animator animator;

		protected override string conditionInfo{
			get{return "Mec.Bool '" + mecanimParameter + "' == " + value;}
		}

		protected override bool OnCheck(){

			return animator.GetBool(mecanimParameter) == value.value;
		}
	}
}