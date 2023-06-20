using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public static NextLevel instance;
    [SerializeField] GameObject[] Levels;
    int currentLevel = 0;
    GameObject tempLevel;
    GameManager gameManager;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        gameManager = GameManager.instance;
        if (Levels[0] != null)
        {
            tempLevel = Instantiate(Levels[0], Vector3.zero, Quaternion.identity);
        }
    }

    public void Next()
    {
        
        currentLevel++;
        if(currentLevel < Levels.Length)
        {
            Destroy(tempLevel);
            tempLevel = Instantiate(Levels[currentLevel], Vector3.zero, Quaternion.identity);
            gameManager.DisablePanel(gameManager.nextLevelPanel);
            gameManager.ResetStars();
        }
        else
        {
            gameManager.DisablePanel(gameManager.nextLevelPanel);
            gameManager.EnablePanel(gameManager.soonPanel);
        }
    }
    public void ReloadLevel()
    {
        Destroy(tempLevel);
        tempLevel = Instantiate(Levels[currentLevel], Vector3.zero, Quaternion.identity);
    }


}
