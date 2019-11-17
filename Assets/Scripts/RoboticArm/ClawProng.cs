using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawProng : MonoBehaviour
{
    public RobotClaw robotClawScript;
    public RobotClawRotation robotClawRotationScript;
    public string whichSide;

    GameObject blockToGrab;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log("Trigger");
    //    if (collision.gameObject.tag == "Block")
    //    {
    //        fj.connectedBody = collision.gameObject.GetComponent<Rigidbody2D>();
    //        fj.enabled = true;
    //        fj.autoConfigureConnectedAnchor = false;

    //        Rigidbody2D blockRB = collision.gameObject.GetComponent<Rigidbody2D>();
    //        blockRB.velocity = Vector2.zero;
    //        blockRB.angularVelocity = 0.0f;

    //        if (whichSide == "Left")
    //            robotClawScript.leftHitBlock = true;
    //        else if (whichSide == "Right")
    //            robotClawScript.rightHitBlock = true;
    //    }
    //}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (blockToGrab == null)//((whichSide == "Left" && robotClawScript.leftHitBlock == false) || (whichSide == "Right" && robotClawScript.rightHitBlock == false)) && blockToGrab == null)
        {
            if (collision.gameObject.tag == "Block" || collision.gameObject.tag == "Bunny")
            {
                Rigidbody2D blockRB = collision.gameObject.GetComponent<Rigidbody2D>();
                blockRB.velocity = Vector2.zero;
                blockRB.angularVelocity = 0.0f;

                //collision.gameObject.layer = LayerMask.NameToLayer("HeldBlock");
                blockToGrab = collision.gameObject;

                if (robotClawScript != null)
                    robotClawScript.blockTransform = collision.gameObject.transform;
                else if (robotClawRotationScript != null)
                    robotClawRotationScript.blockTransform = collision.gameObject.transform;

                if (robotClawScript != null)
                {
                    if (whichSide == "Left" && prongsIsDeepEnough(robotClawScript.leftProng))
                        robotClawScript.leftHitBlock = true;
                    else if (whichSide == "Right" && prongsIsDeepEnough(robotClawScript.rightProng))
                        robotClawScript.rightHitBlock = true;
                }
                else if (robotClawRotationScript != null)
                {
                    if (whichSide == "Right") Debug.Log(whichSide);
                    if (whichSide == "Left")
                    {
                        //Debug.Log("Left Hit");
                        robotClawRotationScript.leftHitBlock = true;
                    }
                    else if (whichSide == "Right")
                    {
                        //Debug.Log("Right hit");
                        robotClawRotationScript.rightHitBlock = true;
                    }

                }

            }
        }
    }

    bool prongsIsDeepEnough(GameObject prong)
    {
        Vector3 difference = prong.transform.position - blockToGrab.transform.position;

        //if (whichSide == "Right")
        //{
        //    Debug.Log("Y Distance from prong: " + Mathf.Abs(difference.y));
        //    Debug.Log("Y Bounds: " + blockToGrab.gameObject.GetComponent<Collider2D>().bounds.extents.y * 0.85);

        //    Debug.Log("X Distance from prong: " + Mathf.Abs(difference.x));
        //    Debug.Log("X Bounds: " + blockToGrab.gameObject.GetComponent<Collider2D>().bounds.extents.x * 0.85);
        //}


        if (Mathf.Abs(difference.y) < blockToGrab.gameObject.GetComponent<Collider2D>().bounds.extents.y * 0.85f ||
            Mathf.Abs(difference.x) <  blockToGrab.gameObject.GetComponent<Collider2D>().bounds.extents.x * 0.85f)
            return true;

        return false;
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Block")
    //    {
    //        fj.connectedBody = collision.gameObject.GetComponent<Rigidbody2D>();
    //        fj.enabled = true;
    //        fj.autoConfigureConnectedAnchor = false;

    //        Rigidbody2D blockRB = collision.gameObject.GetComponent<Rigidbody2D>();
    //        blockRB.velocity = Vector2.zero;
    //        blockRB.angularVelocity = 0.0f;

    //        if (whichSide == "Left")
    //            robotClawScript.leftHitBlock = true;
    //        else if (whichSide == "Right")
    //            robotClawScript.rightHitBlock = true;
    //    }
    //}

    public void releaseBlock()
    {
        blockToGrab = null;
    }
}
