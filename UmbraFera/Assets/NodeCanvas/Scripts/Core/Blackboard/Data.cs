using UnityEngine;

namespace NodeCanvas.Variables{
	
	///Data are mostly stored in Blackboard. Derived classes of this store the correct type respectively depending on the class
	abstract public class Data : MonoBehaviour{

		public string dataName;

		///The Type this data holds
		virtual public System.Type dataType{
			get {return GetValue().GetType();}
		}

		///Get the Data value
		abstract public System.Object GetValue();
		
		///Set the Data value
		abstract public void SetValue(System.Object value);

		///Get the value in a serializable format for saving
		virtual public System.Object GetSerialized(){
			return GetValue();
		}

		///Set the value from a serializable format after loading
		virtual public void SetSerialized(System.Object obj){
			SetValue(obj);
		}

		//////////////////////////
		///////EDITOR/////////////
		//////////////////////////
		#if UNITY_EDITOR

		virtual public void ShowDataGUI(){
			/*
			var field = this.GetType().GetField("value");
			field.SetValue(this, EditorUtils.GenericField(dataName, field.GetValue(this), dataType));
			*/
		}

		#endif
	}
}