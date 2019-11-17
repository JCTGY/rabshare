using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour
{
    public Sprite openSprite;
    public Sprite closedSprite;
    public GameObject clawCenter;
    public FixedJoint2D fj;
    SpriteRenderer spriteRenderer;
    bool clawOpen = true;
    bool holdingObject = false;
    string blockLayer;
    GameObject currentBlockHeld;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Magnet") && clawOpen)
        {
            spriteRenderer.sprite = closedSprite;
            clawOpen = false;

            if (holdingObject == false)
            { 
                Collider2D[] hitObjects = Physics2D.OverlapBoxAll(this.GetComponent<Collider2D>().bounds.center, this.GetComponent<Collider2D>().bounds.size, this.transform.rotation.z);

                GameObject block = null;
                foreach (Collider2D hit in hitObjects)
                {
                    float blockDistance = 0.0f;
                    Debug.Log(hit.gameObject.name);
                    if (hit.gameObject.tag == "Block")
                    {
                        if (block == null)
                        {
                            block = hit.gameObject;
                            blockDistance = Vector2.Distance(clawCenter.gameObject.transform.position, hit.gameObject.transform.position);
                        }
                        else if (Vector2.Distance(clawCenter.gameObject.transform.position, hit.gameObject.transform.position) < blockDistance)
                        {
                            block = hit.gameObject;
                            blockDistance = Vector2.Distance(clawCenter.gameObject.transform.position, hit.gameObject.transform.position);
                        }
                    }
                }

                if (block == null)
                    return;

                blockLayer = LayerMask.LayerToName(block.gameObject.layer);
                block.gameObject.layer = LayerMask.NameToLayer("HeldBlock");
                block.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                block.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
                currentBlockHeld = block;

                fj.connectedBody = block.GetComponent<Rigidbody2D>();
                fj.enabled = true;
                fj.autoConfigureConnectedAnchor = false;

                holdingObject = true;
            }
        }
        else if (Input.GetButtonDown("Magnet") && clawOpen == false)
        {
            spriteRenderer.sprite = openSprite;
            clawOpen = true;

            if (holdingObject == true)
            {
                fj.connectedBody = null;
                fj.enabled = false;
                fj.autoConfigureConnectedAnchor = true;
                holdingObject = false;
                currentBlockHeld.layer = LayerMask.NameToLayer(blockLayer);
                currentBlockHeld = null;
            }
        }
    }
}
