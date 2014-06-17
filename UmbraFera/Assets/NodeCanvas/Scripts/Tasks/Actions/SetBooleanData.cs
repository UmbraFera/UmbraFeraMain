using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptName("Set Boolean")]
	[ScriptCategory("Interop")]
	public class SetBooleanData : ActionTask{

		[RequiredField]
		public BBBool boolData = new BBBool{blackboardOnly = true};
		
		public enum SetMode{False, True, Toggle}
		public SetMode setTo = SetMode.True;

		protected override string actionInfo{
			get 
			{
				if (setTo == SetMode.Toggle)
					return "Toggle " + boolData.ToString();

				return "Set " + boolData.ToString() + " to " + setTo.ToString();			
			}
		}

		protected override void OnExecute(){
			
			if (setTo == SetMode.Toggle){
				
				boolData.value = !boolData.value;
		
			} else {

				var checkBool = ( (int)setTo == 1 );
				boolData.value = checkBool;
			}

			EndAction();
		}
	}
}