using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptCategory("Interop")]
	public class SetFloatRandom : ActionTask {

		public BBFloat minValue;
		public BBFloat maxValue;

		public BBFloat setValue = new BBFloat{blackboardOnly = true};

		protected override string actionInfo{
			get {return "Set " + setValue + " Random(" + minValue + ", " + maxValue + ")";}
		}

		protected override void OnExecute(){

			setValue.value = Random.Range(minValue.value, maxValue.value);
			EndAction();
		}
	}
}