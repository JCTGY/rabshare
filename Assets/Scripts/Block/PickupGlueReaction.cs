using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGlueReaction : MonoBehaviour
{
    public float breakForce = 700.0f;
    private FixedJoint2D fj;
    bool addedFixedJoint = false;
    private HashSet<GameObject> adjacentGlue;

    // Start is called before the first frame update
    void Start()
    {
        adjacentGlue = new HashSet<GameObject>();
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Glue" && collision.collider.gameObject.GetComponent<Cement>())
        {
            if (!adjacentGlue.Contains(collision.collider.gameObject))
                adjacentGlue.Add(collision.collider.gameObject);
            GameObject bottomBlock = collision.collider.gameObject.GetComponent<Cement>().bottomBlock;
            if (bottomBlock && addedFixedJoint == false && bottomBlock.GetComponent<Rigidbody2D>() != gameObject.GetComponent<Rigidbody2D>())
            {
                addedFixedJoint = true;
                //StartCoroutine(setGlueScale(1.0f));
                StartCoroutine(setFixedJoint(bottomBlock, collision, 1.0f));
            }
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Glue" && collision.gameObject.GetComponent<Cement>())
        {
            if (!adjacentGlue.Contains(collision.gameObject))
                adjacentGlue.Add(collision.gameObject);
            GameObject bottomBlock = collision.gameObject.GetComponent<Cement>().bottomBlock;
            if (bottomBlock && addedFixedJoint == false && bottomBlock.GetComponent<Rigidbody2D>() != gameObject.GetComponent<Rigidbody2D>())
            {
                addedFixedJoint = true;
                //StartCoroutine(setGlueScale(1.0f));
                StartCoroutine(setFixedJoint(bottomBlock, collision, 1.0f));
            }
        }
    }

    /*private void OnCollisionExit2D(Collision2D collision)
    {
        if (adjacentGlue.Contains(collision.collider.gameObject))
        {
            Debug.Log("OnCollisionExit" + exitCalls);
            exitCalls++;
            adjacentGlue.Remove(collision.collider.gameObject);
            glueList.Remove(collision.collider.gameObject);
        }
    }*/

    //bugged right now. but could be used to make squish effect
    /*IEnumerator setGlueScale(float time)
    {
        yield return new WaitForEndOfFrame();
        for(int i = 0; i < glueList.Count; i++)
        {
            Vector3 originalScale = glueList[i].transform.localScale;
            Vector3 destinationScale = new Vector3(originalScale.x, originalScale.y - 0.1f, originalScale.z);
            float currentTime = 0.0f;
            do
            {
                glueList[i].transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
                currentTime += Time.deltaTime;
                yield return null;
            } while (currentTime <= time);

            Vector2 anchor = glueList[i].GetComponent<FixedJoint2D>().connectedAnchor;
            anchor.y -= 0.01f;
            glueList[i].GetComponent<FixedJoint2D>().connectedAnchor = anchor;
        }
    }*/

    IEnumerator setFixedJoint(GameObject bottomBlock, Collider2D collision, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        if (this.gameObject.GetComponent<Collider2D>().IsTouching(collision.gameObject.GetComponent<Collider2D>()))
        {
            fj = gameObject.AddComponent<FixedJoint2D>();
            fj.connectedBody = bottomBlock.GetComponent<Rigidbody2D>();
            fj.autoConfigureConnectedAnchor = false;
            fj.breakForce = breakForce * adjacentGlue.Count;
            adjacentGlue.Clear();
        }
        else
            addedFixedJoint = false;
    }

    private IEnumerator OnJointBreak2D(Joint2D joint)
    {
        yield return null;
        if (joint == null)
        {
            fj = null;
            addedFixedJoint = false;
        }
    }
}
