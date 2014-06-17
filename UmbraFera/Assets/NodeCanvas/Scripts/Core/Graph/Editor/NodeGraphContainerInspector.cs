using UnityEngine;
using System.Collections;
using UnityEditor;
using NodeCanvas;

namespace NodeCanvasEditor{

    [CustomEditor(typeof(NodeGraphContainer))]
    public class NodeGraphContainerInspector : Editor {

        private NodeGraphContainer graph{
            get {return target as NodeGraphContainer;}
        }


        void OnEnable(){

            graph.nodesRoot.gameObject.hideFlags = NodeGraphContainer.doHide? HideFlags.HideInHierarchy : 0;
        }

        public override void OnInspectorGUI(){

            if (IsPrefab())
                return;
            
            ShowBasicGUI();
            ShowTargetsGUI();
        }

    	//hack
        private void OnDestroy(){
            if (graph == null){
                Transform root= graph._nodesRoot;
                if (root != null)
                    DestroyImmediate(root.gameObject);
            }
        }

        //for use in derived inspectors
    	private bool IsPrefab(){

            bool isPrefab= (PrefabUtility.GetPrefabType(graph) == PrefabType.Prefab);
            if (isPrefab)
                EditorGUILayout.HelpBox("Editing is not allowed when prefab asset is selected. Please place the prefab in a scene, edit and apply it", MessageType.Warning);
            return isPrefab;
        }


        private void ShowBasicGUI(){

           if (graph.isRunning)
                EditorUtils.CoolLabel("Now Running!");

            GUILayout.Space(10);
            graph.graphName = EditorGUILayout.TextField("Graph Name", graph.graphName);
            graph.graphComments = GUILayout.TextArea(graph.graphComments, GUILayout.Height(50));

            GUI.backgroundColor = new Color(0.8f,0.8f,1);
            if (GUILayout.Button("-> OPEN IN NODECANVAS <-"))
                NodeGraphEditor.OpenWindow(graph);
            GUI.backgroundColor = Color.white;

            EditorUtils.BoldSeparator();

            Repaint();
        }

        private void ShowTargetsGUI(){

            GUI.color = new Color(1f,1f,1f,0.5f);
            GUILayout.Label("Current Owner References:");

            graph.agent = EditorGUILayout.ObjectField("Agent", graph.agent, typeof(Component), true) as Component;
            graph.blackboard = EditorGUILayout.ObjectField("Blackboard", graph.blackboard, typeof(Blackboard), true) as Blackboard;

            GUI.color = Color.white;
        }
    }
}