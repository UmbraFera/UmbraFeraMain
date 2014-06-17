using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[ScriptCategory("System Events")]
	[ScriptName("CheckCollision2D")]
	[AgentType(typeof(Collider2D))]
	[EventListener("OnCollisionEnter2D", "OnCollisionExit2D", "OnCollisionStay2D")]
	public class CheckCollision2D : ConditionTask {

		public enum CheckTypes
		{
			CollisionEnter = 0,
			CollisionExit  = 1,
			CollisionStay  = 2
		}

		public CheckTypes checkType = CheckTypes.CollisionEnter;
		public bool specifiedTagOnly;
		[TagField]
		public string objectTag = "Untagged";
		public BBGameObject saveGameObjectAs = new BBGameObject{blackboardOnly = true};

		private int current = -1;

		protected override string conditionInfo{
			get {return checkType.ToString() + ( specifiedTagOnly? (" '" + objectTag + "' tag") : "" );}
		}

		protected override bool OnCheck(){

			return (int)checkType == current;
		}

		void OnCollisionEnter2D(Collision2D collisionInfo){

			if (!specifiedTagOnly || collisionInfo.gameObject.tag == objectTag){
				saveGameObjectAs.value = collisionInfo.gameObject;
				current = 0;
				StartCoroutine(ResetCurrent());
			}
		}

		IEnumerator OnCollisionExit2D(Collision2D collisionInfo){
			
			if (!specifiedTagOnly || collisionInfo.gameObject.tag == objectTag){
				saveGameObjectAs.value = collisionInfo.gameObject;
				yield return null;
				current = 1;
				StartCoroutine(ResetCurrent());
			}
		}

		IEnumerator OnCollisionStay2D(Collision2D collisionInfo){

			if (!specifiedTagOnly || collisionInfo.gameObject.tag == objectTag){
				saveGameObjectAs.value = collisionInfo.gameObject;
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