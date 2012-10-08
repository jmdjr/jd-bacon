﻿
using UnityEditor;
using UnityEngine;
public class EditorGUIUtilityFindTexture : EditorWindow 
{

    string path = "";

    [MenuItem("Examples/Check Path For Texture")]
    static void Init() {
        EditorGUIUtilityFindTexture window = (EditorGUIUtilityFindTexture)GetWindow(typeof(EditorGUIUtilityFindTexture));
        window.position = new Rect(0,0,180,55);
        window.Show();
    }
    
    void OnGUI() {
        path = EditorGUILayout.TextField("Path To Test:", path);
        if(GUILayout.Button("Check"))
            if(EditorGUIUtility.Load(path) != null) {
                Debug.Log("Yay!, texture found at: " + path);
            } else {
                Debug.LogError("No texture found at: " + path + " Check your path");
            }
    }
}