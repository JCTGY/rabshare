using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BPScoreEvent : MonoBehaviour
{
    private Text scoreText;
    private UnityAction updateSkillListener;

    void Awake()
    {
        updateSkillListener = new UnityAction(UpdateScore);
        scoreText = GetComponent<Text>();
    }

    void OnEnable()
    {
        EventManager.StartListening("UpdateBP", updateSkillListener);
    }

    void OnDisable()
    {
        EventManager.StopListening("UpdateBP", updateSkillListener);
    }

    void UpdateScore()
    {
        scoreText.text = GameMaster.CurrentScoreBP.ToString();
    }
}
