using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using HVRCORE;

[CustomEditor (typeof(HVRRenderMgrCore))]
public class HVRRenderMgrCoreEditor : UnityEditor.Editor
{
	
	private static Texture2D _icon;
	private static bool _expandAbout = false;
	private const string LinkPluginWebsite = "http://developer.huawei.com/consumer/cn/devunion/ui/server/virtualReality.html";
	private const string LinkForumPage = "http://club.huawei.com/forum-2406-1.html";
	private const string LinkUserManual = "http://developer.huawei.com/consumer/cn/wiki/index.php?title=HuaweiVR_SDK_for_Unity%E5%BC%80%E5%8F%91%E6%8C%87%E5%8D%97";
	private const string LinkScriptingClassReference = "http://developer.huawei.com/consumer/cn/wiki/index.php?title=HuaweiVR_Unity_API%E8%AF%B4%E6%98%8E";
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public override void OnInspectorGUI ()
	{

		EditorGUILayout.BeginVertical ("box");

		EditorGUILayout.LabelField ("Script", EditorStyles.boldLabel);
/*
        //Add by w00417917
        SerializedProperty singlePass = serializedObject.FindProperty("singlePass");
        EditorGUILayout.PropertyField(singlePass);
        //End
*/
        SerializedProperty renderDepth = serializedObject.FindProperty("renderDepthFormat");
        EditorGUILayout.PropertyField(renderDepth);

		SerializedProperty renderResolution = serializedObject.FindProperty ("renderResolutionScale");
		EditorGUILayout.PropertyField (renderResolution);

		serializedObject.ApplyModifiedProperties();

		EditorGUILayout.EndVertical ();

		if (!Application.isPlaying) {
			OnInspectorGUI_About ();
		}
	
	}

	private void OnInspectorGUI_About ()
	{

		GUI.color = Color.white;
		GUI.backgroundColor = Color.clear;
		if (_expandAbout) {
			GUI.color = Color.white;
			GUI.backgroundColor = new Color (0.8f, 0.8f, 0.8f, 0.1f);
			if (EditorGUIUtility.isProSkin) {
				GUI.backgroundColor = Color.black;
			}
		}
		GUILayout.BeginVertical ("box");
		GUI.backgroundColor = Color.white;
		if (GUILayout.Button ("About", EditorStyles.toolbarButton)) {
			_expandAbout = !_expandAbout;
		}
		GUI.color = Color.white;

		if (_expandAbout) {
			string version = "Unknown";
			version = "v2.0.0.27";
			EditorGUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			if (_icon == null) {
				_icon = Resources.Load<Texture2D> ("Textures/HVRIcon/HuaweiVRIcon");
			}
			if (_icon != null) {
				GUILayout.Label (new GUIContent (_icon));
			}
			GUILayout.FlexibleSpace ();
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			GUI.color = Color.yellow;
			GUILayout.Label ("HuaweiVR SDK2.0", EditorStyles.boldLabel);
			GUI.color = Color.white;
			GUILayout.FlexibleSpace ();
			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			GUI.color = Color.yellow;
			GUILayout.Label ("version " + version);
			GUI.color = Color.white;
			GUILayout.FlexibleSpace ();
			EditorGUILayout.EndHorizontal ();



			GUILayout.Space (32f);
			GUI.backgroundColor = Color.white;

			EditorGUILayout.LabelField ("HuaweiVR SDK2.0 Links", EditorStyles.boldLabel);

			GUILayout.Space (8f);

			EditorGUILayout.LabelField ("Documentation");
			if (GUILayout.Button ("Development Guide", GUILayout.ExpandWidth (false))) {
				Application.OpenURL (LinkUserManual);
			}
			if (GUILayout.Button ("Class Reference", GUILayout.ExpandWidth (false))) {
				Application.OpenURL (LinkScriptingClassReference);
			}

			GUILayout.Space (16f);

			GUILayout.Label ("Community");
			if (GUILayout.Button ("Development Forum Page", GUILayout.ExpandWidth (false))) {
				Application.OpenURL (LinkForumPage);
			}

			GUILayout.Space (16f);

			GUILayout.Label ("Homepage", GUILayout.ExpandWidth (false));
			if (GUILayout.Button ("HuaweiVR SDK2.0 Website", GUILayout.ExpandWidth (false))) {
				Application.OpenURL (LinkPluginWebsite);
			}
		}

		EditorGUILayout.EndVertical ();
	}
}
