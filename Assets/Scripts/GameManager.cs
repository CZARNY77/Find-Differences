using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] int hidden;
    [SerializeField] int found = 0;
    int mistakes = 3;
    int currentIndexLevel;
    public bool pause = false;

    [Header("Stars")]
    [SerializeField] GameObject starPrefab;
    GameObject[] stars;
    [SerializeField] GameObject starsPanel;
    [SerializeField] GameObject starsNextLevelPanel;

    [Header("Text UI")]
    [SerializeField] TextMeshProUGUI foundText;
    [SerializeField] TextMeshProUGUI hiddenText;

    [Header("Panel UI")]
    [SerializeField] GameObject curtainPanel;
    public GameObject nextLevelPanel;
    [SerializeField] GameObject buttonPanel;
    public GameObject soonPanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        currentIndexLevel = SceneManager.GetActiveScene().buildIndex;
        if(currentIndexLevel == 1)
        {
            UpdateText();
            StarGenerator(starsPanel, false);
        }
    }
    public void finding()
    {
        found++;
        UpdateText();
        if (found >= hidden)
        {
            EnablePanel(nextLevelPanel);
            found = 0;
            UpdateText();
            StarGenerator(starsNextLevelPanel, true);
        }
    }
    public void lossStars()
    {
        mistakes--;
        if(mistakes >= 0)
            Destroy(stars[mistakes]);
    }
    public void CountsHidden(int collectibles)
    {
        hidden = collectibles;
        hiddenText.text = hidden.ToString();
    }
    void StarGenerator(GameObject objectParent, bool end)
    {
       if(mistakes > 0)
       {
            stars = new GameObject[mistakes];
            stars[0] = Instantiate(starPrefab, starsPanel.transform.position + new Vector3(-30, 0, 0), Quaternion.identity, objectParent.transform);
            if (end) stars[0].GetComponent<Image>().rectTransform.sizeDelta *= 3;
            for (int i = 1; i < mistakes; i++)
            {
                Vector3 newLocation = stars[i - 1].transform.position + new Vector3(30, 0, 0);
                stars[i] = Instantiate(starPrefab, newLocation, Quaternion.identity, objectParent.transform);
                if (end) stars[i].GetComponent<Image>().rectTransform.sizeDelta *= 3;
            }
        }
    }
    void UpdateText()
    {
        foundText.text =found.ToString();
    }

    //UI Button
    public void EnablePanel(GameObject panel)
    {
        pause = true;
        curtainPanel.SetActive(true);
        panel.SetActive(true);
        buttonPanel.SetActive(false);
        starsPanel.SetActive(false);
    }

    public void DisablePanel(GameObject panel)
    {
        curtainPanel.SetActive(false);
        panel.SetActive(false);
        pause = false;
        buttonPanel.SetActive(true);
        starsPanel.SetActive(true);
    }

    public void ReloadLevel()
    {
        NextLevel.instance.ReloadLevel();
        found = 0;
        UpdateText();
        ResetStars();
        DisablePanel(nextLevelPanel);
    }

    public void LoadScene(int indexScene)
    {
        if(indexScene == -1)
            Application.Quit();
        else
            SceneManager.LoadScene(indexScene);
    }

    public void ResetStars()
    {
        while(mistakes >= 1)
        {
            lossStars();
        }
        mistakes = 3;
        StarGenerator(starsPanel, false);
    }
}