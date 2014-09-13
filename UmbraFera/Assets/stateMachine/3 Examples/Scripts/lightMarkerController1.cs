using UnityEngine;
using System.Collections;

public class lightMarkerController1 : MonoBehaviour {

public lightController myLightController;

	public void OnTriggerEnter(Collider coll)
	{
		if(coll.gameObject.tag=="car")
		{		
			Debug.Log("Collided with "+coll.gameObject.tag);
			myLightController.sensor1TrippedBy(coll.gameObject);
		}
	}
}
