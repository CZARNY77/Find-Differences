using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    [SerializeField] GameObject[] Levels;
    int currentLevel = 0;
    GameObject tempLevel;
    void Start()
    {
        if (Levels[0] != null)
        {
            tempLevel = Instantiate(Levels[0], Vector3.zero, Quaternion.identity);
        }
    }

    public void Next()
    {
        
        currentLevel++;
        Destroy(tempLevel);
        tempLevel = Instantiate(Levels[currentLevel], Vector3.zero, Quaternion.identity);
    }
}
