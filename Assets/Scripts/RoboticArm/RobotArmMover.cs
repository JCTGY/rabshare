using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RobotArmMover : MonoBehaviour
{
    public List<Rigidbody2D> boneRBs;
    Quaternion[] previousBoneRotations = new Quaternion[3];

    public GameObject IKComponent;
    public float moveSpeed = 2.0f;
    public float moveForce = 100.0f;
    public float maxVelocity = 10.0f;
    public Collider2D[] armColliders;
    public Collider2D middlePillarCollider;
    Collider2D[] clawOverlaps;
    Collider2D[] middlePillarOverlaps;
    public Collider2D clawCollider;
    Collider2D objectCollided;
    bool needToMoveAway;

    public GameObject minYClamp;

    delegate void Del();
    Del overlapFunctionHandler;

    public GameObject distanceCheckPosition;
    float maxDistance = 0.25f;

    //Set by collision event on each claw prong.
    [HideInInspector]
    public bool clawHitFloor;
    [HideInInspector]
    public float clawYClamp;

    float clawXClamp;
    Vector3 movePos;

    UnityAction robotArmMoveUpListener;
    UnityAction robotArmMoveDownListener;
    UnityAction robotArmMoveRightListener;
    UnityAction robotArmMoveLeftListener;

    UnityAction robotBaseMoveUpListener;
    UnityAction robotBaseMoveDownListener;
    UnityAction robotMidMoveUpListener;
    UnityAction robotMidMoveDownListener;
    UnityAction robotTopMoveUpListener;
    UnityAction robotTopMoveDownListener;

    bool facingRight;

    private void Awake()
    {
        //robotArmMoveUpListener = new UnityAction(armMoveUp);
        //robotArmMoveDownListener = new UnityAction(armMoveDown);
        //robotArmMoveRightListener = new UnityAction(armMoveRight);
        //robotArmMoveLeftListener = new UnityAction(armMoveLeft);

        robotBaseMoveUpListener = new UnityAction(baseMoveUp);
        robotBaseMoveDownListener = new UnityAction(baseMoveDown);
        robotMidMoveUpListener = new UnityAction(midMoveUp);
        robotMidMoveDownListener = new UnityAction(midMoveDown);
        robotTopMoveUpListener = new UnityAction(topMoveUp);
        robotTopMoveDownListener = new UnityAction(topMoveDown);
    }

    private void OnEnable()
    {
        //EventManager.StartListening("ArmMoveRight", robotArmMoveRightListener);
        //EventManager.StartListening("ArmMoveLeft", robotArmMoveLeftListener);
        //EventManager.StartListening("ArmMoveUp", robotArmMoveUpListener);
        //EventManager.StartListening("ArmMoveDown", robotArmMoveDownListener);

        EventManager.StartListening("BaseMoveUp", robotBaseMoveUpListener);
        EventManager.StartListening("BaseMoveDown", robotBaseMoveDownListener);
        EventManager.StartListening("MidMoveUp", robotMidMoveUpListener);
        EventManager.StartListening("MidMoveDown", robotMidMoveDownListener);
        EventManager.StartListening("TopMoveUp", robotTopMoveUpListener);
        EventManager.StartListening("TopMoveDown", robotTopMoveDownListener);
    }

    private void OnDisable()
    {
        //EventManager.StopListening("ArmMoveRight", robotArmMoveRightListener);
        //EventManager.StopListening("ArmMoveLeft", robotArmMoveLeftListener);
        //EventManager.StopListening("ArmMoveUp", robotArmMoveUpListener);
        //EventManager.StopListening("ArmMoveDown", robotArmMoveDownListener);

        EventManager.StopListening("BaseMoveUp", robotBaseMoveUpListener);
        EventManager.StopListening("BaseMoveDown", robotBaseMoveDownListener);
        EventManager.StopListening("MidMoveUp", robotMidMoveUpListener);
        EventManager.StopListening("MidMoveDown", robotMidMoveDownListener);
        EventManager.StopListening("TopMoveUp", robotTopMoveUpListener);
        EventManager.StopListening("TopMoveDown", robotTopMoveDownListener);
    }

    //Arm Movements with IK
    //void armMoveRight()
    //{
    //    if (clawHitFloor)
    //        return;

    //    movePos = IKComponent.transform.position;
    //    movePos.x += (moveSpeed * Time.deltaTime);
    //    checkClawCollision();
    //    setClawIKPosition();
    //}

    //void armMoveLeft()
    //{
    //    if (clawHitFloor)
    //        return;

    //    movePos = IKComponent.transform.position;
    //    movePos.x -= (moveSpeed * Time.deltaTime);
    //    checkClawCollision();
    //    setClawIKPosition();
    //}

    //void armMoveUp()
    //{
    //    if (clawHitFloor)
    //        return;

    //    movePos = IKComponent.transform.position;
    //    movePos.y += (moveSpeed * Time.deltaTime);
    //    checkClawCollision();
    //    setClawIKPosition();
    //}

    //void armMoveDown()
    //{
    //    if (clawHitFloor)
    //        return;

    //    movePos = IKComponent.transform.position;
    //    movePos.y -= (moveSpeed * Time.deltaTime);
    //    checkClawCollision();
    //    setClawIKPosition();
    //}

    //Complex Bone Movements
    void baseMoveUp()
    {
        //if (clawHitFloor && !facingRight)
        //    return;

        //float rot = boneRBs[0].rotation;
        //rot += (moveSpeed * Time.fixedDeltaTime);
        //boneRBs[0].MoveRotation(rot);

        boneRBs[0].angularVelocity += moveSpeed * Time.fixedDeltaTime;
        boneRBs[0].angularVelocity = Mathf.Clamp(boneRBs[0].angularVelocity, -maxVelocity, maxVelocity);

        //boneRBs[0].AddTorque(moveSpeed);
        //boneRBs[0].angularVelocity = Mathf.Clamp(boneRBs[0].angularVelocity, -maxVelocity, maxVelocity);

        //checkClawCollision();
        //if (needToMoveAway)
        //    overlapFunctionHandler();
        //else

    }

    void baseMoveDown()
    {
        if (clawHitFloor && facingRight)
            return;

        //float rot = boneRBs[0].rotation;
        //rot -= (moveSpeed * Time.fixedDeltaTime);
        //boneRBs[0].MoveRotation(rot);

        boneRBs[0].angularVelocity -= moveSpeed * Time.fixedDeltaTime;
        boneRBs[0].angularVelocity = Mathf.Clamp(boneRBs[0].angularVelocity, -maxVelocity, maxVelocity);

        //boneRBs[0].AddTorque(-moveSpeed);
        //boneRBs[0].angularVelocity = Mathf.Clamp(boneRBs[0].angularVelocity, -maxVelocity, maxVelocity);

        //checkClawCollision();
        //if (needToMoveAway)
        //    overlapFunctionHandler();
        //else

    }

    void midMoveUp()
    {
        if (clawHitFloor && !facingRight)
            return;

        //float rot = boneRBs[1].rotation;
        //rot += (moveSpeed * Time.fixedDeltaTime);
        //boneRBs[1].MoveRotation(rot);

        boneRBs[1].angularVelocity += moveSpeed * Time.fixedDeltaTime;
        boneRBs[1].angularVelocity = Mathf.Clamp(boneRBs[1].angularVelocity, -maxVelocity, maxVelocity);

        //checkClawCollision();
        //if (needToMoveAway)
        //    overlapFunctionHandler();
        //else

    }

    void midMoveDown()
    {
        if (clawHitFloor && facingRight)
            return;

        //float rot = boneRBs[1].rotation;
        //rot -= (moveSpeed * Time.fixedDeltaTime);
        //boneRBs[1].MoveRotation(rot);

        boneRBs[1].angularVelocity -= moveSpeed * Time.fixedDeltaTime;
        boneRBs[1].angularVelocity = Mathf.Clamp(boneRBs[1].angularVelocity, -maxVelocity, maxVelocity);

        //checkClawCollision();
        //if (needToMoveAway)
        //    overlapFunctionHandler();
        //else

    }

    void topMoveUp()
    {
        if (clawHitFloor && !facingRight)
            return;

        //float rot = boneRBs[2].rotation;
        //rot += (moveSpeed * Time.fixedDeltaTime);
        //boneRBs[2].MoveRotation(rot);

        boneRBs[2].angularVelocity += moveSpeed * Time.fixedDeltaTime;
        boneRBs[2].angularVelocity = Mathf.Clamp(boneRBs[2].angularVelocity, -maxVelocity, maxVelocity);

        //checkClawCollision();
        //if (needToMoveAway)
        //    overlapFunctionHandler();
        //else

    }

    void topMoveDown()
    {
        if (clawHitFloor && facingRight)
            return;

        //float rot = boneRBs[2].rotation;
        //rot -= (moveSpeed * Time.fixedDeltaTime);
        //boneRBs[2].MoveRotation(rot);

        boneRBs[2].angularVelocity -= moveSpeed * Time.fixedDeltaTime;
        boneRBs[2].angularVelocity = Mathf.Clamp(boneRBs[2].angularVelocity, -maxVelocity, maxVelocity);

        //checkClawCollision();
        //if (needToMoveAway)
        //    overlapFunctionHandler();
        //else

    }

    //void checkClawCollision()
    //{
    //    //Store colliders that claw and middle pillar are overlapping with.
    //    clawOverlaps = Physics2D.OverlapBoxAll(clawCollider.bounds.center, clawCollider.bounds.size, clawCollider.transform.localRotation.z);
    //    middlePillarOverlaps = Physics2D.OverlapBoxAll(middlePillarCollider.bounds.center, middlePillarCollider.bounds.size, middlePillarCollider.transform.localRotation.z);
    //    if (needToMoveAway == false)
    //    {
    //        //check to see if the claw is overlapping with any of the robotic arm's colliders.
    //        //if so, set state to move claw away from whatever it's touching.
    //        foreach (Collider2D overlaps in clawOverlaps)
    //        {
    //            foreach (Collider2D col in armColliders)
    //            {
    //                if (overlaps == col)
    //                {
    //                    needToMoveAway = true;
    //                    objectCollided = col;
    //                    //if (col.gameObject.layer == LayerMask.NameToLayer("Cart"))
    //                    //{
    //                    //    overlapFunctionHandler = clampClawXPos;
    //                    //    clawXClamp = IKComponent.transform.position.x;
    //                    //}
    //                    //else
    //                    overlapFunctionHandler = moveAwayFromClawCollision;
    //                    break;
    //                }
    //                if (needToMoveAway)
    //                    break;
    //            }
    //        }

    //        //check to see if the middle pillar is colliding with the bottom pillar.
    //        //if so, set state to move middle pillar away from bottom pillar
    //        if (objectCollided == null && needToMoveAway == false)
    //        {
    //            foreach (Collider2D overlaps in middlePillarOverlaps)
    //            {
    //                if (overlaps == armColliders[1])
    //                {
    //                    needToMoveAway = true;
    //                    objectCollided = armColliders[1];
    //                    overlapFunctionHandler = moveAwayFromPillarCollision;
    //                    break;
    //                }
    //                if (needToMoveAway)
    //                    break;
    //            }
    //        }
    //    }
    //}

    //void setClawIKPosition()
    //{
    //    //if claw is colliding with something, move it away. else set the position as normal
    //    if (needToMoveAway == true)
    //        overlapFunctionHandler();
    //    else
    //    {
    //        //clamp at ground Y position
    //        //if (clawHitFloor)
    //        //    movePos.y = Mathf.Clamp(movePos.y, clawYClamp, float.MaxValue);
    //        //else
    //        //    movePos.y = Mathf.Clamp(movePos.y, minYClamp.transform.position.y, float.MaxValue);

    //        //constrain IKComponent distance from origin point. shouldn't stray too far or else there will be bugs
    //        Vector3 difference = movePos - distanceCheckPosition.transform.position;
    //        Vector3 direction = difference.normalized;
    //        float distance = Mathf.Min(maxDistance, difference.magnitude);

    //        IKComponent.transform.position = distanceCheckPosition.transform.position + direction * distance;
    //    }
    //}

    ////moves in a direction away from objectCollided as long as claw is still colliding with that object.
    //void moveAwayFromClawCollision()
    //{
    //    Debug.Log("Moving");
    //    Vector3 direction = clawCollider.transform.position - objectCollided.transform.position;
    //    direction.Normalize();
    //    Collider2D arrayCheck;
    //    if (arrayCheck = Array.Find(clawOverlaps, overlap => overlap == objectCollided))
    //    {
    //        IKComponent.transform.position = IKComponent.transform.position + (direction * Time.deltaTime * 10.0f);
    //    }
    //    else
    //    {
    //        objectCollided = null;
    //        needToMoveAway = false;
    //    }
    //}

    ////moves in a direction away from objectCollided as long as middlePillar is still colliding with that object
    //void moveAwayFromPillarCollision()
    //{
    //    Vector3 direction = middlePillarCollider.transform.position - armColliders[1].transform.position;
    //    direction.Normalize();
    //    Collider2D arrayCheck;
    //    if (arrayCheck = Array.Find(middlePillarOverlaps, overlap => overlap == objectCollided))
    //    {
    //        IKComponent.transform.position = IKComponent.transform.position + (direction * Time.deltaTime);
    //    }
    //    else
    //    {
    //        objectCollided = null;
    //        needToMoveAway = false;
    //    }
    //}

    //void clampClawXPos()
    //{
    //    if (IKComponent.transform.position.x - clawCollider.transform.position.x < 0)
    //        movePos.x = Mathf.Clamp(movePos.x, clawXClamp, float.MaxValue);
    //    else
    //        movePos.x = Mathf.Clamp(movePos.x, float.MinValue, clawXClamp);
    //    IKComponent.transform.position = movePos;
    //}

    private void Update()
    {
        if (boneRBs[0].rotation >= 90.0f)
            facingRight = false;
        else
            facingRight = true;

        if (Input.anyKey == false)
        {
            foreach (Rigidbody2D rb in boneRBs)
                rb.angularVelocity = 0.0f;
        }
    }

    //private void FixedUpdate()
    //{
    //    if (!clawHitFloor)
    //    {
    //        for (int i = 0; i < bones.Count; i++)
    //            previousBoneRotations[i] = bones[i].transform.rotation;
    //    }
    //}

    //private void LateUpdate()
    //{
    //    if (clawHitFloor)
    //    {
    //        for (int i = 0; i < bones.Count; i++)
    //        {
    //            bones[i].transform.rotation = previousBoneRotations[i];
    //        }
    //        clawHitFloor = false;
    //    }
    //}
}