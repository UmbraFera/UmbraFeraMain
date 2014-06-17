using UnityEngine;

namespace NodeCanvas.Actions{

	[ScriptName("Play Animation")]
	[ScriptCategory("Mecanim")]
	[AgentType(typeof(Animator))]
	public class MecanimPlayAnimation : ActionTask{

		public int layerIndex;
		[RequiredField]
		public string stateName;
		[SliderField(0,1)]
		public float transitTime = 0.25f;

		public bool waitUntilFinish;

		[GetFromAgent]
		private Animator animator;
		private AnimatorStateInfo info;


		protected override string actionInfo{
			get {return "Mec.PlayAnimation '" + stateName + "'";}
		}

		protected override void OnExecute(){

			animator.CrossFade(stateName, transitTime, layerIndex);
		}

		protected override void OnUpdate(){

			info = animator.GetCurrentAnimatorStateInfo(layerIndex);

			if (waitUntilFinish){

				if (info.IsName(stateName) && elapsedTime >= info.length)
					EndAction();				

			} else {

				if (elapsedTime >= transitTime)
					EndAction();
			}
		}
	}
}