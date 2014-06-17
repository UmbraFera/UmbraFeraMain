using UnityEngine;

namespace NodeCanvas.DialogueTree{

	///Holds data of a dialogue actor that is contained in the argument of an OnActorSpeaking event
	[AddComponentMenu("NodeCanvas/Dialogue Actor")]
	public class DialogueActor : MonoBehaviour{

		[SerializeField]
		private string _actorName;

		public Texture2D portrait;
		public Color dialogueColor = Color.white;
		public Vector3 dialogueOffset;
		public Blackboard blackboard;

		///The actor name
		public string actorName{
			get {return _actorName;}
			set {_actorName = value;}
		}

		///The final position for the dialogue to show based on settings
		public Vector3 dialoguePosition{
			get{return Vector3.Scale(transform.position + dialogueOffset, transform.localScale);}
		}

		private void Reset(){
			actorName = gameObject.name;
		}

		private void OnDrawGizmos(){
			Gizmos.DrawLine(transform.position, dialoguePosition);
		}

		///Finds an actor with provided name. Null if none found
		public static DialogueActor FindActorWithName(string name){
			foreach (DialogueActor actor in FindObjectsOfType(typeof(DialogueActor))){
				if (actor.actorName == name)
					return actor;
			}

			return null;
		}

	}

	///Holds data of what's being said also contained in the argument of an OnActorSpeaking event
	[System.Serializable]
	public class Statement{

		public string text = string.Empty;
		public AudioClip audio;
		public string meta = string.Empty;

		public Statement(){
		}

		public Statement(string text){
			this.text = text;
		}
	}
}