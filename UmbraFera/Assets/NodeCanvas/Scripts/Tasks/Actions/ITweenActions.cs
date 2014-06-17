using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptCategory("ITween")]
	[AgentType(typeof(Transform))]
	abstract public class ITweenActions : ActionTask {

		public BBString id;
		public BBFloat delay;
		public BBFloat time;
		public iTween.EaseType easeType = iTween.EaseType.linear;
		public bool waitForFinish = true;

		protected override void OnExecute(){

			if (!waitForFinish)
				EndAction();
		}

		protected override void OnUpdate(){

			if (elapsedTime >= delay.value + time.value)
				EndAction();
		}

		protected override void OnStop(){

			if (!string.IsNullOrEmpty(id.value))
				iTween.StopByName(id.value);
		}
	}
}