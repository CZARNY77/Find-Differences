using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class FindData
{
    public float x;
    public float y;
    public float r;
}

[System.Serializable]
public class FindsData
{
    public FindData[] finds;
}

public class LoadPicture : MonoBehaviour
{
    static public LoadPicture instance;
    SpriteRenderer[] sprites;
    [SerializeField] GameObject hiddenDifference;
    int currentLevel;
    Transform differences;

    void Start()
    {
        if(instance == null)    instance = this;
        sprites = GetComponentsInChildren<SpriteRenderer>();
        differences = transform.GetChild(2);
        LoadLevel();
    }

    public void LoadLevel()
    {
        string filePath;

        currentLevel = GameManager.instance.currentLevel;
        for (int i = 0; i < sprites.Length; i++)
        {
            filePath = Path.Combine(Application.streamingAssetsPath, "Level " + currentLevel + "/" + (i + 1) + ".png");

            byte[] bytes = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            Sprite loadSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            sprites[i].sprite = loadSprite;
        }
        filePath = Path.Combine(Application.streamingAssetsPath, "Level " + currentLevel + "/finds.json");
        string jsonText = File.ReadAllText(filePath);
        FindsData findsData = JsonUtility.FromJson<FindsData>(jsonText);
        foreach (FindData find in findsData.finds)
        {
            Vector3 newPosition = differences.position + new Vector3(find.x, find.y, -0.1f);
            GameObject currentHiddenDifference = Instantiate(hiddenDifference, newPosition, Quaternion.identity, differences);
            currentHiddenDifference.GetComponent<CircleCollider2D>().radius = find.r;
            GameManager.instance.CountsHidden();
        }
    }

    public void DeleteHiddenDifference()
    {
        HiddenDifference[] hiddenDifferences;
        hiddenDifferences = differences.GetComponentsInChildren<HiddenDifference>();
        foreach (HiddenDifference hd in hiddenDifferences)
        {
            Destroy(hd.gameObject);
        }
    }
}
