using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCollision : MonoBehaviour
{
    public GameObject cart;
    public Transform leftEdgeCheckPosition;
    public Transform rightEdgeCheckPosition;

    CartMover moveScript;

    private void Start()
    {
        moveScript = cart.GetComponent<CartMover>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("RobotTool"))
        {
            Debug.Log("Base Collision");
            Rigidbody2D rb = cart.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            if (collision.transform.position.x < leftEdgeCheckPosition.transform.position.x)
                moveScript.clawCollidingWithBaseOnLeft = true;
            if (collision.transform.position.x > rightEdgeCheckPosition.transform.position.x)
                moveScript.clawCollidingWithBaseOnRight = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("RobotTool"))
        {
            if (moveScript.clawCollidingWithBaseOnLeft) moveScript.clawCollidingWithBaseOnLeft = false;
            if (moveScript.clawCollidingWithBaseOnRight) moveScript.clawCollidingWithBaseOnRight = false;
        }
    }
}
