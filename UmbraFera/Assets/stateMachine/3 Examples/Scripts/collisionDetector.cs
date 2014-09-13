using UnityEngine;
using System.Collections;

public class collisionDetector : MonoBehaviour {

public GameObject target;
	
	void OnTriggerEnter(Collider col)
	{
		target.SendMessage("triggerEnter",col.gameObject);
	}
}
