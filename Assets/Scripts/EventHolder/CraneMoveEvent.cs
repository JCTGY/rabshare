using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CraneMoveEvent : MonoBehaviour
{
    public float moveSpeed = 50f;
    public float forceAmount = 100000000.0f;
    private UnityAction craneMoveRightListsener;
    private UnityAction craneMoveLeftListsener;
    private Transform craneTransform;
    private Rigidbody2D craneRB;

    void Awake ()
    {
        craneMoveRightListsener = new UnityAction (craneMoveRight);
        craneMoveLeftListsener = new UnityAction (craneMoveLeft);
        craneTransform = this.GetComponent<Transform>();
        craneRB = this.GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        EventManager.StartListening ("CraneMoveRight", craneMoveRightListsener);
        EventManager.StartListening ("CraneMoveLeft", craneMoveLeftListsener);
    }

    void OnDisable()
    {
        EventManager.StopListening ("CraneMoveRight", craneMoveRightListsener);
        EventManager.StopListening ("CraneMoveLeft", craneMoveLeftListsener);
    }

    void craneMoveRight()
    {
        craneRB.AddForce(Vector2.right * forceAmount, ForceMode2D.Impulse);
        //craneTransform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0, 0));
    }

    void craneMoveLeft()
    {
        craneRB.AddForce(new Vector2(-1, 0) * forceAmount, ForceMode2D.Impulse);
        //craneTransform.Translate(new Vector3(-moveSpeed * Time.deltaTime, 0, 0));
    }
}
