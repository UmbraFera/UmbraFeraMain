using UnityEngine;
using System.Collections;

namespace NodeCanvas.Conditions{

	[ScriptCategory("System Events")]
	[EventListener("OnMouseEnter", "OnMouseExit", "OnMouseOver")]
	[AgentType(typeof(Collider))]
	public class CheckMouse : ConditionTask {

		public enum CheckTypes
		{
			MouseEnter = 0,
			MouseExit  = 1,
			MouseOver  = 2
		}
		
		public CheckTypes checkType = CheckTypes.MouseEnter;
		private int current = -1;

		protected override string conditionInfo{
			get {return checkType.ToString();}
		}

		protected override bool OnCheck(){

			return (int)checkType == current;
		}

		void OnMouseEnter(){
			current = 0;
			StartCoroutine(ResetCurrent());
		}

		IEnumerator OnMouseExit(){
			yield return null;
			current = 1;
			StartCoroutine(ResetCurrent());
		}

		IEnumerator OnMouseOver(){
			yield return null;
			current = 2;
		}

		IEnumerator ResetCurrent(){
			yield return null;
			current = -1;
		}
	}
}