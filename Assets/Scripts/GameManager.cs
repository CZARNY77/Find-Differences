using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int hidden;
    [SerializeField] int found = 0;
    int mistakes = 3;
    int currentIndexLevel;
    public bool pause = false;
    public int currentLevel = 1;

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
        if (currentIndexLevel >= 1)
        {
            currentLevel = 1;
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
            StarGenerator(starsNextLevelPanel, true);
        }
    }
    public void lossStars()
    {
        mistakes--;
        if (mistakes >= 0)
            Destroy(stars[mistakes]);
    }
    public void CountsHidden()
    {
        hidden++;
        hiddenText.text = hidden.ToString(); // do poprawy
    }
    void StarGenerator(GameObject objectParent, bool end)
    {
        if (mistakes > 0)
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
        foundText.text = found.ToString();
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

    IEnumerator DisablePanel(GameObject panel)
    {
        yield return new WaitForSeconds(1f);
        curtainPanel.SetActive(false);
        panel.SetActive(false);
        pause = false;
        buttonPanel.SetActive(true);
        starsPanel.SetActive(true);
        PlayAnim(curtainPanel, false);
        PlayAnim(panel, false);
    }

    public void ReloadLevel(bool isPanel)
    {
        LoadPicture.instance.DeleteHiddenDifference(false);
        found = 0;
        UpdateText();
        ResetStars();
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

    public void ResetStars()
    {
        while (mistakes >= 1)
        {
            lossStars();
        }
        mistakes = 3;
        StarGenerator(starsPanel, false);
    }

    public void NextLevel()
    {
        currentLevel++;
        hidden = 0;
        LoadPicture.instance.DeleteHiddenDifference(true);

        LoadPicture.instance.LoadLevel();
        PlayAnim(curtainPanel, true);
        PlayAnim(nextLevelPanel, true);
        StartCoroutine(DisablePanel(nextLevelPanel));
        Invoke(nameof(ResetStars), 1f);
        UpdateText();

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
