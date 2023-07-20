using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LoadPicture))]
public class CreateFinds : Editor
{
    bool foldoutOpen = false;
    int countFinds = 0;
    public override void OnInspectorGUI()
    {
        
        DrawDefaultInspector();
        //LoadPicture createFinds = (LoadPicture)target;
        //EditorGUILayout.LabelField("Dev Tools", EditorStyles.boldLabel);
        foldoutOpen = EditorGUILayout.Foldout(foldoutOpen, "Dev Tools");
        if (foldoutOpen)
        {
            countFinds = EditorGUILayout.IntField("Count Finds:", countFinds);
            if (GUILayout.Button("Create Finds"))
            {
                Debug.Log("tworze znajdzki");
            }
            if (GUILayout.Button("Create Json"))
            {
                Debug.Log("twore Json's");
            }
        }
    }
}
