#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;

namespace NodeCanvas.DialogueTree{

	[AddComponentMenu("")]
	///A Dialogue Tree container
	public class DialogueTreeContainer : NodeGraphContainer{

		[SerializeField]
		private List<string> _dialogueActorNames = new List<string>();
		public Dictionary<string, DialogueActor> actorReferences = new Dictionary<string, DialogueActor>();

		///The actor names that are inputed and available to act
		public List<string> dialogueActorNames{
			get {return _dialogueActorNames;}
		}

		public override System.Type baseNodeType{
			get {return typeof(DLGNodeBase);}
		}

		protected override bool allowNullAgent{
			get{return true;}
		}

		private void Reset(){
			graphName = "DialogueTree";
		}

		protected override void OnGraphStarted(){

			if (agent != null)
				agent = agent.gameObject.GetComponent<DialogueActor>();

			if (agent == null){
				Debug.Log("Dialogue Tree Started without a DialogueActor. A Default one has been created. If you are actualy using the 'Owner' default Actor, make sure to start the Dialogue Tree with an Actor.");
				DialogueActor newActor = gameObject.GetComponent<DialogueActor>();
				if (newActor == null)
					newActor = gameObject.AddComponent<DialogueActor>();
				newActor.actorName = "Default";
				newActor.blackboard = newActor.gameObject.GetComponent<Blackboard>();
				agent = newActor;
			}

			actorReferences.Clear();

			foreach (string actorName in dialogueActorNames)
				actorReferences[actorName] = DialogueActor.FindActorWithName(actorName);

			if (dialogueActorNames.Count != actorReferences.Keys.Count){
				Debug.LogError("Not all Dialogue Actors were found for the Dialogue '" + graphName + "'", gameObject);
				StopGraph();
				return;
			}

			//DLGNodes implement ITaskDefaults to provide defaults for the tasks they contain
			//This SendDefaults is send after the graph's SendDefaults so in essence it overrides it
			foreach (DLGNodeBase node in allNodes)
				node.SendDefaults();

			EventHandler.Dispatch(DLGEvents.OnDialogueStarted, this);
			primeNode.Execute();
		}

		protected override void OnGraphStoped(){

			Debug.Log("Dialogue Tree '" + graphName + "', has Finished", gameObject);
			EventHandler.Dispatch(DLGEvents.OnDialogueFinished, this);
			actorReferences.Clear();
		}

		protected override void OnGraphPaused(){
			Debug.LogWarning("Pausing Dialogue Trees is currently unsupported. Please do not do that");
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR
		
		[MenuItem("NC/Create Dialogue Tree")]
		public static void Create(){
			DialogueTreeContainer newDLG = new GameObject("DialogueTree").AddComponent(typeof(DialogueTreeContainer)) as DialogueTreeContainer;
			Selection.activeObject = newDLG;
		}
		
		#endif
	}
}