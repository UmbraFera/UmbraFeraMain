using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("✫ Blackboard")]
	[Description("Set a blackboard float variable")]
	public class SetInt : ActionTask{

		public enum SetMode
		{
			SET,
			ADD,
			SUBTRACT,
			MULTIPLY
		}
		public BBInt valueA = new BBInt{blackboardOnly = true};
		public SetMode Operation = SetMode.SET;
		public BBInt valueB;

		protected override string info{
			get
			{
				if (Operation == SetMode.SET)
					return "Set " + valueA + " = " + valueB;

				if (Operation == SetMode.ADD)
					return "Set " + valueA + " += " + valueB;
				
				if (Operation == SetMode.SUBTRACT)
					return "Set " + valueA + " -= " + valueB;

				if (Operation == SetMode.MULTIPLY)
					return "Set " + valueA + " *= " + valueB;

				return string.Empty;			
			}
		}

		protected override void OnExecute(){

			if (Operation == SetMode.SET){
				valueA.value = valueB.value;
			} else
			if (Operation == SetMode.ADD){
				valueA.value += valueB.value;
			} else
			if (Operation == SetMode.SUBTRACT){
				valueA.value -= valueB.value;
			} else
			if (Operation == SetMode.MULTIPLY){
				valueA.value *= valueB.value;
			}

			EndAction(true);
		}
	}
}