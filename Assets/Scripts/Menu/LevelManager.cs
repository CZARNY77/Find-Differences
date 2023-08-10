using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LevelManager : MonoBehaviour, IPointerClickHandler
{
    Image image;
    public int level;
    Sprite spriteDefault;
    Sprite spriteLevel;
    public bool loaded = false;
    public bool unlocked = false;
    public bool soon = false;
    [SerializeField] GameObject starsPanel;
    [SerializeField] GameObject star;
    GameObject[] stars = new GameObject[3];

    void Start()
    {
        image = transform.GetChild(0).GetComponent<Image>();
        spriteDefault = image.sprite;
    }
    public IEnumerator DownloadPictures()
    {
        string filePath;
        
        int rand = (int)(Random.value * 10000);
        filePath = Path.Combine(Application.streamingAssetsPath, "Level " + level + "/Icon.png?" + rand);
        //filePath = Path.Combine(Application.streamingAssetsPath, "Level " + level + "/Icon.png");

        UnityWebRequest www = UnityWebRequest.Get(filePath);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
        {
            byte[] bytes = www.downloadHandler.data;
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            spriteLevel = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        }
        else
        {
            soon = true;
            spriteLevel = spriteDefault;
        }
        loaded = true;
        SetLevels.instance.VerificationIconLevels(level);
    }

    void LoadPictures()
    {
        if (image != null)
        {
            image.sprite = spriteLevel;
            LockedLevel();
        }
    }

    void LoadStars()
    {
        if(stars != null)   ResetStars();
        int countStars = PlayerPrefs.GetInt("StarsLevel"+level, 0);
        for (int i = 0; i < countStars; i++)
        {
            stars[i] = Instantiate(star, starsPanel.transform);
        }
    }

    void ResetStars()
    {
        foreach(GameObject s in stars)
        {
            if(s != null)
                Destroy(s);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (unlocked && !soon)
        {
            PlayerPrefs.SetInt("CurrentLevel", level);
            SceneManager.LoadScene(1);
        }
    }

    public void LoadIcons()
    {
        LoadPictures();
        LoadStars();
    }

    public void ResetIcons()
    {
        unlocked = false;
        soon = false;
        loaded = false;
    }

    void LockedLevel()
    {
        if(!unlocked)   image.color = Color.gray;
        else            image.color = Color.white;
    }
}
