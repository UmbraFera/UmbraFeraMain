using UnityEngine;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptCategory("GameObject")]
	[AgentType(typeof(Transform))]
	public class GetGameObjectPosition : ActionTask {

		public BBVector saveAs = new BBVector{blackboardOnly = true};

		protected override string actionInfo{
			get {return "Get " + agentInfo + " position as " + saveAs;}
		}

		protected override void OnExecute(){

			saveAs.value = agent.transform.position;
			EndAction();
		}
	}
}