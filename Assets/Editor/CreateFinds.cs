using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

[CustomEditor(typeof(LoadPicture))]
public class CreateFinds : Editor
{
    bool foldoutOpen = false;
    int level = 1;
    int countFinds = 0;
    public override void OnInspectorGUI()
    {
        
        DrawDefaultInspector();
        LoadPicture levels = (LoadPicture)target;
        //EditorGUILayout.LabelField("Dev Tools", EditorStyles.boldLabel);
        foldoutOpen = EditorGUILayout.Foldout(foldoutOpen, "Dev Tools");
        if (foldoutOpen)
        {
            level = EditorGUILayout.IntField("Level: ", level);
            countFinds = EditorGUILayout.IntField("Finds To Add:", countFinds);
            EditorGUILayout.LabelField("Load", EditorStyles.boldLabel);
            if (GUILayout.Button("Load Picture"))
            {
                try {
                    levels.SetSprites();
                    levels.EditorLoadPictures(level);
                }
                catch { Debug.LogError("Images not found"); }
            }
            if (GUILayout.Button("Load Json"))
            {

                try {
                    levels.SetDifferences();
                    levels.EditorLoadFinds(level); 
                }
                catch { Debug.LogError("Json not found"); }
            }
            EditorGUILayout.LabelField("Create", EditorStyles.boldLabel);
            if (GUILayout.Button("Create Finds"))
            {
                Transform parent = levels.transform.GetChild(2);
                Vector3 offset = new Vector3(0, 0, -0.1f);
                for (int i = 0; i < countFinds; i++)
                {
                    Instantiate(levels.hiddenDifference, parent.position + offset, Quaternion.identity, parent);
                }
            }
            if (GUILayout.Button("Create Json"))
            {
                string filePath;
                filePath = Path.Combine(Application.streamingAssetsPath, "Level " + level + "/finds.json");
                if(File.Exists(filePath))
                {
                    Debug.LogError("The Json file already exists");
                    return;
                }
                string folderPath = Path.Combine(Application.streamingAssetsPath, "Level " + level);
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);



                Transform parent = levels.transform.GetChild(2);
                HiddenDifference[] hiddenDifferences = parent.GetComponentsInChildren<HiddenDifference>();
                FindData[] findDatas = new FindData[hiddenDifferences.Length];

                for(int i = 0; i < hiddenDifferences.Length; i++)
                {
                    GameObject tempHiddenDifference = hiddenDifferences[i].gameObject;
                    findDatas[i] = new FindData {
                        x = tempHiddenDifference.transform.position.x - parent.position.x,
                        y = tempHiddenDifference.transform.position.y - parent.position.y,
                        size = tempHiddenDifference.GetComponent<CapsuleCollider2D>().size,
                        direction = tempHiddenDifference.GetComponent<CapsuleCollider2D>().direction,
                        rotation = tempHiddenDifference.transform.rotation.z
                };
                }

                FindsData findsData = new FindsData { finds = findDatas };


                string json = JsonUtility.ToJson(findsData);
                File.WriteAllText(filePath, json);

                Debug.Log("Plik JSON zosta³ utworzony i zapisany.");
            }
            EditorGUILayout.LabelField("Delete", EditorStyles.boldLabel);
            if (GUILayout.Button("Delete all finds"))
            {
                HiddenDifference[] hiddenDifferences = levels.GetComponentsInChildren<HiddenDifference>();
                foreach(HiddenDifference hiddenDifference in hiddenDifferences)
                {
                    DestroyImmediate(hiddenDifference.gameObject);
                }
            }
            if (GUILayout.Button("Delete Player Prefs"))
            {
                PlayerPrefs.DeleteAll();
            }
        }
    }
}
