using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public float maxVelocity = 10.0f;
    public float maxAngularVelocity = 30.0f;

    public static bool magnetHitSomething = false;

    private Collision2D lastObjectHit;

    private void FixedUpdate()
    {
        StartCoroutine(resetMaxDistance());
        //clampVelocity();
    }

    //currently not used. but gets magnet velocity and make sure it doesn't exceed maxVelocity.
    //angular velocity refers to speed of rotation.
    void clampVelocity()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);

        if (rb.angularVelocity < -maxAngularVelocity)
            rb.angularVelocity = -maxAngularVelocity;

        if (rb.angularVelocity > maxAngularVelocity)
            rb.angularVelocity = maxAngularVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        magnetHitSomething = true;
        lastObjectHit = collision;
    }

    //used to set max distance in rope distance joints.
    //if magnet is hitting something. the rope will not expand any more.
    IEnumerator resetMaxDistance()
    {
        yield return null;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (lastObjectHit != null && lastObjectHit.collider != null && (rb.IsTouching(lastObjectHit.collider) == false || lastObjectHit.gameObject.tag == "Held"))
            magnetHitSomething = false;
    }
}