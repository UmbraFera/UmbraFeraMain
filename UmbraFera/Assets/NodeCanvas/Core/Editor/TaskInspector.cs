using UnityEngine;
using System.Collections;
using UnityEditor;
using NodeCanvas;

namespace NodeCanvasEditor{

	[CustomEditor(typeof(Task), true)]
	public class TaskInspector : Editor {

		override public void OnInspectorGUI(){
			
			(target as Task).ShowInspectorGUI();
			EditorUtils.EndOfInspector();

			if (GUI.changed){
				EditorUtility.SetDirty(target);
				Repaint();
			}
		}
	}
}