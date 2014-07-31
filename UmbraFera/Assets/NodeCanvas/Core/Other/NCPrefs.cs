﻿#if UNITY_EDITOR

using UnityEditor;

namespace NodeCanvas{

	///Holds preferences
	public static class NCPrefs {

		static bool loaded = false;
		static bool _showNodeInfo;
		static bool _isLocked;
		static bool _iconMode;
		static int _curveMode;
		static bool _doSnap;
		static bool _showTaskSummary;
		static bool _showBlackboard;
		static bool _autoConnect;

		public static bool showNodeInfo{
			get {if (!loaded) Load(); return _showNodeInfo;}
			set {_showNodeInfo = value; Save();}
		}

		public static bool isLocked{
			get {if (!loaded) Load(); return _isLocked;}
			set {_isLocked = value; Save();}
		}

		public static bool iconMode{
			get {if (!loaded) Load(); return _iconMode;}
			set {_iconMode = value; Save();}
		}

		public static int curveMode{
			get {if (!loaded) Load(); return _curveMode;}
			set {_curveMode = value; Save();}
		}
		
		public static bool doSnap{
			get {if (!loaded) Load(); return _doSnap;}
			set {_doSnap = value; Save();}
		}

		public static bool showTaskSummary{
			get {if (!loaded) Load(); return _showTaskSummary;}
			set {_showTaskSummary = value; Save();}
		}

		public static bool showBlackboard{
			get {if (!loaded) Load(); return _showBlackboard;}
			set {_showBlackboard = value; Save();}
		}

		public static bool autoConnect{
			get {if (!loaded) Load(); return _autoConnect;}
			set {_autoConnect = value; Save();}
		}

		static void Load(){
			_showNodeInfo    = EditorPrefs.GetBool("NC.NodeInfo", true);
			_isLocked        = EditorPrefs.GetBool("NC.IsLocked", false);
			_iconMode        = EditorPrefs.GetBool("NC.IconMode", true);
			_curveMode       = EditorPrefs.GetInt("NC.CurveMode", 0);
			_doSnap          = EditorPrefs.GetBool("NC.DoSnap", true);
			_showTaskSummary = EditorPrefs.GetBool("NC.TaskSummary", true);
			_showBlackboard  = EditorPrefs.GetBool("NC.ShowBlackboard", true);
			_autoConnect     = EditorPrefs.GetBool("NC.AutoConnect", false);
			loaded           = true;
		}

		static void Save(){
			EditorPrefs.SetBool("NC.NodeInfo", _showNodeInfo);
			EditorPrefs.SetBool("NC.IsLocked", _isLocked);
			EditorPrefs.SetBool("NC.IconMode", _iconMode);
			EditorPrefs.SetInt("NC.CurveMode", _curveMode);
			EditorPrefs.SetBool("NC.DoSnap", _doSnap);
			EditorPrefs.SetBool("NC.TaskSummary", _showTaskSummary);
			EditorPrefs.SetBool("NC.ShowBlackboard", _showBlackboard);
			EditorPrefs.SetBool("NC.AutoConnect", _autoConnect);
		}
	}
}

#endif