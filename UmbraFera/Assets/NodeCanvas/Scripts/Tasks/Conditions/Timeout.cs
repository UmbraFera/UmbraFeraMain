using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[ScriptCategory("Interop")]
	public class Timeout : ConditionTask {

		public BBFloat timeout = new BBFloat{value = 1};
		private float currentTime;

		protected override string conditionInfo{
			get {return "Timed Out " + Mathf.Round( (currentTime/timeout.value) * 100 ) + "%";}
		}

		protected override bool OnCheck(){

			if (currentTime < timeout.value){

				if (currentTime == 0)
					StartCoroutine(Count());
			
				return false;
			}

			currentTime = 0;
			return true;
		}

		IEnumerator Count(){

			while (currentTime < timeout.value){
				currentTime += Time.deltaTime;
				yield return null;
			}
		}
	}
}