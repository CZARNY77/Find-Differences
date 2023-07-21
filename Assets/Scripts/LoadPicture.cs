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
    public GameObject hiddenDifference;
    Transform differences;

    void Start()
    {
        if(instance == null)    instance = this;
        SetSprites();
        SetDifferences();
        LoadLevel();
    }

    public void LoadLevel()
    {
        int currentLevel = GameManager.instance.currentLevel;
        LoadPictures(currentLevel);
        LoadFinds(currentLevel);
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

    public void LoadPictures(int currentLevel)
    {
        string filePath;
        for (int i = 0; i < sprites.Length; i++)
        {
            filePath = Path.Combine(Application.streamingAssetsPath, "Level " + currentLevel + "/" + (i + 1) + ".png");

            byte[] bytes = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            Sprite loadSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            sprites[i].sprite = loadSprite;
        }
    }

    public void LoadFinds(int currentLevel)
    {
        string filePath;
        filePath = Path.Combine(Application.streamingAssetsPath, "Level " + currentLevel + "/finds.json");
        string jsonText = File.ReadAllText(filePath);
        FindsData findsData = JsonUtility.FromJson<FindsData>(jsonText);
        foreach (FindData find in findsData.finds)
        {
            Vector3 newPosition = differences.position + new Vector3(find.x, find.y, -0.1f);
            GameObject currentHiddenDifference = Instantiate(hiddenDifference, newPosition, Quaternion.identity, differences);
            currentHiddenDifference.GetComponent<CircleCollider2D>().radius = find.r;
            try { GameManager.instance.CountsHidden(); }
            catch { Debug.Log("success, find loaded"); }
        }
    }

    public void SetDifferences()
    {
        differences = transform.GetChild(2);
    }
    public void SetSprites()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }
}
