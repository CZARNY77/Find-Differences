using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int hidden;
    [SerializeField] int found = 0;
    int currentIndexLevel;
    public bool pause = false;
    public int currentLevel;

    public int hint;

    [Header("Stars")]
    [SerializeField] GameObject starPrefab;
    [SerializeField] GameObject starsNextLevelPanel;

    [Header("Text UI")]
    [SerializeField] Text foundText;
    [SerializeField] Text hiddenText;
    [SerializeField] Text hintText;

    [Header("Panel UI")]
    [SerializeField] GameObject curtainPanel;
    GameObject[] stars;
    [SerializeField] GameObject collectiblePanel;
    public GameObject nextLevelPanel;
    [SerializeField] GameObject buttonPanel;
    public GameObject soonPanel;
    [SerializeField] Button[] buttonsNextLevelPanel;
    [SerializeField] GameObject loadingPanel;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
    }
    private void Start()
    {
        currentIndexLevel = SceneManager.GetActiveScene().buildIndex;
        if (currentIndexLevel >= 1)
        {
            hint = PlayerPrefs.GetInt("hint", 1);
            UpdateText(hintText, hint);
            stars = new GameObject[3];
            UpdateText(foundText, found);
        }
    }


    public void Finding()
    {
        found++;
        UpdateText(foundText, found);
        if (found >= hidden)
        {
            PlayerPrefs.SetInt("WinLevel" + currentLevel, 1);
            starsNextLevelPanel.SetActive(true);
            loadingPanel.SetActive(false);
            foreach (Button button in buttonsNextLevelPanel)
            {
                button.interactable = true;
            }
            EnablePanel(nextLevelPanel);
            found = 0;
            ResetStars();
            StarGenerator();
            Clock.instance.stopClock = true;
        }
    }
    public void ResetStars()
    {
        foreach(GameObject star in stars)
        {
            Destroy(star);
        }
    }
    public void CountsHidden()
    {
        hidden++;
        hiddenText.text = hidden.ToString(); // do poprawy
    }
    void StarGenerator()
    {
        int timeToLevel = hidden * 20;
        int countStars = ((Clock.instance.minutes * 60) + Clock.instance.seconds)/(timeToLevel / 3);
        countStars = 3 - countStars;
        if (countStars > PlayerPrefs.GetInt("StarsLevel" + currentLevel, 0)) PlayerPrefs.SetInt("StarsLevel" + currentLevel, countStars);
        if (countStars > 0)
        {
            stars[0] = Instantiate(starPrefab, Vector3.zero, Quaternion.identity, starsNextLevelPanel.transform);
            for (int i = 1; i < countStars; i++)
            {
                stars[i] = Instantiate(starPrefab, Vector3.zero, Quaternion.identity, starsNextLevelPanel.transform);
                if (i >= 2) UpdateHint(1);
            }
        }
    }
    void UpdateText(Text currentText, int value)
    {
        currentText.text = value.ToString();
    }

    //UI Button
    public void EnablePanel(GameObject panel)
    {
        pause = true;
        curtainPanel.SetActive(true);
        panel.SetActive(true);
    }

    IEnumerator DisablePanel(GameObject panel)
    {
        yield return new WaitForSeconds(1f);
        curtainPanel.SetActive(false);
        panel.SetActive(false);
        pause = false;
        PlayAnim(curtainPanel, false);
        PlayAnim(panel, false);
    }
    public void DisablePanel()
    {
        PlayAnim(curtainPanel, true);
        PlayAnim(nextLevelPanel, true);
        StartCoroutine(DisablePanel(nextLevelPanel));
        UpdateText(foundText, found);
    }
    public void ReloadLevel(bool isPanel)
    {
        LoadPicture.instance.DeleteHiddenDifference(false);
        found = 0;
        UpdateText(foundText, found);
        Clock.instance.ResetTime();
        if (isPanel)
        {
            PlayAnim(curtainPanel, true);
            PlayAnim(nextLevelPanel, true);
            StartCoroutine(DisablePanel(nextLevelPanel));
        }
        Hint[] hints = FindObjectsOfType<Hint>();
        foreach(Hint tempHint in hints)
        {
            Destroy(tempHint);
        }
    }

    public void LoadScene(int indexScene)
    {
        if (indexScene == -1)
            Application.Quit();
        else
            SceneManager.LoadScene(indexScene);
    }

    public void NextLevel()
    {
        foreach (Button button in buttonsNextLevelPanel)
        {
            button.interactable = false;
        }
        starsNextLevelPanel.SetActive(false);
        loadingPanel.SetActive(true);
        currentLevel++;
        hidden = 0;
        LoadPicture.instance.DeleteHiddenDifference(true);
        LoadPicture.instance.LoadLevel();
    }

    public void Resume(GameObject panel)
    {
        PlayAnim(curtainPanel, true);
        PlayAnim(panel, true);
        StartCoroutine(DisablePanel(panel));
    }
    public IEnumerator LastLevel()
    {
        yield return new WaitForSeconds(1f);
        EnablePanel(soonPanel);
    }

    void PlayAnim(GameObject panel, bool b)
    {
        panel.GetComponent<Animator>().SetBool("Exit", b);
    }

    public void UpdateHint(int value)
    {
        hint += value;
        UpdateText(hintText, hint);
        PlayerPrefs.SetInt("hint", hint);
    }
}
