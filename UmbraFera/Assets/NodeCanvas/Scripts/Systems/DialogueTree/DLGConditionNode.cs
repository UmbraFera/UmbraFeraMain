#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;

namespace NodeCanvas.DialogueTree{

	[AddComponentMenu("")]
	[ScriptName("Condition")]
	public class DLGConditionNode : DLGNodeBase{

		[SerializeField]
		private ConditionTask _condition;

		public ConditionTask condition{
			get{return _condition;}
			set
			{
				_condition = value;
				if (_condition != null)
					_condition.SetOwnerDefaults(this);
			}
		}

		public override string nodeName{
			get{return base.nodeName + " " + finalActorName;}
		}

		public override string nodeDescription{
			get{return "This node will execute the first child node if the Condition is true, or the second one if the Condition is false. The Actor selected is used for the Condition check";}
		}

		public override int maxOutConnections{
			get {return 2;}
		}

		public override System.Type outConnectionType{
			get{return typeof(ConnectionBase);}
		}

		protected override NodeStates OnExecute(){

			if (outConnections.Count == 0){
				DLGTree.StopGraph();
				return Error("There are no connections.", gameObject);
			}

			if (!finalActor){
				DLGTree.StopGraph();
				return NodeStates.Error;
			}

			if (!condition){
				Debug.LogWarning("No Condition on Dialoge Condition Node ID " + ID);
				outConnections[0].Execute(finalActor, finalBlackboard);
				return NodeStates.Success;
			}

			if (condition.CheckCondition(finalActor, finalBlackboard)){
				outConnections[0].Execute(finalActor, finalBlackboard);
				return NodeStates.Success;
			}

			if (outConnections.Count == 2){
				outConnections[1].Execute(finalActor, finalBlackboard);
				return NodeStates.Success;
			}

			graph.StopGraph();
			return NodeStates.Success;
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		protected override void OnNodeGUI(){

			base.OnNodeGUI();

			if (condition == null){
				GUILayout.Label("No Condition");
				return;
			}

			if (outConnections.Count == 0){
				GUILayout.Label("Connect Outcomes");
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