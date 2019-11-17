using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the electric fence destroys all that collides with it
//...to provide the playable bounds of the game
public class Fance : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Old Fence code
        //if (collision.collider.gameObject.layer != LayerMask.NameToLayer("Crane") &&
        //    collision.collider.gameObject.layer != LayerMask.NameToLayer("Magnet"))
        //    Destroy(collision.gameObject);

        if (collision.collider.tag == "Bunny" || collision.collider.tag == "Block")
            Destroy(collision.gameObject);
    }
}
