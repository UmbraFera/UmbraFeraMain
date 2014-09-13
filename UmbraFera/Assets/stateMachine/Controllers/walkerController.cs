using FSM_NS;
using UnityEngine;
using System.Collections;

public class walkerController: stateController ,stateControllerInterface 
{
float speed = .08f;

private Animator ani;

void Start ()
  {
    ani=GetComponent<Animator>();
    string pushString = FSM_NS.HelperFile.getTextFileFromResource("walkerFSM");
    myStateMachine    = new FSM_NS.FSM(pushString,this);
	currentState      =	myStateMachine.state.name;
		moveVector    =Vector3.forward*speed;
	//new Vector3(gameObject.transform.position.x,gameObject.transform.position.y,gameObject.transform.position.z);
  }

private Vector3 moveVector=new Vector3();

private float maxSpeed=5;

void Update () 
  {
        
		
		transform.Translate(moveVector);
		superUpdate();
    
  }
//---------------------------------------

	public void incrementCount()
	{
	  int count = int.Parse( myStateMachine.getAtributeValue("count"));
	  count++;
	  myStateMachine.setAttribute("count",count);
	}
	
	public void triggerEnter(GameObject sensesedObject)
	{
		//Debug.Log("Sense "+sensesedObject.name +" type is "+sensesedObject.tag);
		if(sensesedObject.tag=="wall")eventToFSM = "wallAhead";
	}
//----------------------------------------	


private void randomAction()
{
   if(Random.Range(1,10)==1)
      eventToFSM = "turnRight";
   if(Random.Range(1,10)==2)
	  eventToFSM = "turnLeft";
      
}


	public override void tickFired()
	{
	   //Debug.Log("Time tick fired");
		incrementCount();
		randomAction();
	}

//.......................................................................
//This state happens when player is moving forward.
private void Entered_State_foward ()
  {
// Debug.Log("foward");  
		moveVector=Vector3.forward*speed;
   //state handling code goes here

  }
	private void Left_State_foward (){}
//.......................................................................



//.......................................................................
//This state happens when player is not moving.
private void Entered_State_idle ()
  {
        //Debug.Log("idle");  
		moveVector=new Vector3(0,0,0);
		ani.SetBool("moving",false);
   //state handling code goes here

  }
  
private void Left_State_idle ()
{
	ani.SetBool("moving",true);
}
//.......................................................................



//.......................................................................
//This state happens when player is stopped and looking around
private void Entered_State_lookingAround ()
  {
   // Debug.Log("lookingAround");  
   //state handling code goes here
		ani.SetBool("moving",false);
  }
  
private void Left_State_lookingAround ()
{	
	ani.SetBool("moving",true);
}
//.......................................................................



//.......................................................................
//This state happens when player rotates left
private void Entered_State_turnLeft ()
  {
 	//Debug.Log("turnLeft");  
   //state handling code goes here
	transform.Rotate(new Vector3(0,-20,0));
	eventToFSM = "done";
  }
	private void Left_State_turnLeft (){}
//.......................................................................



//.......................................................................
//This state happens when player turns around.
private void Entered_State_turningAround ()
  {
 	//Debug.Log("turningAround");
	transform.Translate(Vector3.back*.5f);  
	 transform.Rotate(new Vector3(0,Random.Range(40,140),0));
   //state handling code goes here
   eventToFSM = "done";

  }
	private void Left_State_turningAround (){}
//.......................................................................



//.......................................................................
//This state happens when player rotates right.
private void Entered_State_turnRight ()
  {
 //Debug.Log("turnRight");  
   //state handling code goes here
		transform.Rotate(new Vector3(0,20,0));
		eventToFSM = "done";

  }
	private void Left_State_turnRight (){}
//.......................................................................





  }

