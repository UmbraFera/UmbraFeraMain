using UnityEngine;
using System;
using System.Linq;

namespace NodeCanvas.Variables{

	[AddComponentMenu("")]
	public class EnumData : VariableData {

		enum DefaultEnum
		{
			One,
			Two,
			Three,
			Four,
			Five
		}

		public Enum value;
		public string stringValue;

		[SerializeField]
		private string _typeName = typeof(DefaultEnum).AssemblyQualifiedName;

		public override System.Type varType{
			get {return type;}
		}

		private System.Type type{
			get {return System.Type.GetType(_typeName);}
			set
			{
				_typeName = value.AssemblyQualifiedName;
				stringValue = Enum.GetNames(value)[0];
			}
		}

		public override object objectValue{
			get {return Enum.Parse(type, stringValue);}
			set
			{
				if (value != null && typeof(Enum).NCIsAssignableFrom(value.GetType()) && type != value.GetType() )
					type = value.GetType();

				if (value.GetType() == typeof(string)){
					if (Enum.GetNames(type).Contains(value)){
						stringValue = (string)value;
					} else {
						Debug.LogError(string.Format("{0} is not a valid name of the {1} enum type", value, type));
					}
				} else {
					stringValue = Enum.GetName(type, value);
					this.value = (Enum)value;
				}
			}
		}


		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		void Reset(){
			type = typeof(DefaultEnum);
		}

		void OnValidate(){
			if (type == null || type == typeof(Enum))
				type = typeof(DefaultEnum);
		}

		public override void ShowDataGUI(){

			var options = Enum.GetNames(type).ToList();
			var index = options.Contains(stringValue)? options.IndexOf(stringValue) : -1;
			index = UnityEditor.EditorGUILayout.Popup(index, options.ToArray(), GUILayout.MaxWidth(90), GUILayout.ExpandWidth(true));
			stringValue = (index == -1)? string.Empty : options[index];

			if (GUILayout.Button("", GUILayout.Width(10), GUILayout.Height(14))){
				var menu = new UnityEditor.GenericMenu();
				menu.AddItem(new GUIContent("Default"), false, Selected, typeof(DefaultEnum));
				menu.AddItem(new GUIContent("Status"), false, Selected, typeof(Status));
				menu.AddSeparator("/");

				foreach(System.Type t in EditorUtils.GetAssemblyTypes(typeof(Enum))){
					var friendlyName = t.Assembly.GetName().Name + "/" + (string.IsNullOrEmpty(t.Namespace)? "" : t.Namespace + "/") + t.Name;
					menu.AddItem(new GUIContent("More/" + friendlyName), false, Selected, t);
				}

				menu.ShowAsContext();
			}
		}

		void Selected(object t){
			type = (System.Type)t;
		}		

		#endif
	}
}