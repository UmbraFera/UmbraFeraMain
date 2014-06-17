using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

//Conditions extend ConditionTask. Here are the important properties inherited
//
//agent Component 			is the component passed when this condition gets Executed, re-setted to the type specified (see bellow)
//blackboard Blackboard		is the blackboard passed if any when the condition gets Executed. If none, the Blackboard compoenent will be taken from the agent

//insteand of puting task into a namespace you can simply 'using NodeCanvas' at the top
namespace NodeCanvas.Conditions{

	[ScriptName("Empty Condition")]
	[ScriptCategory("My Conditions")]
	public class EmptyCondition : ConditionTask{

		//You can Optionaly use these attributes on a field...
		//[RequiredField]:	Marks the field as required and thus the condition will throw an error prior to executing if it's null or empty
		//[GetFromAgent]:	Use this on a Component field type and the Component will be taken from the agent. This also makes the field Required as above

		//this is called once a new agent is set. You may use this for one time operations that dont need to called on every execution
		//Think of it like 'Awake'. If you return a string then an error will be loged and the init will fail. Retrun null if everything is fine.
		protected override string OnInit(){
			return null;
		}

		//called when CheckCondition. Use to return the outcome
		protected override bool OnCheck(){

			return true;
		}
	}
}