using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CanvasEvent : MonoBehaviour
{
    private UnityAction DisableDisasterbuttonListsener;

    void Awake()
    {
        DisableDisasterbuttonListsener = new UnityAction(DisableDisasterbutton);
    }

    void OnEnable()
    {
        EventManager.StartListening("DisableDisasterbutton", DisableDisasterbuttonListsener);
    }

    void OnDisable()
    {
        EventManager.StopListening("DisableDisasterbutton", DisableDisasterbuttonListsener);
    }

    void DisableDisasterbutton()
    {
        GameObject DisasterButton = null;

        // Find DisasterButton
        GameObject[] buttons = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (var button in buttons)
        {
            if (button.name.Equals("DisasterButton"))
            {
                DisasterButton = button;
                break;
            }
        }
        if (DisasterButton != null)
            DisasterButton.SetActive(false);
        else
            Debug.Log("Disaster Button Not Found");
    }
}
