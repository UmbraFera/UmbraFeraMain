#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;

namespace NodeCanvas.Variables{

	[AddComponentMenu("")]
	public class IntData : Data{

		public int value;

		public override void SetValue(System.Object value){
			this.value = (int)value;
		}

		public override System.Object GetValue(){
			return value;
		}

		//////////////////////////
		///////EDITOR/////////////
		//////////////////////////
		#if UNITY_EDITOR

		public override void ShowDataGUI(){
			GUI.backgroundColor = new Color(0.7f,1,0.7f);
			value = EditorGUILayout.IntField(value, GUILayout.MaxWidth(100), GUILayout.ExpandWidth(true));
		}

		#endif
	}
}