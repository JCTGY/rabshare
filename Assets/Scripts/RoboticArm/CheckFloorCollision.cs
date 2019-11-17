using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFloorCollision : MonoBehaviour
{
    RobotArmMover moveScript;
    public GameObject floor;
    Collider2D clawProngCollider;
    bool hitFloor;
    bool currentlyColliding;

    // Start is called before the first frame update
    void Start()
    {
        moveScript = GetComponentInParent<RobotArmMover>();
        clawProngCollider = GetComponent<Collider2D>();
    }

    //private void Update()
    //{
    //    Collider2D[] objectsTouchingClawProng = Physics2D.OverlapBoxAll(clawProngCollider.bounds.center, clawProngCollider.bounds.size, clawProngCollider.transform.localRotation.z);

    //    foreach (Collider2D obj in objectsTouchingClawProng)
    //    {
    //        //push the claw up if it's touching the floor and disable regular movement
    //        //TODO the pushing can look a bit jarring. it would probably be better to get the clamping code that's commented to actually work.
    //        if (obj.gameObject == floor)
    //        {
    //            //Debug.Log("Hit Floor");
    //            //if (currentlyColliding == false && moveScript.clawHitFloor == false)
    //            //{
    //            //    moveScript.clawHitFloor = true;
    //            //    moveScript.clawYClamp = moveScript.IKComponent.transform.position.y;
    //            //    currentlyColliding = true;
    //            //}
    //            //else if (moveScript.IKComponent.transform.position.y < moveScript.clawYClamp)
    //            //    moveScript.clawYClamp = moveScript.IKComponent.transform.position.y;
    //            //hitFloor = true;
    //            moveScript.IKComponent.transform.position += Vector3.up * Time.deltaTime;
    //            moveScript.clawHitFloor = true;
    //            hitFloor = true;
    //            break;
    //        }
    //    }

    //    if (hitFloor == false && currentlyColliding == true)
    //    {
    //        moveScript.clawHitFloor = false;
    //        currentlyColliding = false;
    //    }

    //    if (hitFloor == false)
    //        moveScript.clawHitFloor = false;

    //    hitFloor = false;
    //}

    private void Update()
    {
        Debug.Log("Claw hit floor = " + moveScript.clawHitFloor);
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == floor)
        {
            moveScript.clawHitFloor = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == floor)
        {
            moveScript.clawHitFloor = false;
        }
    }
}
