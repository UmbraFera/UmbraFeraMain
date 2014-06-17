using UnityEditor;
using UnityEngine;
using System.Collections;
using NodeCanvas;

namespace NodeCanvasEditor{

	[InitializeOnLoad]
	public class HierarchyIcons{
		
		static HierarchyIcons(){
			EditorApplication.hierarchyWindowItemOnGUI += ShowIcon;
		}

		static void ShowIcon(int ID, Rect r){
			r.x = r.xMax - 18;
			r.width = 18;
			var go = EditorUtility.InstanceIDToObject(ID) as GameObject;
			if (go.GetComponent<GraphOwner>() != null)
				GUI.Label(r, "♟");
			if (go.GetComponent<NodeGraphContainer>() != null)
				GUI.Label(r, "⑆");
		}
	}

	public class GraphOwnerInspector : Editor {

		GraphOwner owner{
			get{return target as GraphOwner;}
		}

		void OnDestroy(){
		
			if (owner == null){
				if (owner.graph != null && EditorUtility.DisplayDialog("Removing Owner...", "Do you also want to delete the Owner's assigned Graph?", "DO IT", "Keep it")){
					DestroyImmediate(owner.graph.gameObject);
				}
			}
		}

		public override void OnInspectorGUI(){

			var label = "Graph";
			if (owner.graphType == typeof(NodeCanvas.BehaviourTree.BTContainer))
				label = "Behaviour Tree";
			if (owner.graphType == typeof(NodeCanvas.FSM.FSMContainer))
				label = "FSM";

			if (owner.graph == null){
				
				EditorGUILayout.HelpBox(label + "Owner needs " + label + ". Assign or Create a new one", MessageType.Info);
				if (GUILayout.Button("CREATE NEW")){
				
					if (owner.graph == null){
						owner.graph = new GameObject(label + " Graph").AddComponent(owner.graphType) as NodeGraphContainer;
						owner.graph.transform.parent = owner.transform;
						owner.graph.transform.localPosition = Vector3.zero;
					}

					owner.graph.agent = owner;
				}

				owner.graph = (NodeGraphContainer)EditorGUILayout.ObjectField(label, owner.graph, owner.graphType, true);
				return;
			}

			GUILayout.Space(10);

			owner.graph.graphName = EditorGUILayout.TextField(label + " Name", owner.graph.graphName);
			owner.graph.graphComments = GUILayout.TextArea(owner.graph.graphComments, GUILayout.Height(50));

			GUI.backgroundColor = EditorUtils.lightBlue;
			if (GUILayout.Button("OPEN"))
				NodeGraphEditor.OpenWindow(owner.graph, owner, owner.blackboard);
		
			GUI.backgroundColor = Color.white;
			GUI.color = new Color(1, 1, 1, 0.5f);
			owner.graph = (NodeGraphContainer)EditorGUILayout.ObjectField("Current " + label, owner.graph, owner.graphType, true);
			GUI.color = Color.white;

			owner.blackboard = (Blackboard)EditorGUILayout.ObjectField("Blackboard", owner.blackboard, typeof(Blackboard), true);
			owner.executeOnStart = EditorGUILayout.Toggle("Execute On Start", owner.executeOnStart);

			OnExtraOptions();

			EditorUtils.EndOfInspector();

			if (GUI.changed)
				EditorUtility.SetDirty(owner);

		}

		virtual protected void OnExtraOptions(){
			
		}
	}
}