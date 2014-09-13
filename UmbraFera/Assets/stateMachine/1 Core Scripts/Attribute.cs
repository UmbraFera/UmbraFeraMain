using UnityEngine;
using System.Collections;
using System.Collections.Generic;



namespace FSM_NS
{
public class Attribute : object {
public string name;
public string value;



/// <summary>
/// Initializes a new instance of the <see cref="FSM_NS.Attribute"/> class.
/// </summary>
/// <param name="line">Line.</param>
public Attribute(string line)
{
//Debug.Log("att "+line);
	//name,value
   string[] s=line.Split('=');
   name = s[0];
	if(s.Length>1)
      value = s[1];
	else
	   value = "";
}

//--------------------------------------------------------------------

//--------------------------------------------------------------------
/// <summary>
/// Add the specified val.
/// </summary>
/// <param name="val">Value.</param>
	public void add( string val)
	{
		int value_ = int.Parse(val);
		
		int newVal = int.Parse(value) + value_;
		value = ""+newVal;
	}
//--------------------------------------------------------------------
		
			
//--------------------------------------------------------------------					
	/// <summary>
	/// Multiply the specified val.
	/// </summary>
	/// <param name="val">Value.</param>
	public void multiply(string val)
	{
		int value_ = int.Parse(val);
		
		int newVal = int.Parse(value) * value_;
		value = ""+newVal;		
	}
//--------------------------------------------------------------------	


//--------------------------------------------------------------------
	/// <summary>
	/// Sets the attribute.
	/// </summary>
	/// <param name="value_">Value_.</param>
	public void setAttribute( string value_)
	{
		value = value_;		
	}
//--------------------------------------------------------------------	


//--------------------------------------------------------------------
	/// <summary>
	/// Gets the save string.
	/// </summary>
	/// <returns>The save string.</returns>
	 public string getSaveString()
	 {
	 	return this.name+"="+value;
	 }
//--------------------------------------------------------------------

}
}
