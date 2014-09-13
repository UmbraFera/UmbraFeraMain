using FSM_NS;
using UnityEngine;
using System.Collections;

public class carController: stateController ,stateControllerInterface 
{

//public lightController theStopLight;

	float speed = 0;
	private Vector3 moveVector=new Vector3();
	private float maxSpeed=5;
	
	private int carLayerMask = 1 << 9;

void Start ()
  {
    string pushString = FSM_NS.HelperFile.getTextFileFromResource("carFSM");
    myStateMachine = new FSM_NS.FSM(pushString,this);
    myStateMachine.jumpToState(startStateID);
	moveVector    =Vector3.forward*speed;
  }

void Update () 
  {
//Your code here
   
  // transform.Translate(moveVector);
		
   superUpdate();

  }
  void FixedUpdate()
  {
		transform.Translate(moveVector);
  }

 float getFrontDist()
	{
		Vector3 fwd = transform.TransformDirection(Vector3.forward);
		RaycastHit hit;
		
		if (Physics.Raycast(transform.position, fwd, out hit,carLayerMask))
		//if (Physics.Raycast(transform.position, fwd, out hit, 1000))
		{
			if(hit.collider.gameObject.tag=="car")
			{
				myStateMachine.setAttribute("distanceToCar",hit.distance);
				
				return hit.distance;
			;
			}
		}
		myStateMachine.setAttribute("distanceToCar",999);
		return 999;	
	}

public override void tickFired(){
		
		
		refresh();
		
		
}

public void refresh()
{
		float d=getFrontDist();
		//print(gameObject.name+ " front of the object is a "+"  dist="+d);
		speed = float.Parse( myStateMachine.getAtributeValue("speed"));
		
		moveVector    =Vector3.forward*(speed*.1f);
}

//.......................................................................
//This state happens when reaching max speed
	private void Entered_State_speedingup ()
  {
    Debug.Log("speedingup");  
   //state handling code goes here

  }
//.......................................................................


//.......................................................................
//This state happens when reaching max speed
private void Left_State_speedingup ()
	{
		Debug.Log("speedingup");  
		//state handling code goes here
		
	}
	//.......................................................................
	


//.......................................................................
//This state happens when sees redlight
private void Entered_State_stopping ()
  {
 Debug.Log("stopping");  
		   //state handling code goes here

  }
private void Left_State_stopping ()
	{
	}
//.......................................................................



//.......................................................................
//This state happens when at target speed
private void Entered_State_maintaining ()
  {
 Debug.Log("maintaining"); 
		   //state handling code goes here

  }
private void Left_State_maintaining ()
	{
	}
//.......................................................................



//.......................................................................
//This state happens when car stopped
	private void Entered_State_stopped ()
  {
 Debug.Log("stopped"); 
		   //state handling code goes here

  }
	private void Left_State_stopped ()
	{
	}
//.......................................................................

	//.......................................................................
	//This state happens when car slowingDown
	private void Entered_State_slowingDown ()
	{
		Debug.Log("slowingDown"); 
		//state handling code goes here
		
	}
	private void Left_State_slowingDown ()
	{
	}
	//.......................................................................
	
	



  }

