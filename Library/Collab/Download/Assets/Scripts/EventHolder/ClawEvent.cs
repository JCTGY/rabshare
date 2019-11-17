using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClawEvent : MonoBehaviour
{
    
    public Sprite openSprite;
    public Sprite closeSprite;
    
    private UnityAction ClawListener;
    private HingeJoint2D hinge;
    private bool isClawed = false;
    private SpriteRenderer clawSprite;

    void Awake ()
    {
        ClawListener = new UnityAction (claw);
        hinge = GetComponent<HingeJoint2D>();        
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

    void claw()
    {
        isClawed = !isClawed;
        if (isClawed == true)
        {
            clawSprite.sprite = closeSprite;
        }
        else
        {
            hinge.enabled = false;
            hinge.connectedBody = null;
            clawSprite.sprite = openSprite;
        }
    }
    
    private void OnTriggerStay2D(Collider2D other) 
    {
        //Debug.Log("ontrigger: " + other.gameObject.name);
        if (other.gameObject.tag == "Block" || other.gameObject.tag == "Bunny")
        {
            if (isClawed == true)
            {
                hinge.enabled = true;
                hinge.connectedBody = other.gameObject.GetComponent<Rigidbody2D>();
            }  
        }
    }
}
