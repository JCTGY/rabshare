using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DisasterLevelEvent : MonoBehaviour
{
    private Text levelText;
    private UnityAction updateLevelListener;

    void Awake()
    {
        updateLevelListener = new UnityAction(UpdateLevel);
        levelText = GetComponent<Text>();
    }

    void OnEnable()
    {
        EventManager.StartListening("UpdateLevel", updateLevelListener);
    }

    void OnDisable()
    {
        EventManager.StopListening("UpdateLevel", updateLevelListener);
    }

    void UpdateLevel()
    {
        levelText.text = GameMaster.CurrentDisasterLevel.ToString();
    }
}
