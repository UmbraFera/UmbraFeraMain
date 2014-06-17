using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[ScriptCategory("Interop")]
	public class CheckString : ConditionTask {

		public BBString stringA = new BBString{blackboardOnly = true};
		public BBString stringB;

		protected override string conditionInfo{
			get {return stringA + " == " + stringB;}
		}

		protected override bool OnCheck(){
			return stringA == stringB;
		}
	}
}