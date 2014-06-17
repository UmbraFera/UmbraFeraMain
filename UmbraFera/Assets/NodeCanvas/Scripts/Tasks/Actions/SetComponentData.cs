using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptCategory("Interop")]
	[ScriptName("Set Component")]
	public class SetComponentData : ActionTask {

		public BBComponent valueA = new BBComponent{blackboardOnly = true};
		public BBComponent valueB;

		protected override string actionInfo{
			get {return "Set " + valueA + " = " + valueB;}
		}

		protected override void OnExecute(){

			valueA.value = valueB.value;
			EndAction();
		}
	}
}