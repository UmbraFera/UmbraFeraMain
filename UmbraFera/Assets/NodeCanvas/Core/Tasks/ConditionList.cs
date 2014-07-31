#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;

namespace NodeCanvas{

	[ExecuteInEditMode]
	[Category("✫ Utility")]
	public class ConditionList : ConditionTask{

		public List<ConditionTask> conditions = new List<ConditionTask>();
		public bool allSuccessRequired = true;

		protected override string info{
			get
			{
				string finalText = conditions.Count != 0? "" : "No Conditions";
				if (conditions.Count > 1)
					finalText += "<b>(" + (allSuccessRequired? "ALL True" : "ANY True") + ")</b>\n";

				for (int i= 0; i < conditions.Count; i++){
					if (conditions[i].isActive)
						finalText += conditions[i].taskInfo + (i == conditions.Count -1? "" : "\n" );
				}
				return finalText;
			}
		}

		protected override bool OnCheck(){

			int succeedChecks = 0;

			foreach (ConditionTask condition in conditions){

				if (!condition.isActive){
					succeedChecks ++;
					continue;
				}

				if (condition.CheckCondition(agent, blackboard)){

					if (!allSuccessRequired)
						return true;

					succeedChecks ++;
				}
			}

			return succeedChecks == conditions.Count;
		}

		protected override void OnGizmos(){
			foreach (ConditionTask condition in conditions)
				condition.DrawGizmos();
		}

		protected override void OnGizmosSelected(){
			foreach (ConditionTask condition in conditions)
				condition.DrawGizmosSelected();
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		private ConditionTask currentViewCondition;

		protected override void OnEditorValidate(){
			for (int i = 0; i < conditions.Count; i++){
				if (conditions[i] == null)
					conditions.RemoveAt(i);
			}
		}

		private void OnDestroy(){
			foreach(ConditionTask condition in conditions){
				var c = condition;
				EditorApplication.delayCall += ()=> { if (c) DestroyImmediate(c, true); };
			}
		}

		public override Task CopyTo(GameObject go){

			if (this == null)
				return null;

			var newList = (ConditionList)go.AddComponent<ConditionList>();
			Undo.RegisterCreatedObjectUndo(newList, "Copy List");
			Undo.RecordObject(newList, "Copy List");
			UnityEditor.EditorUtility.CopySerialized(this, newList);
			newList.conditions.Clear();

			foreach (ConditionTask condition in conditions){
				var copiedCondition = condition.CopyTo(go);
				newList.AddCondition(copiedCondition as ConditionTask);
			}

			return newList;
		}

		override protected void OnTaskInspectorGUI(){

			ShowListGUI();
			ShowNestedConditionsGUI();

			if (GUI.changed && this != null)
	            EditorUtility.SetDirty(this);
		}

		public void ShowListGUI(){

			if (this == null)
				return;

			EditorUtils.TaskSelectionButton(gameObject, typeof(ConditionTask), delegate(Task c){ AddCondition((ConditionTask)c) ;});

			//Check for possibly removed conditions
			foreach (ConditionTask condition in conditions.ToArray()){
				if (condition == null)
					conditions.Remove(condition);
			}

			if (conditions.Count == 0){
				EditorGUILayout.HelpBox("No Conditions", MessageType.None);
				return;
			}
			
			EditorUtils.ReorderableList(conditions, delegate(int i){

				var condition = conditions[i];
				GUI.color = new Color(1, 1, 1, 0.25f);
				GUILayout.BeginHorizontal("box");
				GUI.color = condition.isActive? new Color(1,1,1,0.8f) : new Color(1,1,1,0.25f);

				Undo.RecordObject(condition, "Mute");
				condition.isActive = EditorGUILayout.Toggle(condition.isActive, GUILayout.Width(18));

				GUI.backgroundColor = condition == currentViewCondition? Color.grey : Color.white;
				if (GUILayout.Button(EditorUtils.viewIcon, GUILayout.Width(25), GUILayout.Height(18)))
					currentViewCondition = condition == currentViewCondition? null : condition;
				EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
				GUI.backgroundColor = Color.white;

				GUILayout.Label(condition.taskInfo);
				if (GUILayout.Button("X", GUILayout.MaxWidth(20))){
					Undo.RecordObject(this, "List Remove Task");
					conditions.Remove(condition);
					Undo.DestroyObjectImmediate(condition);
				}
				EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
				GUILayout.EndHorizontal();
				GUI.color = Color.white;
			});

			EditorUtils.Separator();

			if (conditions.Count > 1){
				GUI.backgroundColor = new Color(0.5f,0.5f,0.5f);
				if (GUILayout.Button(allSuccessRequired? "ALL True Required":"ANY True Suffice"))
					allSuccessRequired = !allSuccessRequired;
				GUI.backgroundColor = Color.white;
			}
		}


		public void ShowNestedConditionsGUI(){

			if (conditions.Count == 1)
				currentViewCondition = conditions[0];

			if (currentViewCondition){
				EditorUtils.BoldSeparator();
				EditorUtils.TaskTitlebar(currentViewCondition);
			}
		}

		public void AddCondition(ConditionTask condition){
			Undo.RecordObject(this, "List Add Task");
			Undo.RecordObject(condition, "List Add Task");
			currentViewCondition = condition;
			conditions.Add(condition);
			condition.SetOwnerSystem(ownerSystem);
		}

		#endif
	}
}