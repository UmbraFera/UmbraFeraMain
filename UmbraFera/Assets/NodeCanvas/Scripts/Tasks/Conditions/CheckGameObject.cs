using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[ScriptCategory("Interop")]
	public class CheckGameObject : ConditionTask {

		public BBGameObject valueA = new BBGameObject{blackboardOnly = true};
		public BBGameObject valueB;

		protected override string conditionInfo{
			get {return valueA + " == " + valueB;}
		}

		protected override bool OnCheck(){

			return valueA.value == valueB.value;
		}
	}
}