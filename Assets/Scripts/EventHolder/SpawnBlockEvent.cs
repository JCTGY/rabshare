using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SpawnBlockEvent : MonoBehaviour
{
    public GameObject squarePrefab;
	public GameObject rectanglePrefab;
	public GameObject trianglePrefab;

    public GameObject UFOsquarePrefab;
    public GameObject UFOrectanglePrefab;
    public GameObject UFOtrianglePrefab;

    public float spawnAreaWidth = 8.0f;
    public float spawnAreaHeight = 5.0f;

    private UnityAction SquareSpawnListener;
	private UnityAction RectangleSpawnListener;
	private UnityAction TriangleSpawnListener;

    int totalBlocks = 0;
    float blockDistance = 0.6f;

	void Awake()
	{
		SquareSpawnListener = new UnityAction(SquareSpawn);
		RectangleSpawnListener = new UnityAction(RectangleSpawn);
		TriangleSpawnListener = new UnityAction(TriangleSpawn);
	}

	void OnEnable()
	{
		EventManager.StartListening("SquareSpawn", SquareSpawnListener);
		EventManager.StartListening("RectangleSpawn", RectangleSpawnListener);
		EventManager.StartListening("TriangleSpawn", TriangleSpawnListener);
	}

	void OnDisable()
	{
		EventManager.StopListening("SquareSpawn", SquareSpawnListener);
		EventManager.StopListening("RectangleSpawn", RectangleSpawnListener);
		EventManager.StopListening("TriangleSpawn", TriangleSpawnListener);
	}

	private Vector3 getRandomPosition()
	{
		float dx = UnityEngine.Random.Range(0f, spawnAreaWidth);
		//float dy = UnityEngine.Random.Range(0f, spawnAreaHeight);
		Vector3 position = this.transform.position;
		position.x += dx;
		//position.y += dy;
		return position;
	}
    
	void SquareSpawn() // Spawn(prefab spawnPrefab)
	{
		if (squarePrefab != null || UFOsquarePrefab != null)
		{
            Vector3 blockPos = adjustPositionUpwards();
            if (SceneManager.GetActiveScene().name.Contains("UFO"))
                GameMaster.gameBlocks.Add(Instantiate(UFOsquarePrefab, blockPos, this.transform.rotation));
            else
                GameMaster.gameBlocks.Add(Instantiate(squarePrefab, blockPos, this.transform.rotation));
            //Instantiate(squarePrefab, getRandomPosition(), this.transform.rotation);
		}
		else
			Debug.Log("Lost Prefab in " + this.gameObject.name);
	}

	void RectangleSpawn() // Spawn(prefab spawnPrefab)
	{
        //Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + 90.0f

        if (rectanglePrefab != null || UFOrectanglePrefab != null)
		{
            Vector3 blockPos = adjustPositionUpwards();
            if (SceneManager.GetActiveScene().name.Contains("UFO"))
                GameMaster.gameBlocks.Add(Instantiate(UFOrectanglePrefab, blockPos, this.transform.rotation));
            else
                GameMaster.gameBlocks.Add(Instantiate(rectanglePrefab, blockPos, this.transform.rotation));

            //Instantiate(rectanglePrefab, getRandomPosition(), this.transform.rotation);
            Debug.Log("Spawn" + rectanglePrefab.name);
		}
		else
			Debug.Log("Lost Prefab in " + this.gameObject.name);
	}

	void TriangleSpawn() // Spawn(prefab spawnPrefab)
	{
		if (trianglePrefab != null || UFOtrianglePrefab != null)
		{
            Vector3 blockPos = adjustPositionUpwards();
            if (SceneManager.GetActiveScene().name.Contains("UFO"))
                GameMaster.gameBlocks.Add(Instantiate(UFOtrianglePrefab, blockPos, this.transform.rotation));
            else
                GameMaster.gameBlocks.Add(Instantiate(trianglePrefab, blockPos, this.transform.rotation));
            //Instantiate(trianglePrefab, getRandomPosition(), this.transform.rotation);
            Debug.Log("Spawn" + trianglePrefab.name);
		}
		else
			Debug.Log("Lost Prefab in " + this.gameObject.name);
	}

    Vector3 adjustPositionUpwards()
    {
        totalBlocks++;
        Vector3 blockPos = getRandomPosition();
        blockPos.y += blockDistance * totalBlocks;
        return blockPos;
    }
}
