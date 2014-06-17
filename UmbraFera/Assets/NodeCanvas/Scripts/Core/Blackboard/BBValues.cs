using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace NodeCanvas.Variables{

	///Base class for Variables that allow linking to a Blackboard variable or specifying one directly.
	[Serializable]
	abstract public class BBValue{

		[SerializeField][HideInInspector]
		private Blackboard _bb;
		[SerializeField]
		private string _dataName;
		[SerializeField][HideInInspector]
		private Data _dataRef;

		[SerializeField]
		private bool _useBlackboard = false;
		[SerializeField][HideInInspector]
		private bool _blackboardOnly = false;


		///Set the blackboard provided for all BBValue fields on the object provided
		public static void SetBBFields(Blackboard bb, object o){

			CheckNullBBFields(o);

			foreach (FieldInfo field in o.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)){

				if (typeof(IList).IsAssignableFrom(field.FieldType)){
					
					var list = field.GetValue(o) as IList;
					if (list == null)
						continue;

					if (typeof(BBValue).IsAssignableFrom(field.FieldType.GetGenericArguments()[0])){
						foreach(BBValue bbValue in list)
							bbValue.bb = bb;
					}
				}

				if (typeof(BBValue).IsAssignableFrom(field.FieldType))
					(field.GetValue(o) as BBValue).bb = bb;

				if (typeof(BBValueSet) == field.FieldType)
					(field.GetValue(o) as BBValueSet).bb = bb;
			}
		}

		///Check for null bb fields and init them if null on provided object
		public static void CheckNullBBFields(object o){
			foreach (FieldInfo field in o.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)){
				if (typeof(BBValue).IsAssignableFrom(field.FieldType) && field.GetValue(o) == null)
					field.SetValue(o, Activator.CreateInstance(field.FieldType));
			}
		}

		private Data dataRef{
			get {return _dataRef;}
			set {_dataRef = value;}
		}

		///The blackboard to read/write from.
		public Blackboard bb{
			get {return _bb;}
			set
			{
				_bb = value;
				dataRef = value? value.GetData(dataName, dataType) : null;
			}
		}

		///The name of the data to read/write from
		public string dataName{
			get
			{
				if (dataRef != null)
					return dataRef.dataName;
				return _dataName;
			}
			set
			{
				_dataName = value;
				if (bb)
					dataRef = bb.GetData(value, dataType);
				if (!string.IsNullOrEmpty(value))
					useBlackboard = true;
			}
		}

		///Should read/write be allows only from the blackboard?
		public bool blackboardOnly{
			get { return _blackboardOnly;}
			set { _blackboardOnly = value; if (value == true) useBlackboard = true;}
		}

		///Are we currently reading/writing from the blackboard or the direct assigned value?
		public bool useBlackboard{
			get { return _useBlackboard; }
			set { _useBlackboard = value; if (value == false) dataName = null; }
		}

		///Is the final value null?
		virtual public bool isNull{
			get {return objectValue == null;}
		}

		///The type of the value that this object has.
		virtual public System.Type dataType{
			get {return this.GetType().GetProperty("value").PropertyType;}
		}

		//The System.Object value.
		public object objectValue{
			get {return this.GetType().GetProperty("value").GetValue(this, null);}
		}

		public override string ToString(){
			return "'<b>" + (useBlackboard? "$" + dataName : isNull? "NULL" : objectValue.ToString() ) + "</b>'";
		}

		///Read the specified type from the blackboard
		protected T Read<T>(){

			if (string.IsNullOrEmpty(dataName))
				return default(T);

			if (dataRef != null)
				return (T)dataRef.GetValue();

			if (bb != null)
				return bb.GetDataValue<T>(dataName);
			
			return default(T);
		}

		///Write the specified object to the blackboard
		protected void Write(object o){

			if (string.IsNullOrEmpty(dataName))
				return;

			if (dataRef != null){
				dataRef.SetValue(o);
				return;
			}

			if (bb != null){
				dataRef = bb.SetDataValue(dataName, o);
				return;
			}

			Debug.LogError("BBValue has neither linked data, nor blackboard");
		}
	}

	[Serializable]
	public class BBBool : BBValue{
		
		[SerializeField]
		private bool _value;
		public bool value{
			get {return useBlackboard? Read<bool>() : _value ;}
			set {if (useBlackboard) Write(value); else _value = value;}
		}
	}

	[Serializable]
	public class BBFloat : BBValue{

		[SerializeField]
		private float _value;
		public float value{
			get {return useBlackboard? Read<float>() : _value ;}
			set {if (useBlackboard) Write(value); else _value = value;}
		}
	}

	[Serializable]
	public class BBInt : BBValue{

		[SerializeField]
		private int _value;
		public int value{
			get {return useBlackboard? Read<int>() : _value ;}
			set {if (useBlackboard) Write(value); else _value = value;}
		}
	}

	[Serializable]
	public class BBVector : BBValue{

		[SerializeField]
		private Vector3 _value= Vector3.zero;
		public Vector3 value{
			get {return useBlackboard? Read<Vector3>() : _value ;}
			set {if (useBlackboard) Write(value); else _value = value;}
		}
	}

	[Serializable]
	public class BBVector2 : BBValue{

		[SerializeField]
		private Vector2 _value= Vector2.zero;
		public Vector2 value{
			get {return useBlackboard? Read<Vector2>() : _value ;}
			set {if (useBlackboard) Write(value); else _value = value;}
		}
	}

	[Serializable]
	public class BBColor : BBValue{

		[SerializeField]
		private Color _value = Color.white;
		public Color value{
			get {return useBlackboard? Read<Color>() : _value ;}
			set {if (useBlackboard) Write(value); else _value = value;}
		}
	}

	[Serializable]
	public class BBString : BBValue{

		[SerializeField]
		private string _value = string.Empty;
		public string value{
			get {return useBlackboard? Read<string>() : _value ;}
			set {if (useBlackboard) Write(value); else _value = value;}
		}
		public override bool isNull{ get {return string.IsNullOrEmpty(value); }}
	}

	[Serializable]
	public class BBGameObject : BBValue{

		[SerializeField]
		private GameObject _value;
		public GameObject value{
			get {return useBlackboard? Read<GameObject>() : _value ;}
			set {if (useBlackboard) Write(value); else _value = value;}
		}
		public override bool isNull{ get {return (value as GameObject) == null; }}
		public override string ToString(){
			if (useBlackboard) return base.ToString();
			return "'<b>" + (_value != null? _value.name : "NULL") + "</b>'";
		}
	}

	[Serializable]
	public class BBComponent : BBValue{

		[SerializeField]
		private Component _value;
		public Component value{
			get {return useBlackboard? Read<Component>() : _value ;}
			set {if (useBlackboard) Write(value); else _value = value;}
		}
		public override bool isNull{ get {return (value as Component) == null; }}
		public override string ToString(){
			if (useBlackboard) return base.ToString();
			return "'<b>" + (_value != null? _value.name : "NULL") + "</b>'";
		}
	}

	[Serializable]
	public class BBGameObjectList : BBValue{

		[SerializeField]
		private List<GameObject> _value = new List<GameObject>();
		public List<GameObject> value{
			get {return useBlackboard? Read<List<GameObject>>() : _value ;}
			set {if (useBlackboard) Write(value); else _value = value;}
		}
		public override string ToString(){
			if (useBlackboard) return base.ToString();
			return "'<b>" + (_value != null? "GO List (" + _value.Count.ToString() : "NULL") + ")</b>'";
		}
	}

	///A collection of multiple BBValues
	[Serializable]
	public class BBValueSet{

		public int selectedIndex = 0;

		//value set
		public BBBool boolValue       = new BBBool();
		public BBFloat floatValue     = new BBFloat();
		public BBInt intValue         = new BBInt();
		public BBString stringValue   = new BBString();
		public BBVector vectorValue   = new BBVector();
		public BBGameObject goValue   = new BBGameObject();
		public BBVector2 vector2Value = new BBVector2();
		public BBColor colorValue     = new BBColor();
		//

		[SerializeField]
		private List<System.Type> availableTypes = new List<System.Type>() {
			null,
			typeof(bool),
			typeof(float),
			typeof(int),
			typeof(string),
			typeof(Vector3),
			typeof(GameObject),
			typeof(Vector2),
			typeof(Color)
		};

		public Blackboard bb{
			set
			{
				boolValue.bb    = value;
				floatValue.bb   = value;
				intValue.bb     = value;
				stringValue.bb  = value;
				vectorValue.bb  = value;
				goValue.bb      = value;
				vector2Value.bb = value;
				colorValue.bb   = value;
			}
		}

		public bool blackboardOnly{
			set
			{
				boolValue.blackboardOnly    = value;
				floatValue.blackboardOnly   = value;
				intValue.blackboardOnly     = value;
				stringValue.blackboardOnly  = value;
				vectorValue.blackboardOnly  = value;
				goValue.blackboardOnly      = value;
				vector2Value.blackboardOnly = value;
				colorValue.blackboardOnly   = value;
			}
		}

		public System.Type selectedType{
			get {return availableTypes[selectedIndex];}
		}

		public BBValue selectedBBValue{
			get
			{
				var t = selectedType;
				if (t == typeof(bool))
					return boolValue;
				if (t == typeof(float))
					return floatValue;
				if (t == typeof(int))
					return intValue;
				if (t == typeof(string))
					return stringValue;
				if (t == typeof(Vector3))
					return vectorValue;
				if (t == typeof(GameObject))
					return goValue;
				if (t == typeof(Vector2))
					return vector2Value;
				if (t == typeof(Color))
					return colorValue;

				return null;		
			}
		}

		public object selectedObjectValue{
			get
			{
				if (selectedType == null)
					return null;

				return selectedBBValue.objectValue;
			}
			set
			{
				var t = selectedType;
				if (t == typeof(bool))
					boolValue.value = (bool)value;
				if (t == typeof(float))
					floatValue.value = (float)value;
				if (t == typeof(int))
					intValue.value = (int)value;
				if (t == typeof(string))
					stringValue.value = (string)value;
				if (t == typeof(Vector3))
					vectorValue.value = (Vector3)value;
				if (t == typeof(GameObject))
					goValue.value = (GameObject)value;
				if (t == typeof(Vector2))
					vector2Value.value = (Vector2)value;
				if (t == typeof(Color))
					colorValue.value = (Color)value;
			}
		}

		public List<string> availableTypeNames{
			get
			{
				var typeNames = new List<string>();
				foreach(System.Type t in availableTypes){
					if (t == null){
						typeNames.Add("None");
					} else {

						#if UNITY_EDITOR
						typeNames.Add(EditorUtils.TypeName(t));
						#endif

						#if !UNITY_EDITOR
						typeNames.Add(t.Name);
						#endif
					}
				}
				return typeNames;
			}
		}
	}
}