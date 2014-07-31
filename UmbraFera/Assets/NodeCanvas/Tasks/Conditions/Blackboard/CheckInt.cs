using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[Category("✫ Blackboard")]
	public class CheckInt : ConditionTask{

		public enum CheckTypes
		{
			EqualTo,
			GreaterThan,
			LessThan
		}
		public BBInt valueA = new BBInt{blackboardOnly = true};
		public CheckTypes checkType = CheckTypes.EqualTo;
		public BBInt valueB;

		protected override string info{
			get
			{
				string symbol = " == ";
				if (checkType == CheckTypes.GreaterThan)
					symbol = " > ";
				if (checkType == CheckTypes.LessThan)
					symbol = " < ";
				return valueA + symbol + valueB;
			}
		}

		protected override bool OnCheck(){

			if (checkType == CheckTypes.EqualTo){
				if (valueA.value == valueB.value)
					return true;
				return false;
			}

			if (checkType == CheckTypes.GreaterThan){
				if (valueA.value > valueB.value)
					return true;
				return false;
			}

			if (checkType == CheckTypes.LessThan){
				if (valueA.value < valueB.value)
					return true;
				return false;
			}

			return true;
		}
	}
}