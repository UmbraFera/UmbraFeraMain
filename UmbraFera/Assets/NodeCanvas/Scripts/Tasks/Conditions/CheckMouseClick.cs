using UnityEngine;
using System.Collections;

namespace NodeCanvas.Conditions{

	[ScriptCategory("System Events")]
	[AgentType(typeof(Collider))]
	[EventListener("OnMouseDown", "OnMouseUp")]
	public class CheckMouseClick : ConditionTask {

		public enum MouseClickEvent{
			MouseDown = 0,
			MouseUp = 1
		}

		public MouseClickEvent checkType = MouseClickEvent.MouseDown;

		private int current = -1;

		protected override string conditionInfo{
			get{ return checkType.ToString();}
		}

		protected override bool OnCheck(){

			return (int)checkType == current;
		}


		IEnumerator OnMouseDown(){
			current = 0;
			yield return null;
			current = -1;
		}

		IEnumerator OnMouseUp(){
			current = 1;
			yield return null;
			current = -1;
		}
	}
}