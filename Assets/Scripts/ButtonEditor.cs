using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Pictures))]
public class ButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Pictures pictures = (Pictures)target;
        if(GUILayout.Button("Load Pictures"))
        {
            pictures.LoadPictures();
        }
        if (GUILayout.Button("Create collectibles"))
        {
            pictures.CreateCollectibles();
        }
    }
}
