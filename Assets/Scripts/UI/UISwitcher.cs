using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISwitcher : MonoBehaviour
{
    bool paused;
    public GameObject HUD;
    public GameObject ControlsUI;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause") && SceneManager.GetActiveScene().name.Contains("Build"))
        {
            if (paused == false)
            {
                paused = true;
                HUD.SetActive(false);
                ControlsUI.SetActive(true);
            }
            else if (paused)
            {
                paused = false;
                HUD.SetActive(true);
                ControlsUI.SetActive(false);
            }
        }
    }
}
