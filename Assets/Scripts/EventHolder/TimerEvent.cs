using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerEvent : MonoBehaviour
{
    private Text timeText;

    private float minutes;
    private float seconds;

    private bool stop = false;

    void Awake()
    {
        timeText = GetComponent<Text>();
    }

    private void Update()
    {
        if (stop)
            return;

        GameMaster.timeLeftToCompleteLevel -= Time.deltaTime;

        minutes = Mathf.Floor(GameMaster.timeLeftToCompleteLevel / 60);
        seconds = GameMaster.timeLeftToCompleteLevel % 60;

        if (seconds > 59) seconds = 59;

        if (GameMaster.timeLeftToCompleteLevel <= 0.0f)
        {
            stop = true;
            GameMaster.timeLeftToCompleteLevel = 0.0f;
            minutes = 0.0f;
            seconds = 0.0f;
        }

        string timeLeft = string.Format("Time: {0:0}:{1:00}", minutes, seconds);
        timeText.text = timeLeft;
    }
}
