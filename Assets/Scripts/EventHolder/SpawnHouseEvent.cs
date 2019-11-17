using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnHouseEvent : MonoBehaviour
{
    public GameObject house;
    private GameObject houseClone;

    private UnityAction HouseSpawnListener;
    private bool isHouseDisplayed;

    void Awake()
    {
         HouseSpawnListener = new UnityAction(HouseSpawn);
    }

    private void Start()
    {
        house.SetActive(false);
        isHouseDisplayed = false;
    }

    void OnEnable()
    {
        EventManager.StartListening("HouseSpawn", HouseSpawnListener);
    }

    void OnDisable()
    {
        EventManager.StopListening("HouseSpawn", HouseSpawnListener);
    }
    
    void HouseSpawn()
    {
        if (!isHouseDisplayed)
        {
            houseClone = Instantiate(house, house.transform.position, house.transform.rotation);
            houseClone.SetActive(true);
            houseClone.transform.parent = gameObject.transform;
            Debug.Log("Spawn house clone");
        }
        else
        {
            Destroy(houseClone);
            Debug.Log("Destroy house clone");
        }
        isHouseDisplayed = !isHouseDisplayed;
    }
}
