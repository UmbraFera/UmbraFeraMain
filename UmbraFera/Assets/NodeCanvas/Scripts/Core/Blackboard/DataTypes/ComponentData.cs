#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace NodeCanvas.Variables{

	[AddComponentMenu("")]
	public class ComponentData : Data{

		public Component value;
		
		[SerializeField]
		private string _typeName;
		private System.Type type;

		private string typeName{
			get {return _typeName;}
			set
			{
				_typeName = value;
				if (this.value != null && !System.Type.GetType(_typeName).IsAssignableFrom(this.value.GetType()) )
					this.value = null;
			}
		}

		public override System.Type dataType{
			get
			{
				if (value != null)
					return value.GetType();

				if (string.IsNullOrEmpty(typeName))
					typeName = typeof(Component).AssemblyQualifiedName;
					
				return System.Type.GetType(typeName);
			}
		}

		public override void SetValue(System.Object value){
			this.value = (Component)value;
			if (value != null && !dataType.IsAssignableFrom(value.GetType()))
				typeName = value.GetType().AssemblyQualifiedName;
		}

		public override System.Object GetValue(){
			return value;
		}

		public override System.Object GetSerialized(){

			if (value == null)
				return null;

			GameObject obj = value.gameObject;

			if (obj == null)
				return null;

			string path= "/" + obj.name;
			while (obj.transform.parent != null){
				obj = obj.transform.parent.gameObject;
				path = "/" + obj.name + path;
			}
			
			return new SerializedComponent(path, value.GetType());
		}

		public override void SetSerialized(System.Object obj){

			SerializedComponent serComponent = obj as SerializedComponent;
			if (obj == null){
				value = null;
				return;
			}

			typeName = serComponent.trueType.AssemblyQualifiedName;
			GameObject go = GameObject.Find(serComponent.path);
			if (!go){
				Debug.LogWarning("ComponentData Failed to load. The component's gameobject was not found in the scene. Path '" + serComponent.path + "'");
				return;
			}

			value = go.GetComponent(serComponent.trueType);
			if (value == null)
				Debug.LogWarning("ComponentData Failed to load. GameObject was found but the component of type '" + serComponent.trueType.ToString() + "' itself was not. Path '" + serComponent.path + "'");
		}


		[System.Serializable]
		private class SerializedComponent{

			public string path;
			public System.Type trueType;

			public SerializedComponent(string path, System.Type type){
				this.path = path;
				this.trueType = type;
			}
		}


		//////////////////////////
		///////EDITOR/////////////
		//////////////////////////
		#if UNITY_EDITOR

		void OnValidate(){
			if (dataType == null)
				typeName = typeof(Component).AssemblyQualifiedName;
		}

		void Reset(){
			typeName = typeof(Component).AssemblyQualifiedName;
		}

		public override void ShowDataGUI(){

			value = EditorGUILayout.ObjectField(value, dataType, true, GUILayout.MaxWidth(90), GUILayout.ExpandWidth(true)) as Component;
			if (GUILayout.Button("", GUILayout.Width(10), GUILayout.Height(14))){

				var menu = new GenericMenu();
				menu.AddItem(new GUIContent("Component"), false, Selected, typeof(Component));
				foreach(System.Type type in EditorUtils.GetAssemblyTypes(typeof(Component))){
					var friendlyName = type.Assembly.GetName().Name + "/" + (string.IsNullOrEmpty(type.Namespace)? "" : type.Namespace + "/") + type.Name;
					menu.AddItem(new GUIContent(friendlyName), false, Selected, type);
				}

				menu.ShowAsContext();
			}
		}

		void Selected(object t){
			typeName = (t as System.Type).AssemblyQualifiedName;
		}

		#endif
	}
}