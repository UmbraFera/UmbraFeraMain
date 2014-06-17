using UnityEngine;
using System.Collections;
using UnityEditor;
using NodeCanvas;
using NodeCanvas.Variables;

namespace NodeCanvasEditor{

	[CustomEditor(typeof(Blackboard))]
	public class BlackboardInspector : Editor {

		override public void OnInspectorGUI(){
			
			(target as Blackboard).ShowBlackboardGUI();
			EditorUtils.EndOfInspector();
			if (Application.isPlaying)
				Repaint();
		}

		public void OnEnable(){

			foreach (Data data in (target as Blackboard).GetAllData())
				data.hideFlags = HideFlags.HideInInspector;
		}
	}
}