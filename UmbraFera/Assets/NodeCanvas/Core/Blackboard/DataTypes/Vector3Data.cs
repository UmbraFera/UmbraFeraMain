﻿using UnityEngine;

namespace NodeCanvas.Variables{

	[AddComponentMenu("")]
	public class Vector3Data : VariableData{

		public Vector3 value;

		public override object objectValue{
			get {return value;}
			set
			{
				if (this.value != (Vector3)value){
					this.value = (Vector3)value;
					OnValueChanged(value);
				}
			}
		}

		public override object GetSerialized(){
			return new float[] {value.x, value.y, value.z};
		}

		public override void SetSerialized(object obj){
			var floatArr = obj as float[];
			value = new Vector3(floatArr[0], floatArr[1], floatArr[2]);
		}

		//////////////////////////
		///////EDITOR/////////////
		//////////////////////////
		#if UNITY_EDITOR

		public override void ShowDataGUI(){
			objectValue = (Vector3)UnityEditor.EditorGUILayout.Vector3Field("", (Vector3)objectValue, GUILayout.MaxWidth(100), GUILayout.ExpandWidth(true), GUILayout.MaxHeight(18));
		}

		#endif
	}
}