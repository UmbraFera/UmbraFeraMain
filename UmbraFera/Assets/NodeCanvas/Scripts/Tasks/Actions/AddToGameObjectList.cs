using UnityEngine;
using System.Collections.Generic;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptCategory("Interop")]
	public class AddToGameObjectList : ActionTask {

		[RequiredField]
		public BBGameObjectList targetList = new BBGameObjectList{blackboardOnly = true};
		public List<BBGameObject> objectsToAdd = new List<BBGameObject>();

		public bool onlyIfNotContained = true;

		protected override string actionInfo{
			get {return "Add " + objectsToAdd.Count.ToString() + " objects to " + targetList; }
		}

		protected override void OnExecute(){

			foreach (BBGameObject bbGO in objectsToAdd){

				if (onlyIfNotContained && targetList.value.Contains(bbGO.value))
					continue;

				targetList.value.Add(bbGO.value);
			}

			EndAction();
		}
	}
}