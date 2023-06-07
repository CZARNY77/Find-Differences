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
        SpriteRenderer left = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        SpriteRenderer right = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
        left.sprite = orginalSprite;
        left.size = new Vector2(1, 1);
        right.sprite = secondSprite;
        right.size = new Vector2(1, 1);
    }
    public void CreateCollectibles()
    {
        if(hiddenDifference != null)
        {
            for (int i = 0; i < collectibles; i++)
            {
                Instantiate(hiddenDifference, new Vector3(0,0, -0.2f), Quaternion.identity, transform.GetChild(2));
            }
        }
    }

    private void OnMouseDown()
    {
        if (!GameManager.instance.pause)
        {
            if (tempWrongAnswer != null)
            {
                DestroyMe();
                CancelInvoke("DestroyMe");
            }
            GameManager.instance.lossStars();
            Vector2 newLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tempWrongAnswer = Instantiate(wrongAnswer, newLocation, Quaternion.identity);
            Invoke("DestroyMe", 1);
        }
    }
    void DestroyMe()
    {
        Destroy(tempWrongAnswer);
    }
}
