using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenDifference : MonoBehaviour
{
    public Sprite check;
    public float offset;
    bool found = false;
    Transform secondDifference;
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = check;
        GetComponent<SpriteRenderer>().color = Color.green;
        secondDifference = gameObject.transform.GetChild(0);
        secondDifference.GetComponent<SpriteRenderer>().sprite = check;
        secondDifference.GetComponent<SpriteRenderer>().color = Color.green;
        secondDifference.GetComponent<Transform>().transform.position = transform.position + new Vector3(offset, 0, 0);

    }
    private void OnMouseDown()
    {
        if (!found)
        {
            Vector3 newlocation = new Vector3(offset, 0, 0) + transform.position;
            this.GetComponent<SpriteRenderer>().enabled = true;
            secondDifference.GetComponent<SpriteRenderer>().enabled = true;
            found = true;
            GameManager.instance.finding();
            GameManager.instance.CheckWin();
        }
    }
}
