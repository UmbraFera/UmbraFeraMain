using UnityEngine;
using UnityEditor;
using NodeCanvas;

namespace NodeCanvasEditor{

	[CustomEditor(typeof(ConditionsList))]
	public class ConditionsListInspector : TaskInspector{

		public override void OnInspectorGUI(){

			(target as ConditionsList).ShowTaskEditGUI();
		}
	}
}