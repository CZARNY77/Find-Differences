using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager instance;
    SpriteRenderer backgroundRenderer;
    private Color startColor;
    private Color endColor;
    private float duration = 0.25f;
    private float currentTime = 0f;
    int reversal = 1;
    bool startAnimation;

    private void Awake()
    {
        if(instance == null) instance = this;
    }
    void Start()
    {
        backgroundRenderer = GetComponent<SpriteRenderer>();
        startColor = backgroundRenderer.color;
        Vector3 screen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
        screen.z = -0.5f;
        transform.position += new Vector3(0,0,1f);
        transform.localScale = screen * -2;
    }

    private void Update()
    {
        if (startAnimation)
        {
            currentTime += Time.deltaTime * reversal;
            float t = currentTime / duration;
            Color currentColor = Color.Lerp(startColor, endColor, t);
            backgroundRenderer.color = currentColor;
            
            if (t >= 1f || t <= 0f)
            {
                reversal *= -1;
            }
            if(t <= 0f) startAnimation = false;
        }
    }

    public void LoadBackground(Sprite sprite)
    {
        backgroundRenderer.sprite = sprite;
    }

    public void StartAnimation(Color color)
    {
        currentTime = 0f;
        reversal = 1;
        backgroundRenderer.color = startColor;
        endColor = color;
        startAnimation = true;
    }
}
