using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] int hidden;
    [SerializeField] int found = 0;
    int mistakes = 3;
    public bool pause = false;
    [SerializeField] TextMeshProUGUI foundText;
    [SerializeField] TextMeshProUGUI hiddenText;
    [SerializeField] GameObject starPrefab;
    GameObject[] stars;
    [SerializeField] GameObject starsPanel;

    [Header("Button UI")]
    [SerializeField] GameObject CurtainPanel;
    [SerializeField] GameObject NextLevelPanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        StarGenerator();
        UpdateText();
    }
    public void finding()
    {
        found++;
        UpdateText();
        if (found >= hidden)
        {
            EnablePanel(NextLevelPanel);
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
    void StarGenerator()
    {
        stars = new GameObject[mistakes];
        stars[0] = Instantiate(starPrefab, starsPanel.transform.position + new Vector3(-30, 0, 0), Quaternion.identity, starsPanel.transform);
        for (int i = 1; i < mistakes; i++)
        {
            Vector3 newLocation = stars[i-1].transform.position + new Vector3(30, 0, 0);
            stars[i] = Instantiate(starPrefab, newLocation, Quaternion.identity, starsPanel.transform);
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
        CurtainPanel.SetActive(true);
        panel.SetActive(true);
        panel.GetComponent<Animator>().Play(0);
    }

    public void DisablePanel(GameObject panel)
    {
        CurtainPanel.SetActive(false);
        panel.SetActive(false);
        pause = false;
    }

    public void ReloadLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel);
    }
}
