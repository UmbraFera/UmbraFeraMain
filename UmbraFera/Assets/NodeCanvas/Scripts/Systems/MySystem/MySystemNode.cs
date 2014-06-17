#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;

//All nodes must somehow derive NodeBase. Here are some important inherited properties:
//inConnections 		a list on incomming connections to this node
//outConnections 		a list of outgoing connections from this node
//nodeGraph 			the graph this node belongs to
//graphAgent 			the agent of the graph this node belongs to
//graphBlackboard 		the blackboard of the graph this node belongs to

namespace NodeCanvas.MySystem{

	//Use this attribute for friendly names showing on top of the NodeCanvas window
	[ScriptName("My Node")]
	public class MySystemNode : NodeBase{

		//the title name of the node
		public override string nodeName{
			get {return "My Node";}
		}

		//The description displayed on the bottom of the NodeCanvas window
		public override string nodeDescription{
			get {return "This is my node's description";}
		}

		//The max incomming connections for the node. -1 for infinite.
		public override int maxInConnections{
			get {return -1;}
		}

		//The max outgoing connections for the node. -1 for infinite
		public override int maxOutConnections{
			get {return -1;}
		}

		//The out connection type for the node. Currently either ConnectionBase or ConditionalConnection,
		//or you may create your own connection types
		public override System.Type outConnectionType{
			get {return typeof(ConditionalConnection);}
		}

		//What this node does on execute. It should return a NodeState. Catching agent and blackboard is optional
		//When a node executes clearly depends on the system
		protected override NodeStates OnExecute(Component agent, Blackboard blackboard){

			//...
			return NodeStates.Success;
		}

		//When the graph Starts or Stops it's prime node is recusrsively reset, but it can also reset at other times as well depending on the system
		protected override void OnReset(){

			//...
		}

		//Whenever another node is connected to this node, OnPortConnected is called along with it's index in outConnections list
		public override void OnPortConnected(int portIndex){

		}

		//Whenever another node is disconnected from this node, OnPortDisconnected is called along with it's index in the outConnections list
		public override void OnPortDisconnected(int portIndex){

		}


		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		//Use this to display custom info or controls within the node
		protected override void OnNodeGUI(){

			GUILayout.Label("Some node editor control");
		}

		//Use this to display controls when the node is selected within the editor window
		protected override void OnNodeInspectorGUI(){

		}

		//You may use this to add more context actions to the generic menu for when right clicking on a node
		protected override void OnContextMenu(GenericMenu menu){

			//example...
			//menu.AddItem (new GUIContent ("Some Context Action"), false, delegate(){ });
		}
		
		#endif
	}
}