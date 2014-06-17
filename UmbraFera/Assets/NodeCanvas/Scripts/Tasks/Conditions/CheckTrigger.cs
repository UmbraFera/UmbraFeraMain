using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[ScriptCategory("System Events")]
	[EventListener("OnTriggerEnter", "OnTriggerExit", "OnTriggerStay")]
	[AgentType(typeof(Collider))]
	public class CheckTrigger : ConditionTask{

		public enum CheckTypes
		{
			TriggerEnter = 0,
			TriggerExit  = 1,
			TriggerStay  = 2
		}

		public CheckTypes CheckType = CheckTypes.TriggerEnter;
		public bool specifiedTagOnly;
		[TagField]
		public string objectTag = "Untagged";

		public BBGameObject saveGameObjectAs = new BBGameObject{blackboardOnly = true};

		private int current = -1;

		protected override string conditionInfo{
			get {return CheckType.ToString() + ( specifiedTagOnly? (" '" + objectTag + "' tag") : "" );}
		}

		protected override bool OnCheck(){

			return (int)CheckType == current;
		}

		void OnTriggerEnter(Collider other){

			if (!specifiedTagOnly || other.tag == objectTag){
				saveGameObjectAs.value = other.gameObject;
				current = 0;
				StartCoroutine(ResetCurrent());
			}
		}

		IEnumerator OnTriggerExit(Collider other){
			
			if (!specifiedTagOnly || other.tag == objectTag){
				saveGameObjectAs.value = other.gameObject;
				yield return null;
				current = 1;
				StartCoroutine(ResetCurrent());
			}
		}

		IEnumerator OnTriggerStay(Collider other){

			if (!specifiedTagOnly || other.tag == objectTag){
				saveGameObjectAs.value = other.gameObject;
				yield return null;
				current = 2;
			}
		}

		IEnumerator ResetCurrent(){
			yield return null;
			current = -1;
		}
	}
}