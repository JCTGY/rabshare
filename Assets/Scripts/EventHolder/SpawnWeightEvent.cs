using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnWeightEvent : MonoBehaviour
{
    public GameObject weightPrefab30;
    public GameObject weightPrefab60;
    public GameObject weightPrefab90;

    public float spawnAreaWidth = 8.0f;
    public float spawnAreaHeight = 5.0f;

    private UnityAction Weight30SpawnListener;
    private UnityAction Weight60SpawnListener;
    private UnityAction Weight90SpawnListener;

    // weightPrefab
    // weightListener

    void Awake()
    {
        Weight30SpawnListener = new UnityAction(WeightSpawn30);
        Weight60SpawnListener = new UnityAction(WeightSpawn60);
        Weight90SpawnListener = new UnityAction(WeightSpawn90);
        // new weightListener
    }

    void OnEnable()
    {
        EventManager.StartListening("WeightSpawn30", Weight30SpawnListener);
        EventManager.StartListening("WeightSpawn60", Weight60SpawnListener);
        EventManager.StartListening("WeightSpawn90", Weight90SpawnListener);
        // start
    }

    void OnDisable()
    {
        EventManager.StopListening("WeightSpawn30", Weight30SpawnListener);
        EventManager.StopListening("WeightSpawn60", Weight60SpawnListener);
        EventManager.StopListening("WeightSpawn90", Weight90SpawnListener);
        // stop
    }

    private Vector3 getRandomPosition()
    {
        float dx = UnityEngine.Random.Range(0f, spawnAreaWidth);
        float dy = UnityEngine.Random.Range(0f, spawnAreaHeight);
        Vector3 position = this.transform.position;
        position.x += dx;
        position.y += dy;
        return position;
    }

    void WeightSpawn30() // Spawn(prefab spawnPrefab)
    {
        if (weightPrefab30 != null)
        {
            Instantiate(weightPrefab30, getRandomPosition(), this.transform.rotation);
            Debug.Log("Spawn" + weightPrefab30.name);
        }
        else
            Debug.Log("Lost Prefab in " + this.gameObject.name);
    }

    void WeightSpawn60() // Spawn(prefab spawnPrefab)
    {
        if (weightPrefab60 != null)
        {
            Instantiate(weightPrefab60, getRandomPosition(), this.transform.rotation);
            Debug.Log("Spawn" + weightPrefab60.name);
        }
        else
            Debug.Log("Lost Prefab in " + this.gameObject.name);
    }

    void WeightSpawn90() // Spawn(prefab spawnPrefab)
    {
        if (weightPrefab90 != null)
        {
            Instantiate(weightPrefab90, getRandomPosition(), this.transform.rotation);
            Debug.Log("Spawn" + weightPrefab90.name);
        }
        else
            Debug.Log("Lost Prefab in " + this.gameObject.name);
    }
}