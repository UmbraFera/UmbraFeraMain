#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;

namespace NodeCanvas.BehaviourTree{

	[AddComponentMenu("")]
	[ScriptName("Selector")]
	[ScriptCategory("Composites")]
	public class BTSelectorNode : BTNodeBase{

		public bool isDynamic;
		public bool isRandom;

		private int lastRunningNodeIndex= 0;

		public override string nodeName{
			get {return "<color=#b3ff7f>SELECTOR</color>";}
		}

		public override string nodeDescription{
			get{return "The Selector Node executes it’s child nodes either in priority order or randomly, until one of it’s children returns Success at which point the Selector will also return Success. If none does, the Selector will return Failure. If a Selector is ‘Dynamic’, it will keep evaluating in order even if a child node is Running. So if a higher priority node returns Success, the Selector will interupt the currenly Running child node and return Success as well.";}
		}

		protected override NodeStates OnExecute(Component agent, Blackboard blackboard){

			if (outConnections.Count-1 < lastRunningNodeIndex && lastRunningNodeIndex != 0)
				lastRunningNodeIndex--;

			for ( int i= isDynamic? 0 : lastRunningNodeIndex; i < outConnections.Count; i++){

				nodeState = outConnections[i].Execute(agent, blackboard);
				
				if (nodeState == NodeStates.Running){

					if (isDynamic && i < lastRunningNodeIndex)
						outConnections[lastRunningNodeIndex].ResetConnection();

					lastRunningNodeIndex = i;
					return nodeState;
				}

				if (nodeState == NodeStates.Success){
					
					if (isDynamic && i < lastRunningNodeIndex)
						outConnections[lastRunningNodeIndex].ResetConnection();

					return nodeState;
				}
			}

			return NodeStates.Failure;
		}

		protected override void OnReset(){

			lastRunningNodeIndex = 0;
			if (isRandom)
				outConnections = Shuffle(outConnections);
		}

		//Fisher-Yates shuffle algorithm
		private List<ConnectionBase> Shuffle(List<ConnectionBase> list){
			for ( int i= list.Count -1; i > 0; i--){
				int j = (int)Mathf.Floor(Random.value * (i + 1));
				ConnectionBase temp = list[i];
				list[i] = list[j];
				list[j] = temp;
			}

			return list;
		}

		
		/////////////////////////////////////////
		/////////GUI AND EDITOR STUFF////////////
		/////////////////////////////////////////
		#if UNITY_EDITOR

		protected override void OnNodeGUI(){

			if (isDynamic)
				GUILayout.Label("Dynamic");
			if (isRandom)
				GUILayout.Label("Random");

			if (!isDynamic && !isRandom)
				GUILayout.Label("", GUILayout.Height(1));
		}

		protected override void OnNodeInspectorGUI(){
			
			base.OnNodeInspectorGUI();
			isDynamic = EditorGUILayout.Toggle("Dynamic", isDynamic);
			isRandom = EditorGUILayout.Toggle("Random", isRandom);		
		}

		#endif
	}
}