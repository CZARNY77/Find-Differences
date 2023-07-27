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
    int mistakes = 3;
    int tempMistakes;
    int currentIndexLevel;
    public bool pause = false;
    public int currentLevel = 1;

    [Header("Stars")]
    [SerializeField] GameObject starPrefab;
    GameObject[] starsInGame;
    GameObject[] starsInPanel;
    [SerializeField] GameObject starsPanel;
    [SerializeField] GameObject starsNextLevelPanel;

    [Header("Text UI")]
    [SerializeField] Text foundText;
    [SerializeField] Text hiddenText;

    [Header("Panel UI")]
    [SerializeField] GameObject curtainPanel;
    [SerializeField] GameObject collectiblePanel;
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
        if (currentIndexLevel >= 1)
        {
            starsInGame = new GameObject[3];
            starsInPanel = new GameObject[3];
            currentLevel = 1;
            UpdateText();
            StarGenerator(starsPanel, starsInGame);
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
            ResetStars(false);
            StarGenerator(starsNextLevelPanel, starsInPanel);
        }
    }
    public void lossStars(bool inGame)
    {

        if (mistakes >= 0)
        {
            if (inGame)
            {
                mistakes--;
                Destroy(starsInGame[mistakes]);
            }
            else
            {
                tempMistakes--;
                Destroy(starsInPanel[tempMistakes]);
            }
        }
    }
    public void ResetStars(bool inGame)
    {
        if (inGame)
        {
            while (mistakes >= 1)
            {
                lossStars(inGame);
            }

            mistakes = 3;
            StarGenerator(starsPanel, starsInGame);
        }
        else
        {
            tempMistakes = starsNextLevelPanel.transform.childCount;
            while (tempMistakes >= 1)
            {
                lossStars(inGame);
            }
        }
    }
    public void CountsHidden()
    {
        hidden++;
        hiddenText.text = hidden.ToString(); // do poprawy
    }
    void StarGenerator(GameObject objectParent, GameObject[] stars)
    {
        if (mistakes > 0)
        {
            stars[0] = Instantiate(starPrefab, Vector3.zero, Quaternion.identity, objectParent.transform);
            for (int i = 1; i < mistakes; i++)
            {
                stars[i] = Instantiate(starPrefab, Vector3.zero, Quaternion.identity, objectParent.transform);
            }
        }
    }
    void UpdateText()
    {
        foundText.text = found.ToString();
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
        UpdateText();
    }
    public void ReloadLevel(bool isPanel)
    {
        LoadPicture.instance.DeleteHiddenDifference(false);
        found = 0;
        UpdateText();
        ResetStars(true);
        if (isPanel)
        {
            PlayAnim(curtainPanel, true);
            PlayAnim(nextLevelPanel, true);
            StartCoroutine(DisablePanel(nextLevelPanel));
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
        currentLevel++;
        hidden = 0;
        ResetStars(true);
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
}
