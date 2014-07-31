using UnityEngine;

namespace NodeCanvas.Actions{

	[Name("Set Mecanim Trigger")]
	public class MecanimSetTrigger : MecanimActions{

		[RequiredField]
		public string mecanimParameter;

		protected override string info{
			get{return "Mec.SetTrigger '" + mecanimParameter + "'";}
		}

		protected override void OnExecute(){

			animator.SetTrigger(mecanimParameter);
			EndAction();
		}
	}
}