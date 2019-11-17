using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RobotClawRotation : MonoBehaviour
{
    public GameObject leftProng;
    public GameObject rightProng;

    ClawProng leftProngScript;
    ClawProng rightProngScript;

    public GameObject leftGrabber;
    public GameObject rightGrabber;

    public GameObject leftProngCenter;
    public GameObject rightProngCenter;

    Vector3 origLeftRot;
    Vector3 origRightRot;

    public bool clawOpen = true;
    bool clawClosing = false;
    bool clawOpening = false;

    [HideInInspector]
    public bool leftHitBlock = false;
    [HideInInspector]
    public bool rightHitBlock = false;
    [HideInInspector]
    public Transform blockTransform;

    public static bool holdingBlock = false;

    bool closeLeftClawRoutineRunning;
    bool closeRightClawRoutineRunning;

    public FixedJoint2D clawFJ;

    int oldPickupLayer;

    UnityAction UseClawListener;

    float rightProngAngleConstraint = 334.0f;
    float leftProngAngleConstraint = 26.0f;
    float rotationSpeed = 20.0f;

    private GameObject heldBlockFakeBoxCollider;

    public FloorClamp floorClampScript;

    void Awake()
    {
        UseClawListener = new UnityAction(activateClaw);
    }

    private void OnEnable()
    {
        EventManager.StartListening("Use", UseClawListener);
    }

    private void OnDisable()
    {
        EventManager.StopListening("Use", UseClawListener);
    }

    // Start is called before the first frame update
    void Start()
    {
        origLeftRot = new Vector3(leftProng.transform.rotation.eulerAngles.x, leftProng.transform.rotation.eulerAngles.y, leftProng.transform.rotation.eulerAngles.z);
        origRightRot = new Vector3(rightProng.transform.rotation.eulerAngles.x, rightProng.transform.rotation.eulerAngles.y, rightProng.transform.rotation.eulerAngles.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (clawClosing)
        {
            if (closeLeftClawRoutineRunning == false && closeRightClawRoutineRunning == false)
            {
                clawClosing = false;
                clawOpen = false;

                if (leftHitBlock && rightHitBlock && clawPositionsAreValid())//leftProngIK.transform.position.x < blockTransform.position.x && rightProngIK.transform.position.x > blockTransform.position.x)
                    grabBlock();
                else
                {
                    destroyProngScripts();
                    releaseBlock();
                }


                leftHitBlock = false;
                rightHitBlock = false;
            }
        }

        if (clawOpen == false && clawClosing == false && holdingBlock == false)
            destroyProngScripts();

        //if (Input.GetButtonDown("Magnet") && clawOpen == true && clawClosing == false && clawOpening == false)
        //{
        //    clawClosing = true;
        //    leftProngScript = leftProng.AddComponent<ClawProng>();
        //    leftProngScript.whichSide = "Left";
        //    leftProngScript.robotClawScript = this;
        //    rightProngScript = rightProng.AddComponent<ClawProng>();
        //    rightProngScript.whichSide = "Right";
        //    rightProngScript.robotClawScript = this;
        //    StartCoroutine(closeLeftClaw());
        //    StartCoroutine(closeRightClaw());
        //}
        //else if (Input.GetButtonDown("Magnet") && clawOpen == false && clawClosing == false && clawOpening == false)
        //{
        //    clawOpening = true;

        //    releaseBlock();

        //    destroyProngScripts();

        //    holdingBlock = false;

        //    StartCoroutine(openClaw());
        //}
    }

    void activateClaw()
    {
        if (clawOpen == true && clawClosing == false && clawOpening == false)
            Grab();
        else if (clawOpen == false && clawClosing == false && clawOpening == false)
            Release();
    }

    void Grab()
    {
        clawClosing = true;
        leftProngScript = leftGrabber.AddComponent<ClawProng>();
        leftProngScript.whichSide = "Left";
        leftProngScript.robotClawRotationScript = this;
        rightProngScript = rightGrabber.AddComponent<ClawProng>();
        rightProngScript.whichSide = "Right";
        rightProngScript.robotClawRotationScript = this;
        StartCoroutine(closeLeftClaw());
        StartCoroutine(closeRightClaw());
    }

    void Release()
    {
        clawOpening = true;

        releaseBlock();

        destroyProngScripts();

        holdingBlock = false;

        StartCoroutine(openClaw());
    }

    IEnumerator closeLeftClaw()
    {
        bool clawIsTouching = false;
        closeLeftClawRoutineRunning = true;

        while (clawIsTouching == false)
        {
            float leftProngAngle = leftProng.transform.localRotation.eulerAngles.z;
            if ((leftProngAngle >= leftProngAngleConstraint && leftProngAngle < 45.0f) || leftHitBlock)
                clawIsTouching = true;

            Vector3 rot = leftProng.transform.localRotation.eulerAngles;
            rot.z += rotationSpeed * Time.deltaTime;
            leftProng.transform.localRotation = Quaternion.Euler(rot);

            yield return null;
        }

        closeLeftClawRoutineRunning = false;
    }

    IEnumerator closeRightClaw()
    {
        bool clawIsTouching = false;
        closeRightClawRoutineRunning = true;

        while (clawIsTouching == false)
        {
            //need to check if prongs are touching
            float rightProngAngle = rightProng.transform.localRotation.eulerAngles.z;
            if ((rightProngAngle <= rightProngAngleConstraint && rightProngAngle > 1.0f) || rightHitBlock)
                clawIsTouching = true;

            Vector3 rot = rightProng.transform.localRotation.eulerAngles;
            rot.z -= rotationSpeed * Time.deltaTime;
            rightProng.transform.localRotation = Quaternion.Euler(rot);

            yield return null;
        }

        closeRightClawRoutineRunning = false;
    }

    IEnumerator openClaw()
    {
        bool clawInDefaultPos = false;

        while (clawInDefaultPos == false)
        {
            if (leftProng.transform.localRotation.eulerAngles.z <= 0.0f &&
                rightProng.transform.localRotation.eulerAngles.z >= 0.0f)
                clawInDefaultPos = true;

            Vector3 rot = leftProng.transform.localRotation.eulerAngles;
            rot.z -= rotationSpeed * Time.deltaTime;
            if (rot.z > 45.0f)
                rot.z = 0.0f;
            leftProng.transform.localRotation = Quaternion.Euler(rot);

            rot = rightProng.transform.localRotation.eulerAngles;
            rot.z += rotationSpeed * Time.deltaTime;
            if (rot.z > 0.0f && rot.z < 300.0f)
                rot.z = 0.0f;
            rightProng.transform.localRotation = Quaternion.Euler(rot);

            yield return null;
        }

        clawOpening = false;
        clawOpen = true;
    }

    void destroyProngScripts()
    {
        if (leftProngScript != null)
        {
            leftProngScript = null;
            Destroy(leftGrabber.GetComponent<ClawProng>());
        }

        if (rightProngScript != null)
        {
            rightProngScript = null;
            Destroy(rightGrabber.GetComponent<ClawProng>());
        }
    }

    //check to see that claws are on opposite sides of picked up block. either horizontally or vertically.
    bool clawPositionsAreValid()
    {
        //if (prongsAreParallel() == false)
        //    return false;


        if (Vector3.Dot(leftProngCenter.transform.position - blockTransform.position, blockTransform.right) <= 0 &&
            Vector3.Dot(rightProngCenter.transform.position - blockTransform.position, blockTransform.right) > 0)
            return true;

        if (Vector3.Dot(leftProngCenter.transform.position - blockTransform.position, blockTransform.right) > 0 &&
            Vector3.Dot(rightProngCenter.transform.position - blockTransform.position, blockTransform.right) <= 0)
            return true;

        if (Vector3.Dot(leftProngCenter.transform.position - blockTransform.position, blockTransform.up) <= 0 &&
            Vector3.Dot(rightProngCenter.transform.position - blockTransform.position, blockTransform.up) > 0)
            return true;

        if (Vector3.Dot(leftProngCenter.transform.position - blockTransform.position, blockTransform.up) > 0 &&
            Vector3.Dot(rightProngCenter.transform.position - blockTransform.position, blockTransform.up) <= 0)
            return true;

        return false;
    }

    void grabBlock()
    {
        holdingBlock = true;
        RobotArmAgent.pickedUpBlock = true;
        oldPickupLayer = blockTransform.gameObject.layer;
        blockTransform.gameObject.layer = LayerMask.NameToLayer("HeldBlock");
        blockTransform.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 4;
        blockTransform.position = new Vector3(blockTransform.position.x, blockTransform.position.y, -2);

        blockTransform.SetParent(this.transform);
        blockTransform.GetComponent<Rigidbody2D>().isKinematic = true;

        //clawFJ.connectedBody = blockTransform.gameObject.GetComponent<Rigidbody2D>();
        //clawFJ.enabled = true;
        //clawFJ.autoConfigureConnectedAnchor = false;

        if (blockTransform.gameObject.tag == "Bunny")
        {
            blockTransform.GetComponent<Rigidbody2D>().freezeRotation = false;
            blockTransform.GetComponent<BunnyController>().bunnyIsGrabbed = true;
        }

        createColliderAroundHeldObject();
    }

    void releaseBlock()
    {
        //clawFJ.connectedBody = null;
        //clawFJ.enabled = false;
        //clawFJ.autoConfigureConnectedAnchor = true;

        if (blockTransform != null)
        {
            blockTransform.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;

            blockTransform.gameObject.layer = oldPickupLayer;

            blockTransform.position = new Vector3(blockTransform.position.x, blockTransform.position.y, 0);

            blockTransform.GetComponent<Rigidbody2D>().isKinematic = false;
            blockTransform.SetParent(null);
            blockTransform.GetComponent<DontDestroy>().enabled = false;
            blockTransform.GetComponent<DontDestroy>().enabled = true;

            if (blockTransform.gameObject.tag == "Bunny")
            {
                blockTransform.GetComponent<Rigidbody2D>().freezeRotation = true;
                //blockTransform.GetComponent<BunnyController>().rotateBunny = true;
                blockTransform.GetComponent<BunnyController>().bunnyIsDropped = true;
            }

            blockTransform.GetComponent<BlockScoreChecker>().droppedBlock = true;
            blockTransform = null;
            floorClampScript.colliders.RemoveAt(floorClampScript.colliders.Count - 1);
            Destroy(heldBlockFakeBoxCollider);
        }

        if (leftProngScript != null)
            leftProngScript.releaseBlock();

        if (rightProngScript != null)
            rightProngScript.releaseBlock();
    }

    //creates a dummy box collider so that the now kinematic heldBlock will still collide with ground and other objects 
    private void createColliderAroundHeldObject()
    {
        heldBlockFakeBoxCollider = new GameObject("blockCollider");
        heldBlockFakeBoxCollider.tag = "Held";
        heldBlockFakeBoxCollider.layer = LayerMask.NameToLayer("RobotArm");
        heldBlockFakeBoxCollider.transform.SetParent(this.transform);
        heldBlockFakeBoxCollider.transform.localPosition = blockTransform.localPosition;
        heldBlockFakeBoxCollider.transform.localRotation = blockTransform.localRotation;
        heldBlockFakeBoxCollider.transform.localScale = blockTransform.localScale;
        Collider2D _collider = blockTransform.GetComponent<Collider2D>();
        if (_collider is BoxCollider2D)
        {
            BoxCollider2D box = heldBlockFakeBoxCollider.gameObject.AddComponent<BoxCollider2D>();
            Vector2 boxSize = new Vector2(blockTransform.gameObject.GetComponent<BoxCollider2D>().size.x, blockTransform.gameObject.GetComponent<BoxCollider2D>().size.y);
            box.size = boxSize;
        }
        else if (_collider is CapsuleCollider2D)
        {
            CapsuleCollider2D box = heldBlockFakeBoxCollider.gameObject.AddComponent<CapsuleCollider2D>();
            Vector2 boxSize = new Vector2(blockTransform.gameObject.GetComponent<CapsuleCollider2D>().size.x, blockTransform.gameObject.GetComponent<CapsuleCollider2D>().size.y);
            box.size = boxSize;
            box.direction = blockTransform.gameObject.GetComponent<CapsuleCollider2D>().direction;
        }
        floorClampScript.colliders.Add(_collider);
    }
}
