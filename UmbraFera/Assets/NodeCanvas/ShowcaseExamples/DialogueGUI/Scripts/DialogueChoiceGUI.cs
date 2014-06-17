using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas;
using NodeCanvas.DialogueTree;

//This is an example of making use for the Choices of the Dialogue tree
public class DialogueChoiceGUI : MonoBehaviour{

	public GUISkin skin;
	private DialogueOptionsInfo currentOptions;
	private float timer;
	private bool doShowChoices;

	//We need to Subscribe to the event needed
	void OnEnable(){
		EventHandler.Subscribe(this, DLGEvents.OnDialogueOptions);
	}

	void OnDisable(){
		EventHandler.Unsubscribe(this);
	}

	//A function with the same name as the subscribed Event is called when the event is dispatched
	void OnDialogueOptions(DialogueOptionsInfo optionsInfo){
		
		timer = optionsInfo.availableTime;
		currentOptions = optionsInfo;
		doShowChoices = true;
		if (timer > 0)
			StartCoroutine("GUICountDown");
	}

	void OnGUI(){

		if (!doShowChoices || !Camera.main)
			return;

		GUI.skin = skin;

		//Calculate the y size needed
		float neededHeight= timer > 0? 20 : 0;
		foreach (KeyValuePair <Statement, int> pair in currentOptions.finalOptions)
			neededHeight += new GUIStyle("box").CalcSize(new GUIContent(pair.Key.text)).y;

		//show the choices which are within a Dictionary of Statement and the int whic is the Index we need to 
		//callback when an option is selected
		Rect optionsRect= new Rect(10, Screen.height - neededHeight - 10, Screen.width - 20, neededHeight);
		GUILayout.BeginArea(optionsRect);
		foreach (KeyValuePair<Statement, int> option in currentOptions.finalOptions){

			//When a choice is selected we need to Callback with the index of the statement choice selected
			if (GUILayout.Button(option.Key.text, new GUIStyle("box"), GUILayout.ExpandWidth(true))){
				StopCoroutine("GUICountDown");
				doShowChoices = false;
				currentOptions.SelectOption(option.Value);
			}
		}

		//show the countdown UI
		if (timer > 0){
			float colorB = GUI.color.b;
			float colorG = GUI.color.g;
			colorB = timer / currentOptions.availableTime * 0.5f;
			colorG = timer / currentOptions.availableTime * 0.5f;
			GUI.color = new Color(1f, colorG, colorB);
			GUILayout.Box("...", GUILayout.Height(5), GUILayout.Width(timer / currentOptions.availableTime * optionsRect.width));
		}

		GUILayout.EndArea();
	}

	//not private for never used warning message since this is started with string
	//Countdown for the available time. Picking a choice is done by the graph when it ends. All we need to do is to stop
	//showing the UI
	IEnumerator GUICountDown(){

		while (timer > 0){
			timer -= Time.deltaTime;
			yield return 0;
		}

		doShowChoices = false;
	}
}
