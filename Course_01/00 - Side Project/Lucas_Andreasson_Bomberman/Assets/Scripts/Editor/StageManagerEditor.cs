using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

[CustomEditor(typeof(StageManager))]
[CanEditMultipleObjects]
public class StageManagerEditor : Editor
{
   

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(2);
        GUILayout.Label("Editor Only");

        StageManager script = (StageManager)target;

        if (GUILayout.Button("Build stage"))
        {
            script.CreateStage();
        }
    }
}
