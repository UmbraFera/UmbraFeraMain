using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace FSM_NS
{
public class Action : object  {


private string leftValue;
private string rightValue;
private string operand;
private List<Attribute> attributes;
private string line;
public int id=0;	
	
public string name;


//--------------------------------------------------------
/// <summary>
/// Initializes a new instance of the <see cref="FSM_NS.Action"/> class.
/// </summary>
public Action()
	{
	line = "";
	}
//--------------------------------------------------------------------	
		
	
	
//--------------------------------------------------------------------	
/// <summary>
/// Initializes a new instance of the <see cref="FSM_NS.Action"/> class.
/// </summary>
/// <param name="a">The alpha component.</param>
/// <param name="Attributes">Attributes.</param>
public Action(string a, List<Attribute> Attributes)
		{ //attribute:leftVal:operand:rightval
	   		attributes=Attributes;
	   		line = a;//chached for future retrieval
	   	
	   		
	   		
	   		string[] part1  = a.Split('=');
	   		name 			= part1[0];
	   		//Debug.Log("a="+a);
			if(part1.Length<2)return;
			
		   string[] part2plus    = part1[1].Split('+');
		   string[] part2minus   = part1[1].Split('-');
		   string[] part2times   = part1[1].Split('*');
		   string[] part2divide  = part1[1].Split('/');
		
		  	if(part2plus.Length==2)
		    	{
					operand = "+";
					leftValue  		= part2plus[0];
					rightValue  	= part2plus[1];
		    	}
		    else
			if(part2minus.Length==2)
		    	{
					operand = "-";
					leftValue  		= part2minus[0];
					rightValue  	= part2minus[1];		    
		    	}
			else
			if(part2times.Length==2)
				{
					operand = "*";
					leftValue  		= part2times[0];
					rightValue  	= part2times[1];		    
				}		    
			else
			if(part2divide.Length==2)
				{
					operand = "/";
					leftValue  		= part2minus[0];
					rightValue  	= part2minus[1];		    
				}
				else
				{
					operand = "=";
					leftValue  		= part1[0];
					rightValue  	= part1[1];				 
				}	   		
		}
//--------------------------------------------------------------------


//--------------------------------------------------------------------
/// <summary>
/// Gets the save string.
/// </summary>
/// <returns>The save string.</returns>
	public string getSaveString()
	{
	return line;
	}
//--------------------------------------------------------------------


//--------------------------------------------------------------------
/// <summary>
/// Takes the action.
/// </summary>
public void takeAction()
	{
		Attribute target = getAttribute(name);
		if(target == null)return;
		
		string left = getValue(leftValue);
		string right = getValue(rightValue);
		
		if(operand == "=")
			{
			    target.value = right;
			
			}
		if(operand == "+")
			{
				float tempInt = float.Parse(left) + float.Parse(right);
				target.value = ""+tempInt;			
			}
		if(operand == "*")
			{
				float tempInt = float.Parse(left) * float.Parse(right);
				target.value = ""+tempInt;
			}		
		if(operand == "/")
			{
				float tempInt = float.Parse(left) / float.Parse(right);
				target.value = ""+tempInt;
			}	
		
		if(operand == "-")
			{
				float tempInt = float.Parse(left) - float.Parse(right);
				target.value = ""+tempInt;
			}									
	}
//--------------------------------------------------------------------	
	
	
//--------------------------------------------------------------------		
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
//--------------------------------------------------------------------	



//--------------------------------------------------------------------
	/// <summary>
	/// Gets the value.
	/// </summary>
	/// <returns>The value.</returns>
	/// <param name="val">Value.</param>
	public Attribute getAttribute(string val)
	{
		foreach (Attribute a in attributes)
		{
			if(a.name == val)return a;
			
		}
		return null;
	}
//--------------------------------------------------------------------

}
}
