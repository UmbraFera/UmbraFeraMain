using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptCategory("Interop")]
	public class Wait : ActionTask {

		public BBFloat waitTime = new BBFloat{value = 1};

		protected override string actionInfo{
			get {return "Wait " + waitTime + " sec.";}
		}

		protected override void OnUpdate(){
			if (elapsedTime >= waitTime.value)
				EndAction();
		}
	}
}