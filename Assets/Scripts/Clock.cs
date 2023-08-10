using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    public static Clock instance;
    float timePassed = 0f;
    public int minutes, seconds;
    Text timeText;
    public bool stopClock = false;

    private void Awake()
    {
        if(instance == null) instance = this;
    }

    void Start()
    {
            timeText = GetComponent<Text>();
    }


    void Update()
    {
        if (!stopClock)
        {
            timePassed += Time.deltaTime;
            if (timePassed >= 1f)
            {
                AddTime(1);

                timePassed = 0f;
            }

            if (Input.GetKeyDown(KeyCode.A)) AddTime(30);
        }
    }

    public void AddTime(int sec)
    {
        seconds += sec;
        if (seconds >= 60f)
        {
            minutes++;
            seconds -= 60;
        }
        timeText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }

    public void ResetTime()
    {
        minutes = 0;
        seconds = -1;
        stopClock = false;
    }
}
