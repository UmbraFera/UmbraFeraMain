using UnityEngine;

namespace NodeCanvas.Actions{

	[ScriptCategory("GameObject")]
	[AgentType(typeof(Transform))]
	public class DestroyGameObject : ActionTask {

		protected override string actionInfo{
			get {return "Destroy GameObject";}
		}

		protected override void OnExecute(){

			Destroy(agent.gameObject);
			EndAction();
		}
	}
}