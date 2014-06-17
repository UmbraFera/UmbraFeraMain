using UnityEngine;
using System.Collections;
using System.Linq;

namespace NodeCanvas.BehaviourTree{

	[AddComponentMenu("")]
	///The base class for all Behaviour Tree system nodes
	abstract public class BTNodeBase : NodeBase{

		public override string nodeDescription{
			get {return "This is the BTNodeBase Class or the node doesn't override 'nodeDescription' get property";}
		}

		public override System.Type outConnectionType{
			get{return typeof(ConditionalConnection);}
		}

		public override int maxInConnections{
			get{return 1;}
		}

		public override int maxOutConnections{
			get {return -1;}
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		protected override void OnNodeReleased(){
			SortConnectionsByPositionX();
		}

		protected override void OnNodeInspectorGUI(){

			nodeComment = UnityEditor.EditorGUILayout.TextField("Comments", nodeComment);
			EditorUtils.Separator();
		}

		protected override void OnContextMenu(UnityEditor.GenericMenu menu){

			menu.AddItem (new GUIContent ("Make Nested"), false, ContextMakeNested);
		}

		//TODO possibly move making nested into NodeBase so that it's reusable in other graph systems as well
		private void ContextMakeNested(){

			if (!UnityEditor.EditorUtility.DisplayDialog("Make Nested", "This will create a new Nested BT and place this node and child nodes inside. This can't be Undone! Are you sure?", "DO IT", "NO!"))
				return;

			BTNestedTreeNode newNestedNode = graph.AddNewNode(typeof(BTNestedTreeNode)) as BTNestedTreeNode;
			BTContainer newBT = newNestedNode.CreateNewNestedTree();
			newNestedNode.nodeRect.center = this.nodeRect.center;

			if (this.graph.primeNode == this)
				this.graph.primeNode = newNestedNode;

			//Relink connections to the new nested tree node
			foreach (ConnectionBase connection in inConnections.ToArray())
				connection.Relink(newNestedNode);

			//Copy the nodes over to the new graph. TODO: Use IReferencable interface instead of check type
			foreach (NodeBase node in FetchAllChildNodes(true)){
				
				if (this.graph.primeNode == node)
					this.graph.primeNode = newNestedNode;

				if (node.GetType() == typeof(BTActionNode))
					(node as BTActionNode).BreakReference();

				if (node.GetType() == typeof(BTConditionNode))
					(node as BTConditionNode).BreakReference();

				node.MoveToGraph(newBT);
			}

			//TODO: Use IReferencable interface instead of check type
			foreach (NodeBase node in newNestedNode.graph.allNodes){

				if (node.GetType() == typeof(BTActionNode)){
					if ( this.graph.allNodes.Contains( (node as BTActionNode).referencedNode ) )
						(node as BTActionNode).BreakReference();
				}

				if (node.GetType() == typeof(BTConditionNode)){
					if ( this.graph.allNodes.Contains( (node as BTConditionNode).referencedNode ) )
						(node as BTConditionNode).BreakReference();
				}
			}

			newBT.primeNode = this;
			this.inConnections.Clear();
		}

		#endif
	}
}