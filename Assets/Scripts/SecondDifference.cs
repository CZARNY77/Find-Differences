using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondDifference : MonoBehaviour
{
    HiddenDifference hiddenDifference;
    void Start()
    {
        hiddenDifference = GetComponentInParent<HiddenDifference>();
        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        collider.size = hiddenDifference.GetComponent<CapsuleCollider2D>().size;
    }

    private void OnMouseDown()
    {
        hiddenDifference.Click();
    }
}
