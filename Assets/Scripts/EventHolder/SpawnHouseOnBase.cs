using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnHouseOnBase : MonoBehaviour
{
    public GameObject houseBase;
    private GameObject houseClone;

    private UnityAction HouseBaseListener;
    private bool isHouseDisplayed;

    void Awake()
    {
        HouseBaseListener = new UnityAction(HouseBase);
    }

    private void Start()
    {
        //houseBase.SetActive(false);
        isHouseDisplayed = false;
    }

    void OnEnable()
    {
        EventManager.StartListening("HouseOnBase", HouseBaseListener);
    }

    void OnDisable()
    {
        EventManager.StopListening("HouseOnBase", HouseBaseListener);
    }

    void HouseBase()
    {
        if (!isHouseDisplayed)
        {
            houseClone = Instantiate(houseBase, houseBase.transform.position, houseBase.transform.rotation);
            houseClone.SetActive(true);
            houseClone.transform.parent = gameObject.transform;
            foreach (var block in GameObject.FindGameObjectsWithTag("Block"))
            {
                GameMaster.gameBlocks.Add(block);
            }
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
