using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{
    HiddenDifference hiddenDifference;
    void Update()
    {
        transform.Rotate(new Vector3(0,0,200f *Time.deltaTime));
        if(hiddenDifference != null && hiddenDifference.GetFound() == true) Destroy(gameObject);
    }

    public void SetFind(HiddenDifference find)
    {
        hiddenDifference = find;
    }
}
