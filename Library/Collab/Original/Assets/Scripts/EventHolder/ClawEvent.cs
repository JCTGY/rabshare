using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClawEvent : MonoBehaviour
{
    public Sprite openSprite;
    public Sprite closeSprite;
    
    private UnityAction ClawListener;
    private SpringJoint2D hinge;
    private SpriteRenderer clawSprite;

    private bool isClawClosed = false;
    private bool isClawInBlock = false;
    private bool isClawGrabbing = false;

    private Rigidbody2D blocktoGrab;

    void Awake ()
    {
        ClawListener = new UnityAction (claw);
        hinge = GetComponent<SpringJoint2D>();
        hinge.enabled = false;
        clawSprite = GetComponent<SpriteRenderer>();
        clawSprite.sprite = openSprite;
    }

    void OnEnable()
    {
        EventManager.StartListening ("Claw", ClawListener);
    }

    void OnDisable()
    {
        EventManager.StopListening ("Claw", ClawListener);
    }

    void closeClaw()
    {
        clawSprite.sprite = closeSprite;
    }

    void openClaw()
    {
        clawSprite.sprite = openSprite;
    }

    void grabBlock()
    {
        hinge.enabled = true;
        hinge.connectedBody = blocktoGrab;
    }

    void releaseBlock()
    {
        hinge.enabled = false;
        hinge.connectedBody = null;
    }

    void claw()
    {
        if (!isClawClosed)
        {
            closeClaw();
            {
                if (isClawInBlock)
                {
                    grabBlock();
                }
            }
        }
        if (isClawClosed)
        {
            openClaw();
            if (isClawGrabbing)
            {
                releaseBlock();
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        //Debug.Log("ontrigger: " + other.gameObject.name);
        if (other.gameObject.tag == "Block" || other.gameObject.tag == "Bunny")
        {
            isClawInBlock = true;
            blocktoGrab = other.gameObject.GetComponent<Rigidbody2D>(); ;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isClawInBlock = false;
        openClaw();
        releaseBlock();
    }
}
