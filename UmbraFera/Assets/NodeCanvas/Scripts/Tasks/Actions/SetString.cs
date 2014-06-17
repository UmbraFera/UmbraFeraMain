using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptCategory("Interop")]
	public class SetString : ActionTask {

		public BBString stringA = new BBString{blackboardOnly = true};
		public BBString stringB;

		protected override string actionInfo{
			get {return "Set " + stringA + " = " + stringB;}
		}

		protected override void OnExecute(){
			stringA.value = stringB.value;
			EndAction();
		}
	}
}