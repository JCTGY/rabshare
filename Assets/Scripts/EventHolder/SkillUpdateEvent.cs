using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillUpdateEvent : MonoBehaviour
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
        EventManager.StartListening("UpdateSkill", updateSkillListener);
    }

    void OnDisable()
    {
        EventManager.StopListening("UpdateSkill", updateSkillListener);
    }

    void UpdateScore()
    {
        scoreText.text = GameMaster.CurrentScoreAct.ToString();
    }
}
