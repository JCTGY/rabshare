using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CartMover : MonoBehaviour
{
    public float forceAmount = 100.0f;
    Rigidbody2D cartRB;
    UnityAction cartMoveRightListener;
    UnityAction cartMoveLeftListener;

    [HideInInspector]
    public bool clawCollidingWithBaseOnLeft;
    [HideInInspector]
    public bool clawCollidingWithBaseOnRight;

    private void Awake()
    {
        cartRB = this.GetComponent<Rigidbody2D>();
        cartMoveRightListener = new UnityAction(cartMoveRight);
        cartMoveLeftListener = new UnityAction(cartMoveLeft);
    }

    private void OnEnable()
    {
        EventManager.StartListening("CartMoveRight", cartMoveRightListener);
        EventManager.StartListening("CartMoveLeft", cartMoveLeftListener);
    }

    private void OnDisable()
    {
        EventManager.StopListening("CartMoveRight", cartMoveRightListener);
        EventManager.StopListening("CartMoveLeft", cartMoveLeftListener);
    }

    //old movement code (doesn't use event manager)
    //private void FixedUpdate()
    //{
    //    if (Input.GetAxis("Horizontal2") > 0.0f)
    //        cartRB.AddForce(Vector2.right * forceAmount, ForceMode2D.Impulse);

    //    if (Input.GetAxis("Horizontal2") < 0.0f)
    //        cartRB.AddForce(Vector2.left * forceAmount, ForceMode2D.Impulse);
    //}

    void cartMoveRight()
    {
        if (clawCollidingWithBaseOnLeft == false)
            cartRB.AddForce(Vector2.right * forceAmount, ForceMode2D.Impulse);
    }

    void cartMoveLeft()
    {
        if (clawCollidingWithBaseOnRight == false)
            cartRB.AddForce(Vector2.left * forceAmount, ForceMode2D.Impulse);
    }
}
