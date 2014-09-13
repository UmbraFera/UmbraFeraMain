using FSM_NS;
using UnityEngine;
using System.Collections;

public class gameController: stateController ,stateControllerInterface 
{

 public GameObject target;

void Start ()
  {
    string pushString = FSM_NS.HelperFile.getTextFileFromResource("gameControllerFSM");
    myStateMachine = new FSM_NS.FSM(pushString,this);

    myStateMachine.jumpToState(startStateID);
   }

void Update () 
  {
	//Your code here
	if(target!=null)
		if(currentState == "playing")
		{
		  target.transform.Rotate(new Vector3(.3f,.5f,.8f));
		}
	
	superUpdate();

  }



	void OnGUI() 
	{
	
	   if(currentState == "menu")
	   {
		if (GUI.Button(new Rect(Screen.width/2-200, Screen.height/2-100, 400, 200), "PLAY GAME!"))
			{
			   
			   eventToFSM = "playClicked";
			}
			
		}
		if(currentState == "playing")
		{
			GUI.Box(new Rect(Screen.width/2-200, 20, 400, 40), "PLAYING GAME NOW");
			
			if (GUI.Button(new Rect(10, 10, 30, 30), "||"))
			{
				
				eventToFSM = "pauseClicked";
			}
			if (GUI.Button(new Rect(Screen.width/2-50, 100, 100, 30), "won"))
			{
				eventToFSM = "won";
			}
			if (GUI.Button(new Rect(Screen.width/2-50, 140, 100, 30), "lost"))
			{
				eventToFSM = "lost";
			}
			if (GUI.Button(new Rect(Screen.width/2-50, 180, 100, 30), "quit"))
			{
				eventToFSM = "quit";
			}
			if (GUI.Button(new Rect(Screen.width/2-50, 220, 100, 30), "level up"))
			{
				eventToFSM = "levelUp";
			}
			
		}
		if(currentState == "paused")
		{
			if (GUI.Button(new Rect(Screen.width/2-200, Screen.height/2-100, 400, 200), "Resume!"))
			{
				
				eventToFSM = "screenClicked";
			}
			
		}
		
		if(currentState == "won")
		{
		
			GUI.Box(new Rect(Screen.width/2-200, 20, 400, 40), "Congtratulations, you won!");
			
			if (GUI.Button(new Rect(Screen.width/2-75, 180, 150, 50), "Continue"))
			{
				
				eventToFSM = "done";
			}
			
		}
		if(currentState == "lost")
		{
			
			GUI.Box(new Rect(Screen.width/2-200, 20, 400, 40), "Sorry, you lost:(");
			
			if (GUI.Button(new Rect(Screen.width/2-75, 180, 150, 50), "Continue"))
			{
				
				eventToFSM = "done";
			}
			
		}
		if(currentState == "gameOver")
		{
			
			GUI.Box(new Rect(Screen.width/2-200, 20, 400, 40), "GAME OVER");
			
			if (GUI.Button(new Rect(Screen.width/2-75, 150, 150, 50), "Start New Game"))
			{
				
				eventToFSM = "new";
			}
			if (GUI.Button(new Rect(Screen.width/2-75, 220, 150, 50), "Back to Menu"))
			{
				
				eventToFSM = "replay";
			}
			
		}
		if(currentState == "levelUp")
		{
			
			GUI.Box(new Rect(Screen.width/2-300, 20, 600, 40), "CONGRATS! You leveled-up to level "+myStateMachine.getAtributeValue("level")+"!!");
			
			if (GUI.Button(new Rect(Screen.width/2-75, 180, 150, 50), "Continue"))
			{
				
				eventToFSM = "done";
			}
			
		}
	}



public override void tickFired(){

}

//.......................................................................
//This state happens when starting
private void Entered_State_start ()
  {
 Debug.Log("start"); 
 eventToFSM = "start"; 
   //state handling code goes here

  }
//.......................................................................

	//.......................................................................
	//This state happens when starting
	private void Left_State_start ()
	{
		Debug.Log("start left"); 

		
	}
	//.......................................................................
	

//.......................................................................
//Showing the Menu
	private void Entered_State_menu ()
  {
 Debug.Log("menu");  
   //state handling code goes here

  }
//.......................................................................
	//.......................................................................
	//Showing the Menu
	private void Left_State_menu ()
	{
		Debug.Log("left menu");  
		//state handling code goes here
		
	}
	//.......................................................................
	



//.......................................................................
//playing the game
	private void Entered_State_playing ()
  {
 Debug.Log("playing");  
   //state handling code goes here

  }
//.......................................................................

	//.......................................................................
	//playing the game
	private void Left_State_playing ()
	{
		Debug.Log("left playing");  
		//state handling code goes here
		
	}
	//.......................................................................
	


//.......................................................................
//player paused game
	private void Entered_State_paused ()
  {
 Debug.Log("paused");  
   //state handling code goes here

  }
//.......................................................................
	//.......................................................................
	//player paused game
	private void Left_State_paused ()
	{
		Debug.Log("left paused");  
		//state handling code goes here
		
	}
	//.......................................................................
	


//.......................................................................
//Player won
	private void Entered_State_won ()
  {
 Debug.Log("won");  
   //state handling code goes here

  }
//.......................................................................

	//.......................................................................
	//Player won
	private void Left_State_won ()
	{
		Debug.Log("left won");  
		//state handling code goes here
		
	}
	//.......................................................................
	

//.......................................................................
//player lost
	private void Entered_State_lost ()
  {
 Debug.Log("lost");  
   //state handling code goes here

  }
//.......................................................................

	//.......................................................................
	//player lost
	private void Left_State_lost ()
	{
		Debug.Log("left lost");  
		//state handling code goes here
		
	}
	//.......................................................................
	

//.......................................................................
//The game is over
	private void Entered_State_gameOver ()
  {
 Debug.Log("gameOver");  
   //state handling code goes here

  }
//.......................................................................

	//.......................................................................
	//The game is over
	private void Left_State_gameOver ()
	{
		Debug.Log("left gameOver");  
		//state handling code goes here
		
	}
	//.......................................................................
	

	//.......................................................................
	//Player leveled up
	private void Entered_State_levelUp ()
	{
		Debug.Log("levelUp");  
		//state handling code goes here
		
	}
	//.......................................................................
	
	//.......................................................................
	//Player leveled up
	private void Left_State_levelUp ()
	{
		Debug.Log("left levelUp");  
		//state handling code goes here
		
	}
	//.......................................................................
	


  }

