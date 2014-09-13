using UnityEngine;
using System.Collections;

public class clickHandler : MonoBehaviour {

    public GameObject controller;
    public string eventToCall="clickCube";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	  
	}
	
	
	void OnMouseUp()
	{
		controller.SendMessage("recieveEvent",eventToCall,SendMessageOptions.DontRequireReceiver);
	}
	
}
