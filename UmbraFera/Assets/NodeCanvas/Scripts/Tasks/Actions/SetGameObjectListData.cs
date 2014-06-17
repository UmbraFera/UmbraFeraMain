using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptCategory("Interop")]
	public class SetGameObjectListData : ActionTask {

		public BBGameObjectList valueA = new BBGameObjectList{blackboardOnly = true};
		public BBGameObjectList valueB;

		protected override string actionInfo{
			get {return "Set " + valueA + " = " + valueB;}
		}

		protected override void OnExecute(){

			valueA.value = valueB.value;
			EndAction();
		}
	}
}