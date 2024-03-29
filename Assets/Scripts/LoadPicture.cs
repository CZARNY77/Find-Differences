using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

[System.Serializable]
public class FindData
{
    public float x;
    public float y;
    public Vector2 size;
    public CapsuleDirection2D direction;
    public float rotation;
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
    Sprite[] loadSprite;
    [SerializeField] GameObject hint;
    [SerializeField] GameObject loadingPanel;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        loadSprite = new Sprite[2];
        SetSprites();
        SetDifferences();
        LoadLevel();
    }

    public void LoadLevel()
    {
        int currentLevel = GameManager.instance.currentLevel;
        StartCoroutine(LoadPictures(currentLevel));
        StartCoroutine(LoadFinds(currentLevel));
    }

    public void DeleteHiddenDifference(bool del)
    {
        HiddenDifference[] hiddenDifferences;
        hiddenDifferences = differences.GetComponentsInChildren<HiddenDifference>();
        foreach (HiddenDifference hd in hiddenDifferences)
        {
            if(del) Destroy(hd.gameObject);
            else
            {
                hd.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                hd.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
                hd.SetFound();
            }
        }
    }

    public IEnumerator LoadPictures(int currentLevel)
    {
        string filePath;
        for (int i = 0; i < sprites.Length; i++)
        {
            
            int rand = (int)(Random.value * 10000);
            filePath = Path.Combine(Application.streamingAssetsPath, "Level " + currentLevel + "/" + (i + 1) + ".png?" + rand);
            //filePath = Path.Combine(Application.streamingAssetsPath, "Level " + currentLevel + "/" + (i + 1) + ".png");

            UnityWebRequest www = UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                byte[] bytes = www.downloadHandler.data;
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(bytes);
                loadSprite[i] = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            }
            else
            {
                GameManager.instance.DisablePanel();
                StartCoroutine(GameManager.instance.LastLevel());
                yield break;
            }
        }
        for (int i = 0; i < sprites.Length; i++)
        { 
            sprites[i].sprite = loadSprite[i]; 
        }
        if(currentLevel != PlayerPrefs.GetInt("CurrentLevel", 1))   GameManager.instance.DisablePanel();
        else    loadingPanel.SetActive(false); 
        BackgroundManager.instance.LoadBackground(sprites[0].sprite);
        Clock.instance.ResetTime();
    }

    public IEnumerator LoadFinds(int currentLevel)
    {
        string filePath;
        filePath = Path.Combine(Application.streamingAssetsPath, "Level " + currentLevel + "/finds.json");
        UnityWebRequest www = UnityWebRequest.Get(filePath);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success) 
        {
            byte[] bytes = www.downloadHandler.data;
            string jsonText = System.Text.Encoding.UTF8.GetString(bytes);
            FindsData findsData = JsonUtility.FromJson<FindsData>(jsonText);
            foreach (FindData find in findsData.finds)
            {
                Vector3 newPosition = differences.position + new Vector3(find.x, find.y, -0.1f);
                GameObject currentHiddenDifference = Instantiate(hiddenDifference, newPosition, Quaternion.identity, differences);
                currentHiddenDifference.GetComponent<CapsuleCollider2D>().size = find.size;
                currentHiddenDifference.GetComponent<CapsuleCollider2D>().direction = find.direction;
                currentHiddenDifference.transform.rotation = Quaternion.Euler(0,0,find.rotation);
                try { GameManager.instance.CountsHidden(); }
                catch { Debug.Log("success, find loaded"); }
            }
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
    public void Hint()
    {
       if(GameManager.instance.hint >= 1)
        {
            HiddenDifference[] hiddenDifferences = FindObjectsOfType<HiddenDifference>();
            HiddenDifference tempHiddenDifference;
            foreach (HiddenDifference hiddenDifference in hiddenDifferences)
            {
                if (hiddenDifference.GetFound() == false)
                {
                    tempHiddenDifference = hiddenDifference;
                    GameObject tempHint = Instantiate(hint, tempHiddenDifference.transform.position, Quaternion.identity);
                    tempHint.GetComponent<Hint>().SetFind(tempHiddenDifference);
                    GameManager.instance.UpdateHint(-1);
                    break;
                }
            }
        }
    }

#if UNITY_EDITOR
    public void EditorLoadPictures(int currentLevel)
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

    public void EditorLoadFinds(int currentLevel)
    {
        string filePath;
        filePath = Path.Combine(Application.streamingAssetsPath, "Level " + currentLevel + "/finds.json");
        string jsonString = File.ReadAllText(filePath);

        //byte[] bytes = www.downloadHandler.data;
        //string jsonText = System.Text.Encoding.UTF8.GetString(bytes);
        FindsData findsData = JsonUtility.FromJson<FindsData>(jsonString);
        foreach (FindData find in findsData.finds)
        {
            Vector3 newPosition = differences.position + new Vector3(find.x, find.y, -0.1f);
            GameObject currentHiddenDifference = Instantiate(hiddenDifference, newPosition, Quaternion.identity, differences);
            currentHiddenDifference.GetComponent<CapsuleCollider2D>().size = find.size;
            currentHiddenDifference.GetComponent<CapsuleCollider2D>().direction = find.direction;
            currentHiddenDifference.transform.rotation = Quaternion.Euler(0, 0, find.rotation);
            try { GameManager.instance.CountsHidden(); }
            catch { Debug.Log("success, find loaded"); }
        }

    }
#endif
}
