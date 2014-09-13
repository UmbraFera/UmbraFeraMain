using UnityEngine;
using System.Collections;

public class timeFire : MonoBehaviour 
{

	
	
	private float timeLeft  = 0;
	public float totalTimeInSeconds  = 10;
	private bool on=true;
	public string methodToFire = "timerFired";
	public GameObject target_;
	public bool repeating=false;
	
	public void turnOn()
	{
		timeLeft = totalTimeInSeconds;
		on = true;
	}
	public void turnOff()
	{
		on = false;
	}
	
	public void startNewRepeatingTimer(GameObject theTarget, string theTargetMethod, float length)
	{
	    repeating = true;
		//Debug.Log("1..");
		target_ = theTarget;
		methodToFire = theTargetMethod;
		totalTimeInSeconds = length;
		turnOn();
	}	
	
	public void startNewTimer(GameObject theTarget, string theTargetMethod, float length)
	{
//		Debug.Log("1..");
		target_ = theTarget;
		methodToFire = theTargetMethod;
		totalTimeInSeconds = length;
		turnOn();
	}
	
	
	
	// Use this for initialization
	void Start () {
       //target_ = new GameObject();
	}
	
	// Update is called once per frame
public void superUpdate () 
	{
		
		if (on) 
		{
			//Debug.Log(".."+timeLeft);
			timeLeft -= Time.deltaTime;
			
			if (timeLeft < 0) 
			{
				timeLeft = totalTimeInSeconds;	
				if(target_ != null)							
				target_.SendMessage(methodToFire,SendMessageOptions.DontRequireReceiver);
				
				if(repeating)turnOn();
				else
					{	
						turnOff();
						GameObject.DestroyImmediate(gameObject);
					}		
			}
			
		}
	}
}
