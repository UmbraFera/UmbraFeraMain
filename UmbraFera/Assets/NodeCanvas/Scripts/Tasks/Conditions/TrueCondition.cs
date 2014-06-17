using UnityEngine;

namespace NodeCanvas.Conditions{

	[ScriptCategory("Interop")]
	public class TrueCondition : ConditionTask{

		protected override string conditionInfo{
			get {return "True";}
		}

		protected override bool OnCheck(){
			return true;
		}
	}
}