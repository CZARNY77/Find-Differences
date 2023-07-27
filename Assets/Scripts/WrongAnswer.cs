using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrongAnswer : MonoBehaviour
{
    GameObject tempWrongAnswer;
    [SerializeField] GameObject wrongAnswer;

    private void OnMouseDown()
    {
        if (!GameManager.instance.pause)
        {
            if (tempWrongAnswer != null)
            {
                DestroyMe();
                CancelInvoke("DestroyMe");
            }
            GameManager.instance.lossStars(true);
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
