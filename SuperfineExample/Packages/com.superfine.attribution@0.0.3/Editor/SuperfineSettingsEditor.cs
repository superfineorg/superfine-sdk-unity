using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[System.Serializable]
[CustomEditor(typeof(SuperfineSettings))]
public class SuperfineSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        var settings = target as SuperfineSettings;

        GUIStyle titleStyle = new GUIStyle();
        titleStyle.fontSize = 30;
        titleStyle.normal.textColor = Color.white;
        titleStyle.fontStyle = FontStyle.Bold;

        GUILayout.Label("Superfine SDK", titleStyle);
        GUILayout.Label("v0.0.3", EditorStyles.miniBoldLabel);

        GUILayout.Space(25f);
        GUILayout.Label("Project Id");
        settings.appId = GUILayout.TextField(settings.appId);
        GUILayout.Space(5f);
        GUILayout.Label("App Secret");
        settings.appSecret = GUILayout.TextField(settings.appSecret);

        GUILayout.Space(25f);
        GUILayout.Label("Tenjin API Key - iOS");
        settings.tenjinAPIKeyIOS = GUILayout.TextField(settings.tenjinAPIKeyIOS);
        GUILayout.Space(5f);
        GUILayout.Label("Tenjin API Key - Android");
        settings.tenjinAPIKeyAndroid = GUILayout.TextField(settings.tenjinAPIKeyAndroid);

        EditorUtility.SetDirty(settings);
        serializedObject.ApplyModifiedProperties();
    }
}
