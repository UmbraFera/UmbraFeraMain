using UnityEngine;
using System.Collections.Generic;

namespace NodeCanvas.Variables{

	[AddComponentMenu("")]
	public class QuaternionData : VariableData{

		public Quaternion value;

		public override object objectValue{
			get {return value;}
			set {this.value = (Quaternion)value;}
		}

		public override object GetSerialized(){
			return new float[] {value.x, value.y, value.z, value.w};
		}

		public override void SetSerialized(object obj){
			var floatArr = obj as float[];
			value = new Quaternion(floatArr[0], floatArr[1], floatArr[2], floatArr[3]);
		}

		//////////////////////////
		///////EDITOR/////////////
		//////////////////////////
		#if UNITY_EDITOR

		public override void ShowDataGUI(){
			var vecValue = new Vector4(value.x, value.y, value.z, value.w);
			vecValue = UnityEditor.EditorGUILayout.Vector4Field("", vecValue, GUILayout.MaxWidth(100), GUILayout.ExpandWidth(true));
			value = new Quaternion(vecValue.x, vecValue.y, vecValue.z, vecValue.w);
		}

		#endif
	}
}