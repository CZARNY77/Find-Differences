using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] int hidden;
    [SerializeField] int found = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void CheckWin()
    {
        if(found >= hidden)
        {
            Debug.Log("Wygra³eœ");
        }
    }

    public void finding()
    {
        found++;
    }

    public void CountsHidden(int collectibles)
    {
        hidden = collectibles;
    }
}
