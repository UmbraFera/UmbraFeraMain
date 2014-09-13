using UnityEngine;
using System.Collections;

namespace FSM_NS{
public abstract class stateController : MonoBehaviour {
   public FSM myStateMachine;
   public string currentState;
   public float delayOffTime=2.0f;
   private bool repeating=true;
   private float timeLeft  = 0;
   public bool tickerOn=true;
   public string tickerEventName="tick";
   public float totalTimeInSeconds  = 1;
   public int startStateID=0;
   public abstract void tickFired();

   
   
		//----->TIMER FUNCTIONS<---------//
		/// <summary>
		/// Turns the timer on.
		/// </summary>
		public void turnOn()
		{
			timeLeft = totalTimeInSeconds;
			tickerOn = true;
		}//----------------------------------------
		
		
		//----------------------------------------
		/// <summary>
		/// Turns the timer off.
		/// </summary>
		public void turnOff()
		{
			tickerOn = false;
		}//----------------------------------------
		
		
		//----------------------------------------
		/// <summary>
		/// Starts the new repeating timer.
		/// </summary>
		/// <param name="theTarget">The target.</param>
		/// <param name="theTargetMethod">The target method.</param>
		/// <param name="length">Length.</param>
		public void startNewRepeatingTimer(GameObject theTarget, string theTargetMethod, float length)
		{
			repeating = true;
			totalTimeInSeconds = length;
			turnOn();
		}//----------------------------------------	
		
		
		//----------------------------------------
		/// <summary>
		/// Starts the new timer.
		/// </summary>
		/// <param name="theTarget">The target.</param>
		/// <param name="theTargetMethod">The target method.</param>
		/// <param name="length">Length.</param>
		public void startNewTimer(GameObject theTarget, string theTargetMethod, float length)
		{
			totalTimeInSeconds = length;
			turnOn();
		}//----------------------------------------
		
		//----------------------------------------
		/// <summary>
		/// update from the extended class.
		/// </summary>
	 public void superUpdate () 
		{
			fireTimer();
		}//----------------------------------------
		
		
		//----------------------------------------
		/// <summary>
		/// Fires the timer.
		/// </summary>
		private void fireTimer()
		{
		
			if (tickerOn) 
			{
				//Debug.Log(".."+timeLeft);
				timeLeft -= Time.deltaTime;
				
				if (timeLeft < 0) 
				{
					timeLeft = totalTimeInSeconds;								
					eventToFSM = "tick";
					tickFired();
					
					if(repeating)turnOn();
					else
					{	
						turnOff();
						GameObject.DestroyImmediate(gameObject);
					}		
				}
				
			}
		}//----------------------------------------
		
   
   
	//----------------------------------------
	/// <summary>
	/// state transition callback
	/// </summary>
	public void enteredState(string stateName)
	{
		currentState  =  stateName;
		Invoke("Entered_State_"+stateName,0.0f);
	}
	//----------------------------------------
	
	
	//----------------------------------------
	/// <summary>
	/// Leaves the state.
	/// </summary>
	/// <param name="stateName">State name.</param>
	public void leftState(string stateName)
	{
		Invoke("Left_State_"+stateName,0.0f);
	}//----------------------------------------
	
	//----------------------------------------
	/// <summary>
	/// Recieves the event.
	/// </summary>
	/// <param name="event_">Event_.</param>
	public void recieveEvent(string event_)
	{
			//Debug.Log("clicked "+event_);
		myStateMachine.event_(event_);	
		}//----------------------------------------
	
	//-----------------------------------------------------------
	/// <summary>
	/// Sends the event to FSM
	/// </summary>
	/// <value>The event to FS.</value>
	public string eventToFSM
	{
	  set 
	    {
			myStateMachine.event_(value);
		}
	}
	//-----------------------------------------------------------
	
	public void timerFired()
	{
		eventToFSM = tickerEventName;
	}

}
}
