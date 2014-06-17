using UnityEngine;

namespace NodeCanvas{

	///An interface used to provide default agent and blackboard references to tasks
	public interface ITaskDefaults{

		Component agent{ get; }
		Blackboard blackboard{ get; }

		void SendDefaults();
		void SendEvent(string eventName);
	}
}