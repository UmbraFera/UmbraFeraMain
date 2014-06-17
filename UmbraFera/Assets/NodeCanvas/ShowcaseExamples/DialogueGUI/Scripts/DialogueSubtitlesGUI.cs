using UnityEngine;
using System.Collections;
using NodeCanvas;
using NodeCanvas.DialogueTree;

//Sample UI for subtitles.
//1. You must subscribe to the event OnActorSpeaking
//2. When the event is dispatched a method with the same name as the event is called with a DialogueSpeechInfo object
//3. Display the text, play audio if you want and when finish call DoneSpeaking() at the DialogueSpeechInfo
public class DialogueSubtitlesGUI : MonoBehaviour{

	public GUISkin skin;
	public bool showOverActor;

	private DialogueActor talkingActor;
	private string displayText;
	private bool doShowSubs;

	//Subscribe to the event
	void OnEnable(){
		EventHandler.Subscribe(this, DLGEvents.OnActorSpeaking);
	}

	void OnDisable(){
		EventHandler.Unsubscribe(this);
	}


	//Here is what you could do for example as a debug alternative
	//...
	// function OnActorSpeaking(speech:DialogueSpeechInfo){
	// 	Debug.Log(speech.actor.actorName + " says: " + speech.statement.text);
	// 	speech.DoneSpeaking();
	// }

	//Function with same name as the event is called when the event is dispatched by the Dialogue Tree
	void OnActorSpeaking(DialogueSpeechInfo speech){
		
		talkingActor = speech.actor;
		StartCoroutine(Process(speech.statement, speech.DoneSpeaking));
	}

	//The coroutine writes text to the displayText variable overtime which is show later OnGUI
	IEnumerator Process(Statement statement, System.Action Done){

		doShowSubs = true;
		if (statement.audio){

			AudioSource audioSource= talkingActor.gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
			audioSource.PlayOneShot(statement.audio);

			float timer = 0;
			displayText = statement.text;
			while(timer < statement.audio.length){
				timer += Time.deltaTime;
				yield return 0;
			}

			DestroyImmediate(audioSource);

		} else {

			for (int i= 0; i < statement.text.Length; i++){
				displayText += statement.text[i];
				yield return new WaitForSeconds(0.05f);
				char c = statement.text[i];
				if (c == '.' || c == '!' || c == '?')
					yield return new WaitForSeconds(0.5f);
				if (c == ',')
					yield return new WaitForSeconds(0.1f);
			}

			yield return new WaitForSeconds(1.2f);
		}

		displayText = null;
		doShowSubs = false;
		Done();
	}


	void OnGUI(){

		if (!doShowSubs || !Camera.main)
			return;

		GUI.skin = skin;

		//calculate the size needed
		Vector2 finalSize= new GUIStyle("box").CalcSize(new GUIContent(displayText));
		Rect speechRect= new Rect(0,0,0,0);
		speechRect.width = finalSize.x;
		speechRect.height = finalSize.y;

		Vector3 talkPos= Camera.main.WorldToScreenPoint(talkingActor.dialoguePosition);
		talkPos.y = Screen.height - talkPos.y;

		//if show over actor and the actor's dialoguePosition is in screen, show the tet above the actor at that dialoguePosition
		if (showOverActor && Camera.main.rect.Contains( new Vector2(talkPos.x/Screen.width, talkPos.y/Screen.height) )){

			Vector2 newCenter = speechRect.center;
			newCenter.x = talkPos.x;
			newCenter.y = talkPos.y - speechRect.height/2;
			speechRect.center = newCenter;

		//else just show the subtitles at the bottom along with his portrait if any
		} else {

			speechRect = new Rect(10, Screen.height - 60, Screen.width - 20, 50);
			Rect nameRect = new Rect(0, 0, 200, 28);
			Vector2 newCenter = nameRect.center;
			newCenter.x = speechRect.center.x;
			newCenter.y = speechRect.y - 24;
			nameRect.center = newCenter;
			GUI.Box(nameRect, talkingActor.actorName);

			if (talkingActor.portrait){
				Rect portraitRect= new Rect(10, Screen.height - talkingActor.portrait.height - 70, talkingActor.portrait.width, talkingActor.portrait.height);
				GUI.DrawTexture(portraitRect, talkingActor.portrait);
			}
		}

		GUI.Box(speechRect, displayText);
	}
}
