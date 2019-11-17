using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SpawnBunnyEvent : MonoBehaviour
{
    public GameObject bunnyPrefab;

    public float spawnAreaWidth = 8.0f;

    private UnityAction BunnySpawnListener;

    // blockPrefab
    // blockListener

    void Awake()
    {
        BunnySpawnListener = new UnityAction(BunnySpawn);
        // new bunnyListener
    }

    void OnEnable()
    {
        EventManager.StartListening("BunnySpawn", BunnySpawnListener);
        // start
    }

    void OnDisable()
    {
        EventManager.StopListening("BunnySpawn", BunnySpawnListener);
        // stop
    }

    private Vector3 getRandomPosition()
    {
        float dx = UnityEngine.Random.Range(0f, spawnAreaWidth);
        Vector3 position = this.transform.position;
        position.x += dx;
        return position;
    }

    void BunnySpawn() // Spawn(prefab spawnPrefab)
    {
        if (bunnyPrefab != null)
        {
            GameMaster.gameBlocks.Add(Instantiate(bunnyPrefab, getRandomPosition(), this.transform.rotation));
            Debug.Log("Spawn" + bunnyPrefab.name);
        }
        else
            Debug.Log("Lost Prefab in " + this.gameObject.name);
    }
}
