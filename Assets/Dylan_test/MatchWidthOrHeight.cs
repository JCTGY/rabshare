using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchWidthOrHeight : MonoBehaviour
{
    CanvasScaler scaler;
    // Start is called before the first frame update
    void Start()
    {
        scaler = GetComponent<CanvasScaler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Screen.width < Screen.height)
            scaler.matchWidthOrHeight = 0.0f;
        else
            scaler.matchWidthOrHeight = 1.0f;
    }
}
