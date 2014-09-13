using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace FSM_NS
{
public class FSMEvent : object  {

public int toStateID;
public State toState;
public int id=0;	
public string name;
public  List<Condition> conditions;
public  List<Action> actions;
public string action;
private List<Attribute> attributes;

///--------------------------------------------------------------------
	/// <summary>
	/// Initializes a new instance of the <see cref="FSM_NS.FSMEvent"/> class.
	/// </summary>
	public FSMEvent()
	{
	
	}
///--------------------------------------------------------------------	


///--------------------------------------------------------------------			
	/// <summary>
	/// Initializes a new instance of the <see cref="FSMEvent"/> class.
	/// </summary>
	/// <param name="e">E.</param>
	/// <param name="Attributes">Attributes.</param>
	public FSMEvent(string e, List<Attribute> Attributes)
	{ //name,id,destination,condition1 condition2 ...,action1 action2 ...
	
		attributes=Attributes;
		conditions 	= new List<Condition>();
		actions 	= new List<Action>();
		
		string[] parts  = e.Split(',');
	   	name 			= parts[0];
	   	id  			= int.Parse(parts[1]);
	   	toStateID 		= int.Parse(parts[2]);
	   	
	   	if(parts.Length>3)//add condition
	   	{
			string [] condList = parts[3].Split(':');
			
			foreach(string c in condList)
			{
			    if(c.Length>0)
			    {
				Condition condition = new Condition(c,Attributes);
				conditions.Add(condition);
				}
			}	
	   	}
	   	
		if(parts.Length>4)//add action
		{
		
			string [] actionList = parts[4].Split(':');
			
			foreach(string a in actionList)
			{
				Action action = new Action(a,Attributes);
				actions.Add(action);
			}			
		}
	}
///--------------------------------------------------------------------	


///--------------------------------------------------------------------	
	/// <summary>
	/// Gets the save string.
	/// </summary>
	/// <returns>The save string.</returns>
	public string getSaveString()
	{
		string line = name+","+id+","+toStateID+",";    
	    
	    //conditions
	    for(int i=0 ; i<conditions.Count ; i++)
	    {
			if(i<conditions.Count-1)
			{
				line = line+conditions[i].getSaveString()+":";
			}
			else
			{
				line = line+conditions[i].getSaveString()+",";
			}
	    
	    }
		//actions
		for(int i=0 ; i<actions.Count ; i++)
		{
			if(i<actions.Count-1)
			{
				line = line+actions[i].getSaveString()+":";
			}
			else
			{
				line = line+actions[i].getSaveString();
			}
			
		}	
	
		return line;	
	}	
///--------------------------------------------------------------------		
	
	
///--------------------------------------------------------------------		
	/// <summary>
	/// Takes the actions.
	/// </summary>
	/// <param name="custAction">Cust action.</param>
	public void takeActions(string custAction)
	{
		Action act=new Action(custAction,attributes);
		act.takeAction();
		
		foreach (Action a in actions)
		{
			a.takeAction();
			
		}
		
	}
///--------------------------------------------------------------------	

	
///--------------------------------------------------------------------				
	/// <summary>
	/// Takes the action. THis is here so you can pass in a custom action from your code that wasnt in the state machine definition file
	/// </summary>
	/// <param name="custAction1">Cust action1.</param>
	/// <param name="custAction2">Cust action2.</param>
	public void takeActions(string custAction1, string custAction2)
	{
		Action a1=new Action(custAction1,attributes);
		a1.takeAction();
		Action a2=new Action(custAction2,attributes);
		a2.takeAction();
				
		foreach (Action a in actions)
		{
			a.takeAction();
			
		}
		
	}	
///--------------------------------------------------------------------	
		
		
		
///--------------------------------------------------------------------	
	/// <summary>
	/// Takes all actions.
	/// </summary>
	public void takeActions()
	{
	  foreach (Action a in actions)
		  {
		     a.takeAction();
		  
		  }
	}
///--------------------------------------------------------------------		
	
	
///--------------------------------------------------------------------		
	/// <summary>
	/// Gets a value indicating whether this <see cref="FSMEvent"/> conditions met.
	/// </summary>
	/// <value><c>true</c> if conditions met; otherwise, <c>false</c>.</value>
	public bool conditionsMet{
		get{
			
	     
			if(conditions == null)return true;
			//Debug.Log("Cond Count="+conditions.Count);
	   		foreach(Condition c in conditions)
	   			{
	      			if(!c.True)
							{
								//Debug.Log("FAILED Condition: "+c.getSaveString());
							   return false;
								}
	   			}
	   		return true;
	   		}
	}
///--------------------------------------------------------------------		





}
}
