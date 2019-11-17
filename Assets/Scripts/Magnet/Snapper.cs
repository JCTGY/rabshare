using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Snapper : MonoBehaviour
{
    public GameObject magnet;
    public GameObject holdPos;
    public BoxCollider2D rectangleCollider;
    public SpriteRenderer rectangleSprite;
    public AreaEffector2D heavySuction;
    public AreaEffector2D leftSuction;
    public AreaEffector2D rightSuction;
    public AreaEffector2D leftAngleSuction;
    public AreaEffector2D rightAngleSuction;
    private Quaternion origRot;
    private GameObject heldBlock;
    private GameObject heldBlockFakeBoxCollider;
    public static bool magnetOn = false;
    public static bool haveBlock = false;
    bool fixRotation = false;

    private float blockCorrectionSpeed = 5.0f;
    private float rotationCorrectionSpeed = 35.0f;

    private string heldObjectTag;

    // Update is called once per frame
    void Update()
    {
        //non event movement
        /*if (Input.GetButtonDown("Magnet"))
        {
            if (magnetOn == false)
                turnOnMagnet();
            else
                turnOffMagnet();
        }*/

        if (fixRotation == true)
            correctBlockRotation();

        if (heldBlock == false && heldBlockFakeBoxCollider != null)
            Destroy(heldBlockFakeBoxCollider);
    }

    private void FixedUpdate()
    {
        correctYAxis();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the collided object should be picked up, parent it to magnet and set physics settings accordingly.
        if ((collision.gameObject.tag == "Block" || collision.gameObject.tag == "Weight" || collision.gameObject.tag == "Bunny") && haveBlock == false && magnetOn == true)
        {
            heldBlock = collision.gameObject;

            if (heldBlock.tag == "Bunny")
                heldBlock.GetComponent<BunnyController>().bunnyIsGrabbed = true;

            Rigidbody2D blockPhysics = heldBlock.GetComponent<Rigidbody2D>();
            blockPhysics.drag = 3;
            blockPhysics.angularDrag = 20;

            haveBlock = true;

            setFakeFixedJoint(collision);
        }
    }

    //parents collided object to magnet. sets heldBlock to collided object and changes it to kinematic object. turns off suction effectors. 
    void setFakeFixedJoint(Collider2D collision)
    {
        //yield return new WaitForSeconds(0.03f);

        //magnet.GetComponent<Rigidbody2D>().mass /= 2.0f;

        origRot = heldBlock.transform.localRotation;

        heldBlock.transform.parent = magnet.transform;

        fixRotation = true;

        Rigidbody2D blockPhysics = heldBlock.GetComponent<Rigidbody2D>();
        blockPhysics.isKinematic = true;
        blockPhysics.velocity = Vector2.zero;
        blockPhysics.angularVelocity = 0.0f;
        //blockPhysics.mass = 1.0f;
        if (collision.gameObject.tag != "Held")
            heldObjectTag = collision.gameObject.tag;

        collision.gameObject.tag = "Held";
        collision.gameObject.layer = 12;

        expandMagnetBoxCollider();

        heavySuction.enabled = false;
        leftSuction.enabled = false;
        rightSuction.enabled = false;
        leftAngleSuction.enabled = false;
        rightAngleSuction.enabled = false;

        magnet.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        magnet.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
    }

    //creates a dummy box collider so that the now kinematic heldBlock will still collide with ground and other objects 
    private void expandMagnetBoxCollider()
    {
        heldBlockFakeBoxCollider = new GameObject("blockCollider");
        heldBlockFakeBoxCollider.tag = "Held";
        heldBlockFakeBoxCollider.layer = LayerMask.NameToLayer("HeldBlock");
        heldBlockFakeBoxCollider.transform.SetParent(magnet.transform);
        heldBlockFakeBoxCollider.transform.localPosition = heldBlock.transform.localPosition;
        heldBlockFakeBoxCollider.transform.localRotation = heldBlock.transform.localRotation;
        heldBlockFakeBoxCollider.transform.localScale = heldBlock.transform.localScale;
        Collider2D _collider = heldBlock.GetComponent<Collider2D>();
        if (_collider is BoxCollider2D)
        {
            BoxCollider2D box = heldBlockFakeBoxCollider.gameObject.AddComponent<BoxCollider2D>();
            Vector2 boxSize = new Vector2(heldBlock.gameObject.GetComponent<BoxCollider2D>().size.x, heldBlock.gameObject.GetComponent<BoxCollider2D>().size.y);
            box.size = boxSize;
        }
        else if (_collider is CapsuleCollider2D)
        {
            CapsuleCollider2D box = heldBlockFakeBoxCollider.gameObject.AddComponent<CapsuleCollider2D>();
            Vector2 boxSize = new Vector2(heldBlock.gameObject.GetComponent<CapsuleCollider2D>().size.x, heldBlock.gameObject.GetComponent<CapsuleCollider2D>().size.y);
            box.size = boxSize;
        }
    
    }

    //checks if block's rotation is closer to laying vertically or horizontally, and then corrects the rotation to whichever is closer
    //often times the heldBlock parents to the magnet too early or too late and ends up being crooked. this fixes that.
    private void correctBlockRotation()
    {
        if (heldBlock.name.Contains("triangle"))
            return;

        float step = rotationCorrectionSpeed * Time.deltaTime;
        Quaternion rot = heldBlock.transform.localRotation;
        Vector3 angles = origRot.eulerAngles;
        if (angles.z > 45.0f && angles.z < 135.0f)
            rot = Quaternion.RotateTowards(rot, Quaternion.Euler(0, 0, 90), step);
        else if (angles.z >= 135.0f && angles.z < 225.0f)
            rot = Quaternion.RotateTowards(rot, Quaternion.Euler(0, 0, 180), step);
        else if (angles.z >= 225.0f && angles.z < 315.0f)
            rot = Quaternion.RotateTowards(rot, Quaternion.Euler(0, 0, 270), step);
        else
            rot = Quaternion.RotateTowards(rot, Quaternion.identity, step);
        heldBlock.transform.localRotation = rot;
        if (Mathf.Approximately(heldBlock.transform.localRotation.z, 0.0f) || Mathf.Approximately(heldBlock.transform.localRotation.z, 90.0f) || Mathf.Approximately(heldBlock.transform.localRotation.z, 180.0f) || Mathf.Approximately(heldBlock.transform.localRotation.z, 270.0f))
        {
            Rigidbody2D blockPhysics = heldBlock.GetComponent<Rigidbody2D>();
            blockPhysics.angularVelocity = 0;
            fixRotation = false;
        }
        heldBlockFakeBoxCollider.transform.localRotation = heldBlock.transform.localRotation;
    }

    //moves the block vertically towards the magnet if it was parented too early and appears to be far away from the magnet.
    private void correctYAxis()
    {
        if (haveBlock)
        {
            float step = blockCorrectionSpeed * Time.deltaTime;

            if (holdPos.GetComponent<BoxCollider2D>().IsTouching(heldBlock.GetComponent<Collider2D>()) == false)
            {
                heldBlock.transform.localPosition = Vector2.MoveTowards(heldBlock.transform.localPosition, holdPos.transform.localPosition, step);
                heldBlock.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                heldBlock.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
                magnet.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                magnet.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
            }

            if (heldBlock.transform.localPosition.y > holdPos.transform.localPosition.y)
            {
                Vector3 pos = heldBlock.transform.localPosition;
                pos.y -= step;
                heldBlock.transform.localPosition = pos;
            }
        }
    }

    //enables suction effectors. makes magnet body green.
    public void turnOnMagnet()
    {
        magnetOn = true;
        rectangleSprite.color = new Color(0, 255, 0);
        heavySuction.enabled = true;
        leftSuction.enabled = true;
        rightSuction.enabled = true;
        leftAngleSuction.enabled = true;
        rightAngleSuction.enabled = true;
    }

    //disables suction effectors. makes magnet body red. if a block is held, it's dropped and reverted back to its original status
    public void turnOffMagnet()
    {
        magnetOn = false;
        rectangleSprite.color = new Color(255, 0, 0);
        heavySuction.enabled = false;
        leftSuction.enabled = false;
        rightSuction.enabled = false;
        leftAngleSuction.enabled = false;
        rightAngleSuction.enabled = false;
        fixRotation = false;
        if (haveBlock)
        {
            haveBlock = false;
            heldBlock.gameObject.tag = heldObjectTag;
            heldBlock.layer = 10;
            int bunnyLayer = 19;
            if (heldObjectTag == "Bunny")
            {
                heldBlock.layer = bunnyLayer;
                heldBlock.GetComponent<BunnyController>().bunnyIsDropped = true;
            }

            Rigidbody2D blockPhysics = heldBlock.GetComponent<Rigidbody2D>();
            blockPhysics.drag = 0.1f;
            blockPhysics.angularDrag = 1;
            blockPhysics.isKinematic = false;
            addMomentumToDroppedBlock();
            heldBlock.transform.parent = null;

            //Don't destroy get's overwritten when parented. Resetting the component fixes that.
            heldBlock.GetComponent<DontDestroy>().enabled = false;
            heldBlock.GetComponent<DontDestroy>().enabled = true;
            heldBlock = null;
            Destroy(heldBlockFakeBoxCollider);
        }
    }

    //while held, a block loses velocity. this gives the block roughly what momentum it should have on release
    private void addMomentumToDroppedBlock()
    {
        Rigidbody2D blockPhysics = heldBlock.GetComponent<Rigidbody2D>();
        blockPhysics.velocity = magnet.GetComponent<Rigidbody2D>().velocity;
        blockPhysics.angularVelocity = magnet.GetComponent<Rigidbody2D>().angularVelocity;
        if (heldBlock.GetComponent<FixedJoint2D>() != null)
        {
            heldBlock.GetComponent<FixedJoint2D>().connectedBody.velocity = magnet.GetComponent<Rigidbody2D>().velocity;
            heldBlock.GetComponent<FixedJoint2D>().connectedBody.angularVelocity = magnet.GetComponent<Rigidbody2D>().angularVelocity;
        }
    }

    // By Juo: create event
    private UnityAction SnapperListener;

    void Awake()
    {
        SnapperListener = new UnityAction(Use);
    }

    void OnEnable()
    {
        EventManager.StartListening("Use", SnapperListener);
    }

    void OnDisable()
    {
        EventManager.StopListening("Use", SnapperListener);
    }

    private void Use()
    {
        if (magnetOn == false)
            turnOnMagnet();
        else
            turnOffMagnet();
    }
}
