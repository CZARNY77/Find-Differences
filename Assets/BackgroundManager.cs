using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager instance;

    private void Awake()
    {
        if(instance == null) instance = this;
    }
    void Start()
    {
        Vector3 screen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
        screen.z = -0.5f;
        transform.position += new Vector3(0,0,1f);
        transform.localScale = screen * -2;
    }

    public void LoadBackground(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
