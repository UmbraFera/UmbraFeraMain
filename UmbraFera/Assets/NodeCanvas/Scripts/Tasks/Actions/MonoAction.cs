using UnityEngine;

namespace NodeCanvas.Actions{

	[ScriptCategory("Interop")]
	public class MonoAction : ActionTask {

		[SerializeField] [RequiredField]
		private MonoBehaviour _mono;

		public MonoBehaviour mono{
			get {return _mono;}
			set
			{
				_mono = value;
				if (_mono != null && !Application.isPlaying)
					_mono.enabled = false;
			}
		}

		protected override string actionInfo{
			get {return mono != null? ("Mono '" + mono.GetType().Name + "'") : "Select a MonoBehaviour";}
		}

		protected override void OnExecute(){

			mono.enabled = true;
		}

		protected override void OnUpdate(){

			if (mono.enabled == false)
				EndAction();
		}

		protected override void OnStop(){

			mono.enabled = false;
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		protected override void OnActionEditGUI(){

			mono = (MonoBehaviour)EditorUtils.ComponentField("Mono", mono, typeof(MonoBehaviour));
			UnityEditor.EditorGUILayout.HelpBox("Will execute a MonoBehaviour script. Add the script on the game object and drag and drop it here. The mono behaviour will become enable or disable when action executes.", UnityEditor.MessageType.Info);
		}
		
		#endif
	}
}