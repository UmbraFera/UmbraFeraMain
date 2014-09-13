using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FSM_NS
{
public class Condition : object  {

//if left <|>|= right
private string leftValue;
private string rightValue;
private string operand;
private List<Attribute> attributes;

//private int fromStateID;

	//-----------------------------------------------------------------------------
	/// <summary>
	/// Initializes a new instance of the <see cref="FSM_NS.Condition"/> class.
	/// </summary>
	public Condition()
		{
		
		}
	//-----------------------------------------------------------------------------	
	
	
	//-----------------------------------------------------------------------------
	/// <summary>
	/// Gets the save string.
	/// </summary>
	/// <returns>The save string.</returns>
	public string getSaveString()
	{
		string line = leftValue+operand+rightValue;
		return line;	
	}		
	//-----------------------------------------------------------------------------
	
	
	//-----------------------------------------------------------------------------
	/// <summary>
	/// Initializes a new instance of the <see cref="FSM_NS.Condition"/> class.
	/// </summary>
	/// <param name="a">The alpha component.</param>
	/// <param name="Attributes">Attributes.</param>
	public Condition(string a, List<Attribute> Attributes)
		{ //leftVal:operand:rightval
		
		    attributes=Attributes;
			
			
			string[] lessThanOrEqual    = Regex.Split(a,"<=");
			string[] moreThanOrEqual    = Regex.Split(a,">=");
			string[] notEqual   		= Regex.Split(a,"!=");
			string[] equal    			= a.Split('=');
			string[] greaterThan     	= a.Split('>');
			string[] lessThan    		= a.Split('<');
				
				if(lessThanOrEqual.Length==2)
					{
						leftValue  		= lessThanOrEqual[0];
						operand 		= "<=";
						rightValue 		= lessThanOrEqual[1];			
					}
			else
				if(moreThanOrEqual.Length==2)
					{
						leftValue  		= moreThanOrEqual[0];
						operand 		= ">=";
						rightValue 		= moreThanOrEqual[1];			
					}		
			else
				if(notEqual.Length==2)
					{
						leftValue  		= notEqual[0];
						operand 		= "!=";
						rightValue 		= notEqual[1];			
					}		
			else
				if(equal.Length==2)
					{
						leftValue  		= equal[0];
						operand 		= "=";
						rightValue 		= equal[1];			
					}		
			else
				if(greaterThan.Length==2)
					{
						leftValue  		= greaterThan[0];
						operand 		= ">";
						rightValue 		= greaterThan[1];			
					}		
			else
				if(lessThan.Length==2)
					{
						leftValue  		= lessThan[0];
						operand 		= "<";
						rightValue 		= lessThan[1];			
					}
		}
		//-----------------------------------------------------------------------------
		
		
	//-----------------------------------------------------------------------------
	/// <summary>
	/// Gets the value.
	/// </summary>
	/// <returns>The value.</returns>
	/// <param name="val">Value.</param>
	string getValue(string val)
	{
		foreach (Attribute a in attributes)
			{
			   if(a.name == val)return a.value;
			
			}
		return val;
	}
	//-----------------------------------------------------------------------------


	//-----------------------------------------------------------------------------
	/// <summary>
	/// Gets a value indicating whether this <see cref="FSM_NS.Condition"/> is true.
	/// </summary>
	/// <value><c>true</c> if true; otherwise, <c>false</c>.</value>
	public bool True
	{
	get{

			if(operand == "=")
			{
				return(getValue(leftValue) == getValue(rightValue));
			}
			if(operand == "!=")
			{
				return(getValue(leftValue) != getValue(rightValue));	
			}
			if(operand == "<")
			{
					return(float.Parse(getValue(leftValue)) < float.Parse(getValue(rightValue)));
			}
			if(operand == "<=")
			{
					return(float.Parse(getValue(leftValue)) <= float.Parse(getValue(rightValue)));
			}
			if(operand == ">")
			{
					return(float.Parse(getValue(leftValue)) > float.Parse(getValue(rightValue)));
			}
			if(operand == ">=")
			{
					return (float.Parse(getValue(leftValue))>= float.Parse(getValue(rightValue)));
			}

			return false;
		}
	}
	//-----------------------------------------------------------------------------


}
}