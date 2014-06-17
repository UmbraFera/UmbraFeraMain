using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptCategory("Interop")]
	public class SetGameObjectData : ActionTask {

		public BBGameObject valueA = new BBGameObject{blackboardOnly = true};
		public BBGameObject valueB;

		protected override string actionInfo{
			get {return "Set " + valueA + " = " + valueB;}
		}

		protected override void OnExecute(){

			valueA.value = valueB.value;
			EndAction();
		}
	}
}