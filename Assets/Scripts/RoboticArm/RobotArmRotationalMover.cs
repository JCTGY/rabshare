using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RobotArmRotationalMover : MonoBehaviour
{
    public List<GameObject> bones;
    public bool isClaw;

    public float moveSpeed = 2.0f;

    public float rotationLowArm;
    public float rotationMidArm;
    public float rotationTopArm;




    UnityAction robotBaseMoveUpListener;
    UnityAction robotBaseMoveDownListener;
    UnityAction robotMidMoveUpListener;
    UnityAction robotMidMoveDownListener;
    UnityAction robotTopMoveUpListener;
    UnityAction robotTopMoveDownListener;

    [HideInInspector]
    public bool hitFloor;
    bool facingRight;

    private void Awake()
    {
        robotBaseMoveUpListener = new UnityAction(baseMoveUp);
        robotBaseMoveDownListener = new UnityAction(baseMoveDown);
        robotMidMoveUpListener = new UnityAction(midMoveUp);
        robotMidMoveDownListener = new UnityAction(midMoveDown);
        robotTopMoveUpListener = new UnityAction(topMoveUp);
        robotTopMoveDownListener = new UnityAction(topMoveDown);
    }

    private void OnEnable()
    {
        EventManager.StartListening("BaseMoveUp", robotBaseMoveUpListener);
        EventManager.StartListening("BaseMoveDown", robotBaseMoveDownListener);
        EventManager.StartListening("MidMoveUp", robotMidMoveUpListener);
        EventManager.StartListening("MidMoveDown", robotMidMoveDownListener);
        EventManager.StartListening("TopMoveUp", robotTopMoveUpListener);
        EventManager.StartListening("TopMoveDown", robotTopMoveDownListener);
    }

    private void OnDisable()
    {
        EventManager.StopListening("BaseMoveUp", robotBaseMoveUpListener);
        EventManager.StopListening("BaseMoveDown", robotBaseMoveDownListener);
        EventManager.StopListening("MidMoveUp", robotMidMoveUpListener);
        EventManager.StopListening("MidMoveDown", robotMidMoveDownListener);
        EventManager.StopListening("TopMoveUp", robotTopMoveUpListener);
        EventManager.StopListening("TopMoveDown", robotTopMoveDownListener);
    }

    //Complex Bone Movements
    void baseMoveUp()
    {
        if (!facingRight && hitFloor)
            return;

        Vector3 rot = bones[0].transform.localRotation.eulerAngles;
        rot.z += moveSpeed * Time.fixedDeltaTime;
        if (rot.z < 180)
            rot.z = Mathf.Clamp(rot.z, 0.0f, 50.0f);
        bones[0].transform.localRotation = Quaternion.Euler(rot);
    }

    void baseMoveDown()
    {
        if (facingRight && hitFloor)
            return;

        Vector3 rot = bones[0].transform.localRotation.eulerAngles;
        rot.z -= moveSpeed * Time.fixedDeltaTime;
        if (rot.z > 180.0f)
            rot.z = Mathf.Clamp(rot.z, 205.0f, 360.0f);
        bones[0].transform.localRotation = Quaternion.Euler(rot);
    }

    void midMoveUp()
    {
        if (!facingRight && hitFloor)
            return;

        Vector3 rot = bones[1].transform.localRotation.eulerAngles;
        rot.z += moveSpeed * Time.fixedDeltaTime;
        if (rot.z > 90.0f && rot.z < 300.0f)
            rot.z = Mathf.Clamp(rot.z, 0.0f, 250.0f);
        bones[1].transform.localRotation = Quaternion.Euler(rot);
    }

    void midMoveDown()
    {
        if (facingRight && hitFloor)
            return;

        Vector3 rot = bones[1].transform.localRotation.eulerAngles;
        rot.z -= moveSpeed * Time.fixedDeltaTime;
        if (rot.z > 300.0f)
            rot.z = Mathf.Clamp(rot.z, 305.0f, 360.0f);
        bones[1].transform.localRotation = Quaternion.Euler(rot);
    }

    void topMoveUp()
    {
        if (!facingRight && hitFloor)
            return;

        Vector3 rot = bones[2].transform.localRotation.eulerAngles;
        rot.z += moveSpeed * Time.fixedDeltaTime;
        if (rot.z > 90.0f && rot.z < 300.0f)
            rot.z = Mathf.Clamp(rot.z, 0.0f, 230.0f);
        bones[2].transform.localRotation = Quaternion.Euler(rot);
    }

    void topMoveDown()
    {
        if (facingRight && hitFloor)
            return;

        Vector3 rot = bones[2].transform.localRotation.eulerAngles;
        rot.z -= moveSpeed * Time.fixedDeltaTime;
        if (rot.z > 270.0f)
            rot.z = Mathf.Clamp(rot.z, 300.0f, 360.0f);
        bones[2].transform.localRotation = Quaternion.Euler(rot);
    }

    void calculateRotation()
    {
        //first one
        Vector3 dirLow = bones[1].transform.position - bones[0].transform.position;
        float angleLow = Mathf.Atan2(dirLow.y, dirLow.x) * Mathf.Rad2Deg;
        rotationLowArm = Quaternion.AngleAxis(angleLow, Vector3.forward).eulerAngles.z;

        Vector3 dirMid = bones[2].transform.position - bones[1].transform.position;
        float angleMid = Mathf.Atan2(dirMid.y, dirMid.x) * Mathf.Rad2Deg;
        rotationMidArm = Quaternion.AngleAxis(angleMid, Vector3.forward).eulerAngles.z;

        Vector3 dirTop = bones[3].transform.position - bones[2].transform.position;
        float angleTop = Mathf.Atan2(dirTop.y, dirTop.x) * Mathf.Rad2Deg;
        rotationTopArm = Quaternion.AngleAxis(angleTop, Vector3.forward).eulerAngles.z;
    }

    private void Update()
    {
        float rot = bones[0].transform.localRotation.eulerAngles.z;
        if (rot < 312.0f && rot > 180.0f)
            facingRight = true;
        else
            facingRight = false;
        calculateRotation();
    }
}
