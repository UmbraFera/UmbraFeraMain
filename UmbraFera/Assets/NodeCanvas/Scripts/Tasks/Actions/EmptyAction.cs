using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

//Actions extend ActionTask. Here are the important properties inherited
//
//Component agent			is the component passed when this action gets Executed, re-setted to the type specified (see bellow)
//Blackboard blackboard		is the blackboard passed if any when the action gets Executed. If none, the Blackboard compoenent will be taken from the agent
//float elapsedTime			is the time in seconds the action is running starting from when Executed

//insteand of puting task into a namespace you can simply 'using NodeCanvas' at the top
namespace NodeCanvas.Actions{

	[ScriptName("Empty Action")]
	[ScriptCategory("My Actions")]
	public class EmptyAction : ActionTask{

		//You can Optionaly use these attributes on a field...
		//[RequiredField]:	Marks the field as required and thus the action will throw an error prior to executing if it's null or empty
		//[GetFromAgent]:	Use this on a Component field type and the Component will be taken from the agent. This also makes the field Required as above

		//this is called once a new agent is set. You may use this for one time operations that dont need to called on every execution
		//Think of it like 'Awake'. If you return a string then an error will be loged and the init will fail. Retrun null if everything is fine.
		protected override string OnInit(){
			return null;
		}

		//This is called once the action executes
		protected override void OnExecute(){

			//Call EndAction or else the action will indefinitely run
			EndAction(true);
		}

		//This is called per frame while the action is running. If EndAction is called as above this will never be called
		protected override void OnUpdate(){

			//for example...
			/*
			if (elapsedTime > 2)
				EndAction(true);
			*/
		}

		//When the action stops for any reason, this is called. You may use to reset some values
		protected override void OnStop(){

		}
	}
}