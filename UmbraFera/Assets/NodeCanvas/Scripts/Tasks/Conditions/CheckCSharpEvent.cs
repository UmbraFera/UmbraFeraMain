using UnityEngine;
using System.Reflection;

namespace NodeCanvas.Conditions{

	[ScriptCategory("Interop")]
	public class CheckCSharpEvent : ConditionTask {

		[RequiredField]
		public Component script;
		[RequiredField]
		public string eventName;
		
		private bool isReceived;

		protected override string conditionInfo{
			get {return "'" + eventName + "' Gets Raised";}
		}

		protected override string OnInit(){

			var eventInfo = script.GetType().GetEvent(eventName);
			var m = this.GetType().GetMethod("Raised", BindingFlags.Instance | BindingFlags.NonPublic);
			System.Delegate handler = System.Delegate.CreateDelegate(eventInfo.EventHandlerType, this, m);
			eventInfo.AddEventHandler(script, handler);
			return null;
		}

		void Raised(){

			isReceived = true;
		}

		protected override bool OnCheck(){

			if (isReceived){
				isReceived = false;
				return true;
			}
			
			return false;
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnConditionEditGUI(){

			script = EditorUtils.ComponentField("Script", script);

			if (script == null){
				eventName = null;
				UnityEditor.EditorGUILayout.HelpBox("Will subscribe to a C# event handler with zero parameters and type of void", UnityEditor.MessageType.Info);
				return;
			}

			eventName = EditorUtils.StringPopup("Event", eventName, EditorUtils.GetAvailableEvents(script.GetType()));
		}
		
		#endif
	}
}