using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    Text timerText;
    bool blinking;

    // Start is called before the first frame update
    void Start()
    {
        timerText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameMaster.timeLeftToCompleteLevel <= 15.1f && GameMaster.timeLeftToCompleteLevel > 0.0f)
        {
            timerText.color = Color.red;
            if (blinking == false)
                startBlinking();
        }
        else if (blinking)
            stopBlinking();
    }

    void startBlinking()
    {
        StopCoroutine(makeTextBlink());
        StartCoroutine(makeTextBlink());
        blinking = true;
    }

    void stopBlinking()
    {
        StopCoroutine(makeTextBlink());
        blinking = false;
    }

    IEnumerator makeTextBlink()
    {
        while (true)
        {
            switch (timerText.color.a.ToString())
            {
                case "0":
                    timerText.color = new Color(timerText.color.r, timerText.color.g, timerText.color.b, 1);
                    yield return new WaitForSeconds(1.0f * (GameMaster.timeLeftToCompleteLevel / 15.0f));
                    break;
                case "1":
                    timerText.color = new Color(timerText.color.r, timerText.color.g, timerText.color.b, 0);
                    yield return new WaitForSeconds(1.0f * (GameMaster.timeLeftToCompleteLevel / 15.0f));
                    break;
            }
        }
    }
}
