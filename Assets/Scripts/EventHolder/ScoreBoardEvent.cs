using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreBoardEvent : MonoBehaviour
{
    private Text scoreText;
    private UnityAction updateScoreListener;
    public static bool scoring;

    void Awake ()
    {
        updateScoreListener = new UnityAction (UpdateScore);
        scoreText = GetComponent<Text>();
    }

    void OnEnable()
    {
        EventManager.StartListening ("UpdateScore", updateScoreListener);
    }

    void OnDisable()
    {
        EventManager.StopListening ("UpdateScore", updateScoreListener);
    }

    void UpdateScore()
    {
        if (SceneManager.GetActiveScene().name.Contains("Score") || SceneManager.GetActiveScene().name.Contains("Game Over"))
        {
            scoreText.text = GameMaster.CurrentScore.ToString();
            if (!scoring) StartCoroutine(collectScore());
        }
        else
            scoreText.text = GameMaster.CurrentScore.ToString();
    }

    IEnumerator collectScore()
    {
        scoring = true;
        int newScore = GameMaster.CurrentScore + GameMaster.CurrentScoreBP;
        while (GameMaster.CurrentScore < newScore)
        {
            GameMaster.CurrentScore++;
            scoreText.text = GameMaster.CurrentScore.ToString();
            yield return new WaitForSeconds(0.005f);
        }
        scoreText.text = GameMaster.CurrentScore.ToString();
        scoring = false;
    }
}
