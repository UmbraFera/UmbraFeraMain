using UnityEngine;
using System.Collections;

namespace NodeCanvas.Variables{

	[AddComponentMenu("")]
	public class SystemObjectData : Data{

		public System.Object value;

		public override System.Type dataType{
			get {return value != null? value.GetType() : typeof(System.Object);}
		}

		override public System.Object GetValue(){
			return value;
		}

		override public void SetValue(System.Object value){
			this.value = value;
		}


		//////////////////////////
		///////EDITOR/////////////
		//////////////////////////
		#if UNITY_EDITOR

		override public void ShowDataGUI(){
			GUILayout.Label("(" + dataType + ")", GUILayout.MaxWidth(100), GUILayout.ExpandWidth(true));
		}

		#endif
	}
}