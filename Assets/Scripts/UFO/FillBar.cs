using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillBar : MonoBehaviour
{
    public GameObject fillImage;
    private float fullBarScale;
    private float currentEnergyPercentage = 100.0f;
    private float interpTime = 0.0f;
    private float timeToReachEmpty;
    public bool timerIsOn = false;

    // Start is called before the first frame update
    void Start()
    {
        fullBarScale = fillImage.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsOn)
            emptyBarOverTime();

        setEnergyBar();
    }

    void setEnergyBar()
    {
        Vector3 scale = fillImage.transform.localScale;
        scale.x = (currentEnergyPercentage * fullBarScale) / 100.0f;
        fillImage.transform.localScale = scale;
    }

    public void addEnergy(float energy)
    {
        float tempEnergy = currentEnergyPercentage + energy;
        if (tempEnergy <= 100.0f)
            currentEnergyPercentage = tempEnergy;
    }

    public void depleteEnergy(float energy)
    {
        float tempEnergy = currentEnergyPercentage - energy;
        if (tempEnergy >= 0.0f)
            currentEnergyPercentage = tempEnergy;
    }

    private void emptyBarOverTime()
    {
        interpTime += Time.deltaTime / timeToReachEmpty;
        currentEnergyPercentage = Mathf.Lerp(100.0f, 0.0f, interpTime);
    }

    public bool isEmpty()
    {
        if (Mathf.Approximately(currentEnergyPercentage, 0.0f))
            return (true);
        return (false);
    }

    public void resetTimer()
    {
        timerIsOn = false;
        currentEnergyPercentage = 100.0f;
        interpTime = 0.0f;
    }

    public void setTimer(float totalTime)
    {
        timeToReachEmpty = totalTime;
        timerIsOn = true;
    }
}
