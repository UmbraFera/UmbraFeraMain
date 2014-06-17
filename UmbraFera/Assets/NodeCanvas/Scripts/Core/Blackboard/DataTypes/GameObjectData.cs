using UnityEngine;
using System.Collections;

namespace NodeCanvas.Variables{

	[AddComponentMenu("")]
	public class GameObjectData : Data{

		public GameObject value;

		public override System.Type dataType{
			get {return typeof(GameObject);}
		}

		public override void SetValue(System.Object value){
			this.value = (GameObject)value;
		}

		public override System.Object GetValue(){
			return value;
		}

		public override System.Object GetSerialized(){

			GameObject obj= value;

			if (obj == null)
				return null;

			string path= "/" + obj.name;

			while (obj.transform.parent != null){
				obj = obj.transform.parent.gameObject;
				path = "/" + obj.name + path;
			}
			
			return path;
		}

		public override void SetSerialized(System.Object obj){

			value = GameObject.Find(obj as string);
			if (value == null)
				Debug.LogWarning("GameObjectData Failed to load. GameObject is not in scene. Path '" + (obj as string) + "'");
		}

		//////////////////////////
		///////EDITOR/////////////
		//////////////////////////
		#if UNITY_EDITOR

		public override void ShowDataGUI(){
			value = UnityEditor.EditorGUILayout.ObjectField(value, typeof(GameObject), true, GUILayout.MaxWidth(100), GUILayout.ExpandWidth(true)) as GameObject;
		}

		#endif
	}
}