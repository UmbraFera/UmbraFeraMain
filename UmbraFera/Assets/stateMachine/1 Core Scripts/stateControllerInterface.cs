using UnityEngine;
using System.Collections;


public interface stateControllerInterface 
{
    void enteredState (string stateName);
	 void leftState    (string stateName);
	 void tickFired();
   
}
