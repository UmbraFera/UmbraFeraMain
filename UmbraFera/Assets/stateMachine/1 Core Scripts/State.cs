using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FSM_NS
{
public class State : object {
public List<int> events;
public List<FSMEvent> eventBucket;
public string name;
public int id=0;

//-----------------------------------------------------------------
/// <summary>
/// Initializes a new instance of the <see cref="State"/> class.
/// </summary>
/// <param name="line">Line.</param>
	public State(string line)
	{
		eventBucket=new List<FSMEvent>();
	   if(events == null) events = new List<int>();
			//name,id,e1:e2...
	   string[] s=line.Split(',');
	   name = s[0];
	   id = int.Parse(s[1]);
	   
	   string[] aStrings = s[2].Split(':');
	   
	  // Debug.Log("line:"+line);
	   
	   foreach(string eventID in aStrings)
	   {
				if(eventID.Length>0)
				{
					//Debug.Log(name+"  id:"+eventID);
					int tempID = int.Parse(eventID);
					//Debug.Log("value:"+tempID);
					events.Add(tempID);
	      		}
	      
	   }
	   
	   
	}
//----------------------------------------------------------------	    
	
	
//----------------------------------------------------------------
	/// <summary>
	/// Gets the save string.
	/// </summary>
	/// <returns>The save string.</returns>
	public string getSaveString()
	{
	   string line = name+","+id+",";
	   for(int i =0;i<events.Count;i++)
	   {
	   	if(i<events.Count-1)
	   	{
				line = line+events[i]+":";
	   	}
	   	else
	   	{
				line = line+events[i];
	   	}
	   
	   }
	
	return line;
	}
//-----------------------------------------------------------------------------	


//-----------------------------------------------------------------------------
	// Use this for initialization
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
		if(events == null)
			events = new List<int>();
	}
//-----------------------------------------------------------------------------	
	
	
//-----------------------------------------------------------------------------
	/// <summary>
	/// determine if the state as this action
	/// </summary>
	/// <returns><c>true</c>, if action was hased, <c>false</c> otherwise.</returns>
	/// <param name="actionID">Action I.</param>
	public bool hasEvent(int eventID)
	{
		return events.Contains(eventID);
	}
//-----------------------------------------------------------------------------	


//-----------------------------------------------------------------------------
	public void addEvent(int eventID)
	{
		events.Add(eventID);
	}
//-----------------------------------------------------------------------------	

}
}
