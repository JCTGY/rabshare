using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

/*  EventManager:
        Store all the events, start, remove or trigger a event 
 */

public class EventManager : MonoBehaviour 
{
    private Dictionary <string, UnityEvent> eventDictionary;

    private static EventManager eventManager;

    public static EventManager EMinstance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType (typeof(EventManager)) as EventManager;
                if (!eventManager)
                    Debug.LogError ("There needs to be one active EventManger script on a GameObject in your scene.");
                else
                    eventManager.Init(); 
            }
            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
    }

    public static void StartListening(string eventName, UnityAction listener)
    {

        if (EMinstance.eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            EMinstance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        if (eventManager == null)
            return;

        if (EMinstance.eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        if (EMinstance.eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
        {
            thisEvent.Invoke();
        }
        else
        {
            Debug.Log("Event not found error: " + eventName);
        }
    }
}