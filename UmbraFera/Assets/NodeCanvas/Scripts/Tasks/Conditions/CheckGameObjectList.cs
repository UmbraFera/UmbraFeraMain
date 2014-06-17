using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[ScriptCategory("Interop")]
	public class CheckGameObjectList : ConditionTask {

		[RequiredField]
		public BBGameObjectList targetList = new BBGameObjectList{blackboardOnly = true};
		[RequiredField]
		public BBGameObject ckeckGameObject;

		protected override string conditionInfo{
			get {return targetList + " contains " + ckeckGameObject;}
		}

		protected override bool OnCheck(){

			return targetList.value.Contains(ckeckGameObject.value);
		}
	}
}