using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[ScriptCategory("Interop")]
	public class CheckBoolean : ConditionTask{

		public BBBool valueA = new BBBool{blackboardOnly = true};
		public BBBool valueB;

		protected override string conditionInfo{
			get {return valueA + " == " + valueB;}
		}

		protected override bool OnCheck(){

			return valueA.value == valueB.value;
		}
	}
}