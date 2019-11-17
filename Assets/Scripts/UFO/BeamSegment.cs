using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamSegment : MonoBehaviour
{
    public GameObject destination;
    public GameObject UFO;
    public bool hitPickup = false;
    float speed = 4.2f;
    GameObject objectHit;
    bool fixRotation;
    float rotationCorrectionSpeed = 20.0f;
    float blockCorrectionSpeed = 1.0f;
    Quaternion origRot;
    public float bigBeamSize;
    public float smallBeamSize;
    public float bigBeamWeightLimit;
    public float smallBeamWeightLimit;
    private float curWeightLimit;

    enum BeamSize {Small, Large};
    BeamSize beamSize;

    public void Init()
    {
        if (Mathf.Approximately(this.transform.localScale.x, bigBeamSize))
            beamSize = BeamSize.Large;
        else if (Mathf.Approximately(this.transform.localScale.x, smallBeamSize))
            beamSize = BeamSize.Small;

        if (beamSize == BeamSize.Large)
            curWeightLimit = bigBeamWeightLimit;
        else if (beamSize == BeamSize.Small)
            curWeightLimit = smallBeamWeightLimit;
    }

    private void Update()
    {
        if (hitPickup && objectHit != null)
        {
            float step = speed * Time.deltaTime;
            objectHit.transform.position = Vector2.MoveTowards(objectHit.transform.position, destination.gameObject.transform.position, step);
        }

        if (fixRotation)
            correctBlockRotation();

        if (TractorBeam.holdingSomething == false && objectHit != null && objectHit.GetComponent<Rigidbody2D>().IsTouching(destination.GetComponentInChildren<Collider2D>()))
        {
            Debug.Log("held");
            hitPickup = false;
            objectHit.transform.SetParent(UFO.transform);
            TractorBeam.origPickupLayer = objectHit.layer;
            objectHit.layer = LayerMask.NameToLayer("HeldBlock");
            if (objectHit.tag == "Bunny") objectHit.GetComponent<BunnyController>().bunnyIsGrabbed = true;
            TractorBeam.origPickupTag = objectHit.tag;
            objectHit.tag = "Held";
            TractorBeam.heldBlock = objectHit;
            TractorBeam.holdingSomething = true;
            fixRotation = true;

            //TractorBeam.readyToDestroyBeam = true;
        }

        //if (Input.GetButtonDown("GrowBeam"))
        //{
        //    Vector3 scale = this.transform.localScale;
        //    scale.x = bigBeamSize;
        //    this.transform.localScale = scale;
        //    beamSize = BeamSize.Large;
        //}

        //if (Input.GetButtonDown("ShrinkBeam"))
        //{
        //    Vector3 scale = this.transform.localScale;
        //    scale.x = smallBeamSize;
        //    this.transform.localScale = scale;
        //    beamSize = BeamSize.Small;
        //}

        //if (beamSize == BeamSize.Large)
        //    curWeightLimit = bigBeamWeightLimit;
        //else if (beamSize == BeamSize.Small)
        //    curWeightLimit = smallBeamWeightLimit;
    }

    private void FixedUpdate()
    {
        correctYAxis();
        checkForPickups();
    }

    private void checkForPickups()
    {
        if (hitPickup == false && TractorBeam.holdingSomething == false)
        {
 
            Collider2D[] hitBlocks = Physics2D.OverlapBoxAll(this.GetComponent<Collider2D>().bounds.center, this.GetComponent<SpriteRenderer>().bounds.size, this.transform.rotation.z);

            if (hitBlocks.Length == 0)
                return;

            float largestBlockHeight = float.MinValue;
            foreach (Collider2D block in hitBlocks)
            {
                if ((block.gameObject.tag == "Block" || block.gameObject.tag == "Bunny" || block.gameObject.tag == "Weight") && (block.gameObject.layer == LayerMask.NameToLayer("Pickup") || block.gameObject.layer == LayerMask.NameToLayer("Bunny")))
                {
                    if (block.gameObject.GetComponent<Rigidbody2D>().mass <= curWeightLimit && block.gameObject.transform.localPosition.y > largestBlockHeight)
                    {
                        objectHit = block.gameObject;
                        largestBlockHeight = block.gameObject.transform.position.y;
                    }
                }
            }

            if (objectHit == null)
                return;

            hitPickup = true;

            //objectHit = collision.gameObject;
            Rigidbody2D rb = objectHit.GetComponent<Rigidbody2D>();
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0.0f;
            origRot = objectHit.gameObject.transform.localRotation;
            fixRotation = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if ((collision.gameObject.tag == "Block" || collision.gameObject.tag == "Bunny" || collision.gameObject.tag == "Weight") && collision.gameObject.layer == LayerMask.NameToLayer("Pickup"))
        //{
        //    if (hitPickup == false && TractorBeam.holdingSomething == false)
        //    {
        //        hitPickup = true;
        //        Collider2D[] hitBlocks = Physics2D.OverlapBoxAll(this.transform.position, this.GetComponent<BoxCollider2D>().size, this.transform.rotation.eulerAngles.z);
        //        Debug.Log("Hitblocks Count: " + hitBlocks.Length);

        //        float largestBlockHeight = float.MinValue;
        //        foreach (Collider2D block in hitBlocks)
        //        {
        //            if ((block.gameObject.tag == "Block" || block.gameObject.tag == "Bunny" || block.gameObject.tag == "Weight") && block.gameObject.layer == LayerMask.NameToLayer("Pickup"))
        //            {
        //                Debug.Log("do we get here");
        //                if (block.gameObject.transform.localPosition.y > largestBlockHeight)
        //                {
        //                    Debug.Log("do we get here");
        //                    objectHit = block.gameObject;
        //                    largestBlockHeight = block.gameObject.transform.position.y;
        //                }
        //            }
        //        }

        //        Debug.Log("Picking up " + objectHit.name);

        //        //objectHit = collision.gameObject;
        //        Rigidbody2D rb = objectHit.GetComponent<Rigidbody2D>();
        //        rb.isKinematic = true;
        //        rb.velocity = Vector2.zero;
        //        rb.angularVelocity = 0.0f;
        //        origRot = objectHit.gameObject.transform.localRotation;
        //        fixRotation = true;
        //    }
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == objectHit && TractorBeam.holdingSomething == false)
        {
            hitPickup = false;
            //fixRotation = false;
            collision.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        }
    }

    private void correctBlockRotation()
    {
        if (objectHit.name.Contains("triangle"))
            return;

        float step = rotationCorrectionSpeed * Time.deltaTime;
        Quaternion rot = objectHit.transform.localRotation;
        Vector3 angles = rot.eulerAngles;
        Vector3 UFOangles = UFO.transform.localRotation.eulerAngles;
        if (angles.z > 45.0f && angles.z < 135.0f)
            rot = Quaternion.RotateTowards(rot, Quaternion.Euler(0, 0, 90), step);
        else if (angles.z >= 135.0f && angles.z < 225.0f)
            rot = Quaternion.RotateTowards(rot, Quaternion.Euler(0, 0, 180), step);
        else if (angles.z >= 225.0f && angles.z < 315.0f)
            rot = Quaternion.RotateTowards(rot, Quaternion.Euler(0, 0, 270), step);
        else
            rot = Quaternion.RotateTowards(rot, Quaternion.Euler(0, 0, 0), step);
        objectHit.transform.localRotation = rot;
        Vector3 curRotAngle = rot.eulerAngles;
        if (Mathf.Approximately(curRotAngle.z, 0.0f) || Mathf.Approximately(curRotAngle.z, 90.0f) || Mathf.Approximately(curRotAngle.z, 180.0f) || Mathf.Approximately(curRotAngle.z, 270.0f))
        {
            Rigidbody2D blockPhysics = objectHit.GetComponent<Rigidbody2D>();
            blockPhysics.angularVelocity = 0;
            fixRotation = false;
        }
    }

    private void correctYAxis()
    {
        if (TractorBeam.holdingSomething)
        {
            float step = blockCorrectionSpeed * Time.deltaTime;

            if (objectHit.GetComponent<Collider2D>().IsTouching(UFO.GetComponentInChildren<BoxCollider2D>()))
            {
                objectHit.transform.localPosition += Vector3.down * step;
                objectHit.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                objectHit.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
                //UFO.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                //UFO.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
            }

            if (objectHit.GetComponent<Collider2D>().IsTouching(destination.GetComponent<BoxCollider2D>()) == false)
            {
                //Vector3 pos = objectHit.transform.localPosition;
                //pos.y -= step;
                objectHit.transform.localPosition = Vector2.MoveTowards(objectHit.transform.localPosition, destination.transform.localPosition, step);
                objectHit.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                objectHit.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
            }
        }
    }
}
