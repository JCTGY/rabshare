using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cement : MonoBehaviour
{
    FixedJoint2D fixedJoint1;
    FixedJoint2D fixedJoint2;
    public GameObject particleParent;
    public int blobCount = 0;
    public GameObject bottomBlock;
    private FixedJoint2D fj;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.zero); // this activates
        StartCoroutine(destroyUnstuckGlue(1.0f));
    }

    private void FixedUpdate()
    {
        if (fj == null && bottomBlock != null)
        {
            fj = gameObject.AddComponent<FixedJoint2D>();
            fj.connectedBody = bottomBlock.GetComponent<Rigidbody2D>();
            fj.autoConfigureConnectedAnchor = false;
            fj.enableCollision = false;
            this.gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Block")
        {
            if (bottomBlock == null)
                bottomBlock = collision.collider.gameObject;
        }
    }

    IEnumerator destroyUnstuckGlue(float destroyTime)
    {
        // wait one second to allow glue a chance to stick.
        yield return new WaitForSeconds(1.0f);

        if (bottomBlock == null)
        {
            Vector3 originalScale = this.gameObject.transform.localScale;
            Vector3 destinationScale = Vector3.zero;
            float currentTime = 0.0f;
            do
            {
                this.gameObject.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / destroyTime);
                currentTime += Time.deltaTime;
                yield return null;
            } while (currentTime <= destroyTime);
            
            Destroy(this.gameObject);
        }
    }

    private void OnJointBreak2D(Joint2D joint)
    {
        this.gameObject.GetComponent<PolygonCollider2D>().isTrigger = false;
    }
}
