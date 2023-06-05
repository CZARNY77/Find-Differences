using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pictures : MonoBehaviour
{
    [SerializeField] Sprite orginalSprite;
    [SerializeField] Sprite secondSprite;
    [SerializeField] GameObject wrongAnswer;
    [SerializeField] HiddenDifference hiddenDifference;
    [SerializeField] int collectibles = 1;
    GameObject tempWrongAnswer;
    void Start()
    {
        GameManager.instance.CountsHidden(transform.GetChild(2).childCount);
        LoadPictures();
    }

    public void LoadPictures()
    {
        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = orginalSprite;
        transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = secondSprite;
    }
    public void CreateCollectibles()
    {
        if(hiddenDifference != null)
        {
            for (int i = 0; i < collectibles; i++)
            {
                Instantiate(hiddenDifference, Vector3.zero, Quaternion.identity, transform.GetChild(2));
            }
        }
    }

    private void OnMouseDown()
    {
        if (tempWrongAnswer != null)
        {
            DestroyMe();
            CancelInvoke("DestroyMe");
        }

        Vector2 newLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tempWrongAnswer = Instantiate(wrongAnswer, newLocation, Quaternion.identity);
        Invoke("DestroyMe", 2);
    }
    void DestroyMe()
    {
        Destroy(tempWrongAnswer);
    }
}
