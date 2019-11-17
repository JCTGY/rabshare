using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HailController : MonoBehaviour
{
    public GameObject hailPrefab;
    public float spawnAreaWidth = 5f;
    public float hailRate = 2f;
    public int hailDense = 10;

    private float timer;
    private int dense;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        dense = Random.Range(1, hailDense);
        // Increasing disaster level
        hailRate += hailRate * GameMaster.DisasterIncreaseRatio * GameMaster.CurrentDisasterLevel;
        hailDense += (int)(hailDense * GameMaster.DisasterIncreaseRatio * GameMaster.CurrentDisasterLevel);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1 / hailRate)
        {
            for (int i = 0; i < dense; i++)
            {
                GameObject comet = Instantiate(hailPrefab, GetRandomPosition(), this.transform.rotation);
                //ShootingStar starScript = comet.GetComponent<ShootingStar>();
                //starScript.randomize = false;
                //starScript.spawnTime = 0.0f;
            }

            timer = 0;
        }
    }

    private Vector3 GetRandomPosition()
    {
        float dx = UnityEngine.Random.Range(0f, spawnAreaWidth);
        Vector3 position = this.transform.position;
        position.x += dx;

        return position;
    }
}
