using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthQuakeController : MonoBehaviour
{
    public float speedX = 2f;
    public float speedY = 2f;
    public float amplitudeX = 1f;
    public float amplitudeY = 1f;

    private float time = 0f;
    private Rigidbody2D groundRb2d;
    private float fx;
    private float fy;
    //private Vector2 quakeMove;
    private Vector3 quakeMove;
    private Transform groundTransform;
    Material mat;

    // Start is called before the first frame update
    void Awake()
    {
        //groundRb2d = GetComponent<Rigidbody2D>();
        //groundRb2d.bodyType = RigidbodyType2D.Kinematic;
        //groundRb2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        //groundTransform = GetComponent<Transform>();

        groundRb2d = GetComponentInParent<Rigidbody2D>();
        groundRb2d.bodyType = RigidbodyType2D.Kinematic;
        groundRb2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        groundTransform = GetComponentInParent<Transform>();

        // Increasing disaster level
        speedX += speedX * GameMaster.DisasterIncreaseRatio * GameMaster.CurrentDisasterLevel;
        amplitudeX += amplitudeX * GameMaster.DisasterIncreaseRatio * GameMaster.CurrentDisasterLevel;
    }

    private void OnDisable()
    {
        groundRb2d.velocity = new Vector2(0, 0);
    }

    void FixedUpdate()
    {
        fx = amplitudeX * Mathf.Sin(speedX * time);
        fy = amplitudeY * Mathf.Sin(speedY * time);
        time += Time.deltaTime;
        //quakeMove.Set(fx, fy);
        //groundRb2d.AddForce(quakeMove);
        quakeMove.Set(fx, fy, 0);
        //groundTransform.Translate(quakeMove);
        groundRb2d.velocity = quakeMove;
    }
}
