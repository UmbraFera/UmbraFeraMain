using UnityEngine;
using System.Collections;

namespace NodeCanvas.DialogueTree{

	///The various events that are send through the EventHandler and from the Dialogue Tree
	public enum DLGEvents{
		
		OnActorSpeaking,
		OnDialogueOptions,
		OnDialogueStarted,
		OnDialogueFinished
	}
}