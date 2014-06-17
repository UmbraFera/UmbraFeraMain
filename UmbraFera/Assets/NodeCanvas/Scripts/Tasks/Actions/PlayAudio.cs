using UnityEngine;

namespace NodeCanvas.Actions{

	[ScriptCategory("Audio")]
	[AgentType(typeof(Transform))]
	public class PlayAudio : ActionTask{

		[RequiredField]
		public AudioClip Clip;

		[SliderField(0,1)]
		public float Volume = 1;

		public bool waitUntilFinish;

		protected override string actionInfo{
			get {return "PlayAudio '" + (Clip? Clip.name : "NULL") + "'";}
		}
		
		protected override void OnExecute(){

			AudioSource.PlayClipAtPoint(Clip, agent.transform.position, Volume);
			if (!waitUntilFinish)
				EndAction();
		}

		protected override void OnUpdate(){

			if (elapsedTime >= Clip.length)
				EndAction();
		}
	}
}