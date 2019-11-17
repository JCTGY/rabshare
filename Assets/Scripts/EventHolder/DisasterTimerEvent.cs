using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DisasterTimerEvent : MonoBehaviour
{
    private Text timerText;
    private UnityAction updateTimerListener;

    void Awake()
    {
        updateTimerListener = new UnityAction(UpdateTimer);
        timerText = GetComponent<Text>();
    }

    void OnEnable()
    {
        EventManager.StartListening("UpdateTimer", updateTimerListener);
    }

    void OnDisable()
    {
        EventManager.StopListening("UpdateTimer", updateTimerListener);
    }

    void UpdateTimer()
    {
        decimal bar = Convert.ToDecimal(GameMaster.CurrentTime);

        bar = Math.Round(bar, 2);
        timerText.text = bar.ToString();
    }
}
