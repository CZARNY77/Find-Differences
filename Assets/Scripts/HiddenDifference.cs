using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenDifference : MonoBehaviour
{
    public Sprite check;
    public float offset;
    bool found = false;
    SpriteRenderer spriteRenderer;
    SpriteRenderer secondDifference;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = check;
        secondDifference = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        secondDifference.sprite = check;
        secondDifference.GetComponent<Transform>().transform.position = transform.position + new Vector3(offset, 0, 0);

    }
    private void OnMouseDown()
    {
        Click();
    }

    public void Click()
    {
        if (!found && !GameManager.instance.pause)
        {
            spriteRenderer.enabled = true;
            secondDifference.enabled = true;
            found = true;
            GameManager.instance.Finding();
            BackgroundManager.instance.StartAnimation(Color.green);
        }
    }
    public void SetFound()
    {
        found = false;
    }
    public bool GetFound()
    {
        return found;
    }
}
