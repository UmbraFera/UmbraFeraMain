using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptCategory("ITween")]
	public class ITweenStop : ActionTask {

		[RequiredField]
		public BBString id;

		protected override void OnExecute(){
			iTween.StopByName(id.value);
			EndAction();
		}
	}
}