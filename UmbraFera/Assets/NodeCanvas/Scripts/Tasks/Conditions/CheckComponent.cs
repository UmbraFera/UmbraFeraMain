using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[ScriptCategory("Interop")]
	public class CheckComponent : ConditionTask {

		public BBComponent valueA = new BBComponent{blackboardOnly = true};
		public BBComponent valueB;

		protected override string conditionInfo{
			get {return valueA + " == " + valueB;}
		}

		protected override bool OnCheck(){

			return valueA.value == valueB.value;
		}
	}
}