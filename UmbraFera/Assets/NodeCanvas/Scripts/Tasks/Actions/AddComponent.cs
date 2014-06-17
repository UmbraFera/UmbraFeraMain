using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptCategory("GameObject")]
	[AgentType(typeof(Transform))]
	public class AddComponent : ActionTask {

		[RequiredField]
		public string componentName;
		public BBComponent saveAs = new BBComponent{blackboardOnly = true};

		protected override string actionInfo{
			get {return "Add '" + componentName + "' Component";}
		}

		protected override void OnExecute(){

			if (agent.GetComponent(componentName) == null)
				saveAs.value = agent.gameObject.AddComponent(componentName);

			EndAction();
		}
	}
}