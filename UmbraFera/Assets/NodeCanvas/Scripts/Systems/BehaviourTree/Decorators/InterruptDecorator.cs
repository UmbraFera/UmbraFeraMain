using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.BehaviourTree{

	[AddComponentMenu("")]
	[ScriptName("Interruptor")]
	[ScriptCategory("Decorators")]
	public class InterruptDecorator : BTDecoratorNode{

		[SerializeField]
		private ConditionTask _condition;
		public ConditionTask condition{
			get {return _condition;}
			set
			{
				_condition = value;
				if (_condition != null)
					_condition.SetOwnerDefaults(graph);
			}
		}

		public override string nodeName{
			get{return "Interruptor";}
		}

		public override string nodeDescription{
			get{return "Interruptor will execute it's child node if the condition assigned is false. If the condition is or becomes true, Interruptor will stop & reset the child node if running and return false. Otherwise it will return whatever the child returns.";}
		}


		protected override NodeStates OnExecute(Component agent, Blackboard blackboard){

			if (!decoratedConnection)
				return NodeStates.Resting;

			if (!condition || condition.CheckCondition(agent, blackboard) == false)
				return decoratedConnection.Execute(agent, blackboard);

			if (decoratedConnection.connectionState == NodeStates.Running)
				decoratedConnection.ResetConnection();
			
			return NodeStates.Failure;
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){

			if (condition == null){

				GUILayout.Label("No Condition");
				return;
			}

			GUILayout.Label(condition.taskInfo);
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