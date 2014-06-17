using UnityEngine;
using UnityEditor;
using System.Collections;
using NodeCanvas;
using NodeCanvas.FSM;

namespace NodeCanvasEditor{

	[CustomEditor(typeof(FSMOwner))]
	public class FSMOwnerInspector : GraphOwnerInspector {

		FSMOwner owner{
			get {return target as FSMOwner; }
		}

		protected override void OnExtraOptions(){
			
			if (Application.isPlaying && owner.FSM != null){
			
				GUILayout.BeginVertical("box");
				
				GUILayout.Label("Debug Playback Controls");

				if ( (owner.FSM.isRunning || owner.FSM.isPaused) && GUILayout.Button("Stop FSM"))
					owner.StopGraph();

				if (!owner.FSM.isRunning && GUILayout.Button("Start/Resume FSM"))
					owner.StartGraph();

				if (owner.FSM.isRunning && GUILayout.Button("Pause FSM"))
					owner.FSM.PauseGraph();

				GUILayout.EndVertical();
			}
		}
	}
}