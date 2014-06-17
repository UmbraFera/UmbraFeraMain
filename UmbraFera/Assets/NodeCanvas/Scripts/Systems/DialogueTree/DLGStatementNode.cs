#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;

namespace NodeCanvas.DialogueTree{

	[AddComponentMenu("")]
	[ScriptName("Say")]
	public class DLGStatementNode : DLGNodeBase{

		public Statement statement= new Statement("This is a dialogue text");

		public override string nodeName{
			get{return base.nodeName + " " + finalActorName;}
		}

		public override string nodeDescription{
			get{return "This node will dispatch the OnActorSpeaking event, with arguments the Actor selected and the Statement info. Use EventHandler.Subscribe to catch events.";}
		}

		protected override NodeStates OnExecute(){

			if (!finalActor){
				DLGTree.StopGraph();
				return NodeStates.Error;
			}

			string s = statement.text;
			int i = 0;
			while ( (i = s.IndexOf('[', i)) != -1){
				int end = s.Substring(i + 1).IndexOf(']');
				string varName = s.Substring(i + 1, end);
				string output = s.Substring(i, end + 2);
				object o = null;
				if (finalBlackboard != null)
					o = finalBlackboard.GetDataValue(varName, typeof(object));
				s = s.Replace(output, o != null? o.ToString() : "*" + varName + "*");
				i++;
			}

			var finalStatement = new Statement(s);
			finalStatement.audio = statement.audio;
			finalStatement.meta = statement.meta;

			EventHandler.Dispatch(DLGEvents.OnActorSpeaking, new DialogueSpeechInfo(finalActor, finalStatement, OnStatementEnd));
			return NodeStates.Success;
		}

		private void OnStatementEnd(){

			if (!DLGTree.isRunning)
				return;

			if (outConnections.Count == 0){
				DLGTree.StopGraph();
				return;
			}

			outConnections[0].Execute();
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){

			base.OnNodeGUI();
			GUIStyle labelStyle = new GUIStyle(GUI.skin.GetStyle("label"));
			labelStyle.wordWrap = true;
			GUILayout.Label("\"<i> " + statement.text + "</i> \"", labelStyle);
		}

		protected override void OnNodeInspectorGUI(){

			base.OnNodeInspectorGUI();
			GUIStyle areaStyle = new GUIStyle(GUI.skin.GetStyle("TextArea"));
			areaStyle.wordWrap = true;
			
			GUI.color = Color.yellow;
			GUILayout.Label("Dialogue Text");
			GUI.color = Color.white;
			statement.text = EditorGUILayout.TextArea(statement.text, areaStyle, GUILayout.Height(100));

			GUI.color = Color.yellow;
			GUILayout.Label("Audio File");
			GUI.color = Color.white;
			statement.audio = EditorGUILayout.ObjectField(statement.audio, typeof(AudioClip), false)  as AudioClip;
			
			GUI.color = Color.yellow;
			GUILayout.Label("Meta Data");
			GUI.color = Color.white;
			statement.meta = EditorGUILayout.TextField(statement.meta);
		}

		#endif
	}
}