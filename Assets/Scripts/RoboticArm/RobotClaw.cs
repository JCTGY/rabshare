using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RobotClaw : MonoBehaviour
{
    public GameObject leftProngIK;
    public GameObject rightProngIK;

    ClawProng leftProngScript;
    ClawProng rightProngScript;

    public GameObject leftProng;
    public GameObject rightProng;

    Vector3 origLeftPos;
    Vector3 origRightPos;

    bool clawOpen = true;
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

    FixedJoint2D clawFJ;

    int oldPickupLayer;

    UnityAction UseClawListener;

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
        origLeftPos = new Vector3(leftProngIK.transform.localPosition.x, leftProngIK.transform.localPosition.y, leftProngIK.transform.localPosition.z);
        origRightPos = new Vector3(rightProngIK.transform.localPosition.x, rightProngIK.transform.localPosition.y, rightProngIK.transform.localPosition.z);
        clawFJ = GetComponent<FixedJoint2D>();
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
                {
                    holdingBlock = true;
                    RobotArmAgent.pickedUpBlock = true;
                    oldPickupLayer = blockTransform.gameObject.layer;
                    blockTransform.gameObject.layer = LayerMask.NameToLayer("HeldBlock");
                    blockTransform.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 4;
                    clawFJ.connectedBody = blockTransform.gameObject.GetComponent<Rigidbody2D>();
                    clawFJ.enabled = true;
                    clawFJ.autoConfigureConnectedAnchor = false;

                    if (blockTransform.gameObject.tag == "Bunny")
                    {
                        blockTransform.GetComponent<Rigidbody2D>().freezeRotation = false;
                        blockTransform.GetComponent<BunnyController>().bunnyIsGrabbed = true;
                    }

                }
                else
                    releaseBlock();

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
        leftProngScript = leftProng.AddComponent<ClawProng>();
        leftProngScript.whichSide = "Left";
        leftProngScript.robotClawScript = this;
        rightProngScript = rightProng.AddComponent<ClawProng>();
        rightProngScript.whichSide = "Right";
        rightProngScript.robotClawScript = this;
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
            if (leftProngIK.transform.position == rightProngIK.transform.position || leftHitBlock)
                clawIsTouching = true;

            leftProngIK.transform.position = Vector3.MoveTowards(leftProngIK.transform.position, rightProngIK.transform.position, Time.deltaTime);
            //rightProngIK.transform.position = Vector3.MoveTowards(rightProngIK.transform.position, leftProngIK.transform.position, Time.deltaTime);

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
            if (leftProngIK.transform.position == rightProngIK.transform.position || rightHitBlock)
                clawIsTouching = true;

            //leftProngIK.transform.position = Vector3.MoveTowards(leftProngIK.transform.position, rightProngIK.transform.position, Time.deltaTime);
            rightProngIK.transform.position = Vector3.MoveTowards(rightProngIK.transform.position, leftProngIK.transform.position, Time.deltaTime);

            yield return null;
        }

        closeRightClawRoutineRunning = false;
    }

    IEnumerator openClaw()
    {
        bool clawInDefaultPos = false;

        while (clawInDefaultPos == false)
        {
            if (leftProngIK.transform.localPosition == origLeftPos && rightProngIK.transform.localPosition == origRightPos)
                clawInDefaultPos = true;

            leftProngIK.transform.localPosition = Vector3.MoveTowards(leftProngIK.transform.localPosition, origLeftPos, Time.deltaTime);
            rightProngIK.transform.localPosition = Vector3.MoveTowards(rightProngIK.transform.localPosition, origRightPos, Time.deltaTime);

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
            Destroy(leftProng.GetComponent<ClawProng>());
        }

        if (rightProngScript != null)
        {
            rightProngScript = null;
            Destroy(rightProng.GetComponent<ClawProng>());
        }
    }

    //check to see that claws are on opposite sides of picked up block. either horizontally or vertically.
    bool clawPositionsAreValid()
    {
        //if (prongsAreParallel() == false)
        //    return false;


        if (Vector3.Dot(leftProngIK.transform.position - blockTransform.position, blockTransform.right) <= 0 &&
            Vector3.Dot(rightProngIK.transform.position - blockTransform.position, blockTransform.right) > 0)
            return true;

        if (Vector3.Dot(leftProngIK.transform.position - blockTransform.position, blockTransform.right) > 0 &&
            Vector3.Dot(rightProngIK.transform.position - blockTransform.position, blockTransform.right) <= 0)
            return true;

        if (Vector3.Dot(leftProngIK.transform.position - blockTransform.position, blockTransform.up) <= 0 &&
            Vector3.Dot(rightProngIK.transform.position - blockTransform.position, blockTransform.up) > 0)
            return true;

        if (Vector3.Dot(leftProngIK.transform.position - blockTransform.position, blockTransform.up) > 0 &&
            Vector3.Dot(rightProngIK.transform.position - blockTransform.position, blockTransform.up) <= 0)
            return true;

        return false;
    }



    void releaseBlock()
    {
        clawFJ.connectedBody = null;
        clawFJ.enabled = false;
        clawFJ.autoConfigureConnectedAnchor = true;

        if (blockTransform != null)
        {
            blockTransform.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;

            blockTransform.gameObject.layer = oldPickupLayer;

            if (blockTransform.gameObject.tag == "Bunny")
            {
                blockTransform.GetComponent<Rigidbody2D>().freezeRotation = true;
                //blockTransform.GetComponent<BunnyController>().rotateBunny = true;
                blockTransform.GetComponent<BunnyController>().bunnyIsDropped = true;
            }

            blockTransform.GetComponent<BlockScoreChecker>().droppedBlock = true;
            blockTransform = null;
        }

        if (leftProngScript != null)
            leftProngScript.releaseBlock();

        if (rightProngScript != null)
            rightProngScript.releaseBlock();
    }
}
