using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//--------------------------------------------------------------------
namespace FSM_NS
{
public class FSM :object  {


	private List<State> 	  states;
	private List<FSMEvent> 	events;
	private List<Attribute> attributes;
	public static int 		  stateCount=0;
	public static int 		  actionCount=0;
	public stateControllerInterface target;
	private State           currentState;	
	private string          definition;	
	
//--------------------Constructors----------------------------------------------


//------------------------------------------------------------------
/// <summary>
/// Initializes a new instance of the <see cref="FSM_NS.FSM"/> class.
/// </summary>
/// <param name="line">Line.</param>
public FSM(string line)
	{
	
	    //get rid of extra formating used to make human readable text file
		line = line.Replace(" ","");
		line = stripComments(line.Split('\n'));
		
	    loadStateMachine(line);
	    state = states[0];
	}
//------------------------------------------------------------------
		
		
//-----------------------------------------------------------------	
/// <summary>
/// Gets the name of the current state
/// </summary>
/// <value>The name.</value>
public string name {
	
	get{
		return state.name;
	}
}
//-----------------------------------------------------------------	
	
	
//-----------------------------------------------------------------
/// <summary>
/// Initializes a new instance of the <see cref="FSM_NS.FSM"/> class.
/// </summary>
/// <param name="line">Line.</param>
/// <param name="target_">Target_.</param>
public FSM(string line, stateControllerInterface target_)
	{
		
	  target = target_;
		line = line.Replace(" ","");
		line = stripComments(line.Split('\n'));

		loadStateMachine(line);
		state = states[0];
		
	}
//-----------------------------------------------------------------


//-----------------------------------------------------------------
/// <summary>
/// constructor to restore state machine
/// </summary>
/// <param name="line">Line.</param>
/// <param name="target_">Target_.</param>
/// <param name="currentStateID">Current state I.</param>
public FSM(string line, stateControllerInterface target_, int currentStateID)
	{
		target = target_;
		line = line.Replace(" ","");
		loadStateMachine(line);
		state = getState(currentStateID);
	}
//-----------------------------------------------------------------	


//-----------------------------------------------------------------
/// <summary>
/// Strips the comments from the input file
/// </summary>
/// <returns>The comments.</returns>
/// <param name="lines">Lines.</param>
private string stripComments(string[] lines)	
{
	string outstring="";
	foreach(string line in lines)
	{
		if( line.Length>0)
		 {
		  if(line.Length>1 && !(line[0]=='/' && line[1]=='/'))
		   {
				outstring=outstring+line;
			}
			if(line.Length==1)outstring=outstring+line;
		 }
		
	}
	return outstring;
}
//-----------------------------------------------------------------


//-----------------------------------------------------------------
/// <summary>
/// Serielizes the State Machine. 
/// </summary>
/// <returns>The FS.</returns>
	public string serielizeFSM()
	{
	    string statesString = getStatesString();
		string attributesString = getAttributesString();
		string eventsString = getEventsString();
		string line = statesString+"|"+attributesString+"|"+eventsString;
		
		return line;
	}
//-----------------------------------------------------------------	


	//-----------------------------------------------------------------	
	/// <summary>
	/// Gets the states string.
	/// </summary>
	/// <returns>The states string.</returns>
	private string  getStatesString ()
	{
		string s="";
		s=currentState.getSaveString()+";";
		for(int i=0;i<states.Count;i++)
			{
				if(states[i]!=currentState)
				{
					if(i<states.Count-1)
						{
							s=s+states[i].getSaveString()+";";
						}
					else
						{
							s=s+states[i].getSaveString();
						}
					
				}
			}
		return s;
	}
//-----------------------------------------------------------------	
		
			
//-----------------------------------------------------------------				
	/// <summary>
	/// Gets the attributes string.
	/// </summary>
	/// <returns>The attributes string.</returns>
	private string  getAttributesString ()
	{
		string s="";
		for(int i=0;i<attributes.Count;i++)
			{
				if(i<attributes.Count-1)
					{
						
						s=s+attributes[i].getSaveString()+";";
					}
				else
					{
						s=s+attributes[i].getSaveString();
						
					}
				
			}
		return s;
	}
	//-----------------------------------------------------------------	
	
	
//-----------------------------------------------------------------	
	/// <summary>
	/// Gets the events string.
	/// </summary>
	/// <returns>The events string.</returns>
	private string  getEventsString ()
	{
		string s="";
		for(int i=0;i<events.Count;i++)
			{
				if(i<events.Count-1)
					{
						
						s=s+events[i].getSaveString()+";";
					}
				else
					{
						s=s+events[i].getSaveString();
						
					}
				
			}
		return s;
	}
	//------------------------------------------------------------------
	
	
	//-----------------------------------------------------------------		
	/// <summary>
	/// Sends event to the statecontroller that FSM just left a specified state
	/// </summary>
	private void fireStateLeft()
		{
			if(target==null)return;
			target.leftState(state.name);		
		}
	//------------------------------------------------------------------	
		
 		
		
	//------------------------------------------------------------------
	/// <summary>
	/// Sends even to the statecontroller that FSM just entered a specified state. This the main even used in the controller to control the event flow
	/// </summary>
	private void fireStateEntered()
		{
		  //Debug.Log("fireStateEntered");
			if(target==null)return;
			target.enteredState(state.name);			
		}
    //------------------------------------------------------------


	//------------------------------------------------------------------
	/// <summary>
	/// Gets or sets the state.
	/// </summary>
	/// <value>The state.</value>
	public State state
		{
		 get
			 {
			   return currentState;
			 }
		 
		 set
			 {
			   currentState = value;
			 }
		}
	//------------------------------------------------------------
	
	/// <summary>
	/// Take this action and update the state. Does nothing if action not in the states action set
	/// </summary>
	/// <param name="theAction">The action.</param>
	//------------------------------------------------------------
	public void event_ (string theEvent)
		{
		if(!isEventAvailable(theEvent))
		    {
			  return;
		    }
        
		FSMEvent thisEvent = getEvent(theEvent);
		
		if(thisEvent==null)
		    {
		      return;
			}
		
		//process actions if any
		thisEvent.takeActions();
		
		State newState= thisEvent.toState;//  getState(thisEvent.toStateID );
		
		if(state != newState)
			{
				//sends message to target in-case it cares
				fireStateLeft();
				
				//update current state
				state = newState ;
							
				fireStateEntered();
					
			}
		}
	//------------------------------------------------------------




	//------------------------------------------------------------------
	/// <summary>
	/// Event_ that took place, include custom actionString1 and actionString2.
	/// </summary>
	/// <param name="theEvent">The event.</param>
	/// <param name="actionString1">Action string1.</param>
	/// <param name="actionString2">Action string2.</param>
	public void event_ (string theEvent,string actionString1,string actionString2)
		{
			if(!isEventAvailable(theEvent))
			    return;
			
			FSMEvent thisEvent = getEvent(theEvent);
			
			if(thisEvent==null)
				return;
			
			//process actions if any
			thisEvent.takeActions(actionString1, actionString2);
			
		
			State newState= thisEvent.toState;//  getState(thisEvent.targetState );
			
			if(state != newState)
			{
				//sends message to target in-case it cares
				fireStateLeft();
				
				//update current state
				state = newState ;
				
				fireStateEntered();
				
			}
		
		}
	//------------------------------------------------------------


/// <summary>
/// Event_ that took place. include custom actionString.
/// </summary>
/// <param name="theEvent">The event.</param>
/// <param name="actionString">Action string.</param>
	public void event_ (string theEvent,string actionString)
	{		
		//if(!isEventAvailable(theEvent))return;
		
		FSMEvent thisEvent =  getEvent(theEvent);
		
		if(thisEvent==null)
			return;
		
		//process actions if any]
		
		thisEvent.takeActions(actionString);
		
		State newState= thisEvent.toState;//  getState(thisEvent.targetState );
		
		if(state != newState)
		{
			//sends message to target in-case it cares
			fireStateLeft();
			
			//update current state
			state = newState ;
			
			fireStateEntered();
			
		}
		
	 }
	//------------------------------------------------------------
	

	
	//------------------------------------------------------------		
	/// <summary>
	/// grabs events for the current state
	/// </summary>
	/// <returns>The action.</returns>
	/// <param name="action">Action.</param>
	public FSMEvent getEvent(string event_)
	{
	
	foreach(FSMEvent e in state.eventBucket)
	{
	   if(event_ == e.name)
	   {
			if(e.conditionsMet)
				return e;	     
	   }
	}
	
	/*
		ArrayList eventSet = getEvents(event_);
		
		foreach(FSMEvent e in eventSet)
		{
			if(state.hasEvent(e.id))
			{
			   if(e.conditionsMet)
				  return e;
			}
		}	
		*/
		return null;
	}	
	//-------------------------------------------------------
	
	//------------------------------------------------------------
	/// <summary>
	/// Helper to get action from action name
	/// </summary>
	/// <returns>The action.</returns>
	/// <param name="action">Action.</param>
	private FSMEvent getEvent (State s, int event_)
	{
		foreach(int e in s.events)
		{
			if(e == event_)
			{	
				return getEvent(e);
			}
		}
		return null;
	}
	//------------------------------------------------------------	
	
	
	
	//------------------------------------------------------------	
	/// <summary>
	/// Gets the event.
	/// </summary>
	/// <returns>The event.</returns>
	/// <param name="event_">Event_.</param>
	private FSMEvent getEvent (int event_)
	{
		
		foreach(FSMEvent e in events)
		{
			if(e.id == event_)
			{
				return e;
			}
		}
		return null;
	}
	//------------------------------------------------------------
	
	
	//------------------------------------------------------------
	/// <summary>
	/// Gets the events.
	/// </summary>
	/// <returns>gets all events with this name.</returns>
	/// <param name="event_">Event_.</param>
	public ArrayList getEvents (string event_)
	{			
		ArrayList eventSet = new ArrayList();
		string lowercase=event_.ToLower();
		foreach(FSMEvent e in events)
		{
		
			if(e.name.ToLower() == lowercase)
			{
				
				eventSet.Add(e);
			}
		}
		return eventSet;
	}	
	//------------------------------------------------------------	
	
	
					
	//------------------------------------------------------------
	/// <summary>
	/// Gets the attribute with provived name
	/// </summary>
	/// <returns>The attribute.</returns>
	/// <param name="name">Name.</param>
	Attribute getAttribute(string name)
	{
		string lowercaseName = name.ToLower();
	   foreach (Attribute a in attributes)
	   {
	       if(a.name.ToLower() == lowercaseName)return a;
	   
	   }
	   return null;
	}
	
	//------------------------------------------------------------
		
						
	//------------------------------------------------------------
	/// <summary>
	/// checks to see if the event is available.
	/// </summary>
	/// <returns><c>true</c>, if event available was ised, <c>false</c> otherwise.</returns>
	/// <param name="event_">Event_.</param>
	public bool isEventAvailable(string event_)
	{
		ArrayList eventSet = getEvents(event_);		
		foreach(FSMEvent e in eventSet)
		{
			if(state.hasEvent(e.id))
			{
			   return true;
			}
		}	
		
	   return false;
	}
	//------------------------------------------------------------	
	
	
	
	//------------------------------------------------------------
	/// <summary>
	/// Loads the state machine.
	/// </summary>
	/// <param name="sm">Sm.</param>
	public void loadStateMachine(string sm)
	{
		states = new List<State>();
		events = new List<FSMEvent>();
	    attributes = new List<Attribute>();
		//states|attributes|events
		string[] one = sm.Split('|');

		//states
		//name,id,event1:event2...;name,id,act1....
		string[] stateStrings = one[0].Split(';');//states
		foreach(string s in stateStrings)
			{
				State newState = new State(s);
				states.Add(newState);
			}
	   	   
		//attributes
		//name,val;name,val...
		definition = one[1];
		string[] attributeStrings = one[1].Split(';');//attributes
		foreach(string a in attributeStrings)
			{
			    if(a.Length>0)
			    {
				Attribute newAttribute = new Attribute(a);
				attributes.Add(newAttribute);
				}
			}	
		
		
		//events
	   	//name,id,source,destination;name,id,source,dest...
		string[] eventStrings = one[2].Split(';');//events
		foreach(string e in eventStrings)
			{
				if(e.Length>0)
				{
				FSMEvent newEvent = new FSMEvent(e,attributes);
				events.Add(newEvent);
				}
			}	
			
		 //load events to states
		 foreach(State theState in states)
		 {
		    foreach(int eventID in theState.events)
		    {
		     FSMEvent e=  getEvent(eventID);
		     if(e!=null)
		     theState.eventBucket.Add(e);
		    }
		    
		 }   
			//load target state to event
			foreach(FSMEvent e in events)
			{
               e.toState = getState(e.toStateID);
				
			} 	
	}//------------------------------------------------------------
	
	
	
	//------------------------------------------------------------	
	/// <summary>
	/// Helper to get state from state name
	/// </summary>
	/// <returns>The state.</returns>
	/// <param name="mystate">Mystate.</param>
	private State getState (int mystate)
	{
		foreach(State s in states)
		{
			if(s.id == mystate)return s;
		}
		return null;
	}	
	//------------------------------------------------------------

	
	//------------------------------------------------------------			
	/// <summary>
	/// Gets the attribute display string.
	/// </summary>
	/// <value>The attribute display string.</value>
	public string attributeDisplayString
	{
		get{
			     string outString="";
				  foreach(Attribute a in attributes)
					   {
				outString=outString+a.name+" = "+a.value+"\n";
					   }
						return outString;
		}
	}
		
	//------------------------------------------------------------		
	/// <summary>
	/// Prints attributes for testing purposes.
	/// </summary>
	public void printAttributes()
	{
	   foreach(Attribute a in attributes)
	   {
	      Debug.Log("Attribute:"+a.name+" value:"+a.value);
	   }
	}
	//------------------------------------------------------------

	//------------------------------------------------------------
	/// <summary>
	/// Sets the attribute.
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="val">Value.</param>
	public void setAttribute (string name, int val)
	{
		foreach(Attribute a in attributes)
		{
			if(a.name.ToLower()==name.ToLower())
			{
			   a.setAttribute(val+"");
			}
		}	
	}	
	//------------------------------------------------------------	
	
			
					
		//------------------------------------------------------------
		/// <summary>
		/// Sets the attribute.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="val">Value.</param>
		public void setAttribute (string name, float val)
		{
			foreach(Attribute a in attributes)
			{
				if(a.name.ToLower()==name.ToLower())
				{
					a.setAttribute(val+"");
					return;
				}
			}	
		}	
		//------------------------------------------------------------	

		//------------------------------------------------------------
		/// <summary>
		/// Sets the attribute.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="val">Value.</param>
		public void setAttribute (string name, bool val)
		{
			foreach(Attribute a in attributes)
			{
				if(a.name.ToLower()==name.ToLower())
				{
					a.setAttribute(val+"");
					return;
				}
			}	
		}	
		//------------------------------------------------------------	
		
		//------------------------------------------------------------
		/// <summary>
		/// Sets the attribute.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="val">Value.</param>
		public void setAttribute (string name, long val)
		{
			foreach(Attribute a in attributes)
			{
				if(a.name.ToLower()==name.ToLower())
				{
					a.setAttribute(val+"");
					return;
				}
			}	
		}	
		//------------------------------------------------------------	
		
	//------------------------------------------------------------
	/// <summary>
	/// Sets the attribute.
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="val">Value.</param>
	public void setAttribute (string name, string val)
	{
		foreach(Attribute a in attributes)
		{
			if(a.name.ToLower()==name.ToLower())
			{
			   a.setAttribute(val);
					return;
			}
		}	
	}
	//------------------------------------------------------------
	
						
	//------------------------------------------------------------
	/// <summary>
	/// Gets the atribute value.
	/// </summary>
	/// <returns>The atribute value.</returns>
	/// <param name="name">Name.</param>
	public string getAtributeValue (string name)
	{
	
		foreach(Attribute a in attributes)
		{
			if(a.name.ToLower()==name.ToLower())
			{
				return a.value;
			}
		}	
		return "";
	}
	//------------------------------------------------------------	

	
	
	//------------------------------------------------------------	
	/// <summary>
	/// Shortcut jump to a specified state. This is normally used for example to exit back to a menu or start state.
	/// </summary>
	/// <param name="stateID">State I.</param>
	public void jumpToState (int stateID)
	{
		state = getState(stateID);
		fireStateEntered();
	}
	//------------------------------------------------------------
	
	
								
	//------------------------------
	/// <summary>
	/// Reset the State Machine
	/// </summary>
	public void reset()
	{
		   attributes.Clear();
			string[] attributeStrings = definition.Split(';');//attributes
			foreach(string a in attributeStrings)
				{
				    if(a.Length>0)
				    {
					Attribute newAttribute = new Attribute(a);
					attributes.Add(newAttribute);
					}
				}	
	}
//-----------------------------
												
									
										
												
}
}
