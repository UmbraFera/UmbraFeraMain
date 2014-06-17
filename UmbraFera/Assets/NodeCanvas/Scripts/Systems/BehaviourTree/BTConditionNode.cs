using UnityEngine;
using System.Collections;

namespace NodeCanvas.BehaviourTree{

	[AddComponentMenu("")]
	[ScriptName("Condition")]
	public class BTConditionNode : BTNodeBase{

		[SerializeField]
		private ConditionTask _condition;

		[SerializeField]
		private BTConditionNode _referencedNode;

		public ConditionTask condition{
			get
			{
				if (referencedNode != null)
					return referencedNode.condition;
				return _condition;
			}
			set
			{
				_condition = value;
				if (_condition != null)
					_condition.SetOwnerDefaults(graph);
			}
		}

		public BTConditionNode referencedNode{
			get {return _referencedNode;}
			private set {_referencedNode = value;}
		}

		public override string nodeName{
			get{return "CONDITION";}
		}

		public override string nodeDescription{
			get{return "The Condition Node can be assigned a ConditionTask. It returns Success or Failure based on that Condition Task assigned.";}
		}

		public override int maxOutConnections{
			get{return 0;}
		}

		protected override NodeStates OnExecute(Component agent, Blackboard blackboard){

			if (condition)
				return condition.CheckCondition(agent, blackboard)? NodeStates.Success: NodeStates.Failure;

			return NodeStates.Success;
		}


		/////////////////////////////////////////
		/////////GUI AND EDITOR STUFF////////////
		/////////////////////////////////////////
		#if UNITY_EDITOR

		protected override void OnNodeGUI(){
			
	        Rect markRect = new Rect(nodeRect.width - 15, 5, 15, 15);
	        if (referencedNode != null)
	        	GUI.Label(markRect, "<b>R</b>");

			if (condition == null) GUILayout.Label("No Condition");
			else GUILayout.Label(condition.taskInfo);
		}

		protected override void OnNodeInspectorGUI(){

			base.OnNodeInspectorGUI();
			
			if (referencedNode != null){
				if (GUILayout.Button(">Referenced Node<"))
					NodeGraphContainer.currentSelection = referencedNode;

				if (GUILayout.Button("Break Reference"))
					BreakReference();

				if (condition != null){
					GUILayout.Label("<b>" + condition.taskName + "</b>");
					condition.ShowTaskEditGUI();
				}
				return;
			}

			if (!condition){
				EditorUtils.ShowComponentSelectionButton(gameObject, typeof(ConditionTask), delegate(Component c){condition = (ConditionTask)c;});
				return;
			}

			if (EditorUtils.TaskTitlebar(condition))
				condition.ShowTaskEditGUI();
		}

		protected override void OnContextMenu(UnityEditor.GenericMenu menu){
			menu.AddItem (new GUIContent ("Duplicate (Reference)"), false, DuplicateReference);
		}
		
		private void DuplicateReference(){
			var newNode = graph.AddNewNode(typeof(BTConditionNode)) as BTConditionNode;
			newNode.nodeRect.center = this.nodeRect.center + new Vector2(50, 50);
			newNode.referencedNode = referencedNode != null? referencedNode : this;
		}

		public void BreakReference(){

			if (referencedNode == null)
				return;

			if (referencedNode.condition != null)
				condition = (ConditionTask)referencedNode.condition.CopyTo(this.gameObject);

			referencedNode = null;
		}

		#endif
	}
}