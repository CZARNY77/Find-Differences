using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondDifference : MonoBehaviour
{
    HiddenDifference hiddenDifference;
    void Start()
    {
        hiddenDifference = GetComponentInParent<HiddenDifference>();
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        collider.radius = hiddenDifference.GetComponent<CircleCollider2D>().radius;
    }

    private void OnMouseDown()
    {
        hiddenDifference.Click();
    }
}
