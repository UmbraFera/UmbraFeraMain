using UnityEngine;
using System.Collections;

namespace NodeCanvas.Actions{

	[ScriptCategory("Interop")]
	public class CheckExpression : ConditionTask {

		public string expression;

		private string leftVar;
		private string operation;
		private string rightVar;

		private System.Type type;
		private object leftValue;
		private object rightValue;

		private string error;

		protected override string conditionInfo{
			get {return string.IsNullOrEmpty(error)? "'" + expression + "'" : error;}
		}

		protected override bool OnCheck(){

			string[] words = expression.Split(' ');
			if (words.Length != 3 || string.IsNullOrEmpty(words[2]))
				return Error("Wrong format");

			leftVar        = words[0];
			operation      = words[1];
			rightVar       = words[2];

			leftValue = blackboard.GetDataValue<object>(leftVar);

			if (leftValue == null)
				return Error("No variable exists");

			type = leftValue.GetType();

			rightValue = null;
			var temp = blackboard.GetData(rightVar, type);
			if (temp != null)
				rightValue = temp.GetValue();

			error = null;
			try
			{
				if (type == typeof(bool))
					return BoolCheck();
				if (type == typeof(float))
					return FloatCheck();
				if (type == typeof(int))
					return IntCheck();				
			}
			catch
			{
				return Error("Parsing Error");
			}

			return Error("Unsupported Variable Type");
		}

		bool Error(string err){
			error = "<color=#d63e3e>" + err + "</color>";
			return false;
		}

		bool BoolCheck(){

			if (rightValue == null)
				rightValue = bool.Parse(rightVar);

			if (operation == "==")
				return (bool)leftValue == (bool)rightValue;

			if (operation == "!=")
				return (bool)leftValue != (bool)rightValue;

			return Error("Wrong Format");
		}

		bool FloatCheck(){

			if (rightValue == null)
				rightValue = float.Parse(rightVar);

			if (operation == "==")
				return (float)leftValue == (float)rightValue;

			if (operation == "!=")
				return (float)leftValue != (float)rightValue;

			if (operation == ">")
				return (float)leftValue > (float)rightValue;

			if (operation == "<")
				return (float)leftValue < (float)rightValue;
		
			if (operation == ">=")
				return (float)leftValue >= (float)rightValue;

			if (operation == "<=")
				return (float)leftValue <= (float)rightValue;

			return Error("Wrong Format");
		}

		bool IntCheck(){

			if (rightValue == null)
				rightValue = int.Parse(rightVar);

			if (operation == "==")
				return (int)leftValue == (int)rightValue;

			if (operation == "!=")
				return (int)leftValue != (int)rightValue;

			if (operation == ">")
				return (int)leftValue > (int)rightValue;

			if (operation == "<")
				return (int)leftValue < (int)rightValue;
		
			if (operation == ">=")
				return (int)leftValue >= (int)rightValue;

			if (operation == "<=")
				return (int)leftValue <= (int)rightValue;

			return Error("Wrong Format");
		}


	}
}