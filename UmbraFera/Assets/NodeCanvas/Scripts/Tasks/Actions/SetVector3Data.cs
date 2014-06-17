using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptCategory("Interop")]
	public class SetVector3Data : ActionTask {

		public BBVector valueA = new BBVector{blackboardOnly = true};
		public BBVector valueB;

		protected override string actionInfo{
			get {return "Set " + valueA + " = " + valueB;}
		}

		protected override void OnExecute(){

			valueA.value = valueB.value;
			EndAction();
		}
	}
}