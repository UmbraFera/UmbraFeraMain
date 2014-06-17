using UnityEngine;
using System.Collections;
using NodeCanvas.DialogueTree;

[RequireComponent(typeof(BoxCollider))]
public class ClickToStartDialogue : MonoBehaviour {

	public DialogueTreeContainer dialogueTree;

	void OnMouseDown(){

		if (dialogueTree != null){
			dialogueTree.StartGraph(OnGraphFinished);
			gameObject.SetActive(false);
		}
	}

	void OnGraphFinished(){

		gameObject.SetActive(true);
	}
}
