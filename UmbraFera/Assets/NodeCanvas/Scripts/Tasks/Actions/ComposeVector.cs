using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptCategory("Interop")]
	public class ComposeVector : ActionTask {

		public BBFloat x;
		public BBFloat y;
		public BBFloat z;
		public BBVector saveAs =  new BBVector{blackboardOnly = true};

		protected override string actionInfo{
			get {return "New Vector " + saveAs;}
		}

		protected override void OnExecute(){

			saveAs.value = new Vector3(x.value, y.value, z.value);
			EndAction();
		}
	}
}