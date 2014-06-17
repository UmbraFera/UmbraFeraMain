using UnityEngine;
using System.Collections;

namespace NodeCanvas.BehaviourTree{

	[AddComponentMenu("")]
	[ScriptName("Accessor")]
	[ScriptCategory("Decorators")]
	public class AccessorDecorator : BTDecoratorNode {

		[SerializeField]
		private ConditionTask _condition;
		private bool accessed;

		public override string nodeName{
			get {return "Accessor";}
		}

		public override string nodeDescription{
			get {return "Accessor will allow access and thus execute it's child node if the Condition Task assigned is true and will return whatever the child node returns. Accessor will return Failure if the Condition is false and the child node is not already Running. It is kind of the opposite to the Interruptor.";}
		}

		public ConditionTask condition{
			get {return _condition;}
			set
			{
				_condition = value;
				if (_condition != null)
					_condition.SetOwnerDefaults(graph);
			}
		}

		protected override NodeStates OnExecute(Component agent, Blackboard blackboard){

			if (!decoratedConnection)
				return NodeStates.Resting;

			if (!condition)
				return NodeStates.Failure;

			if (condition.CheckCondition(agent, blackboard)){
				accessed = true;
				if (decoratedConnection.connectionState != NodeStates.Running)
					decoratedConnection.ResetConnection();
			}

			return accessed? decoratedConnection.Execute(agent, blackboard) : NodeStates.Failure;
		}

		protected override void OnReset(){
			accessed = false;
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){

			if (condition != null){
				GUILayout.Label(condition.taskInfo);
				return;
			}

			GUILayout.Label("No Condition");
		}

		protected override void OnNodeInspectorGUI(){

			base.OnNodeInspectorGUI();

			if (condition == null){
				EditorUtils.ShowComponentSelectionButton(gameObject, typeof(ConditionTask), delegate(Component c){condition = (ConditionTask)c;});
				return;
			}

			if (EditorUtils.TaskTitlebar(condition))
			condition.ShowTaskEditGUI();
		}
		
		#endif
	}
}