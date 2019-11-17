using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneConstrainer : MonoBehaviour
{
    public GameObject[] boneClamps;
    private Vector3 newV3 = new Vector3(0f, 0f, 0f);
    public RobotArmMover robotArmScript;

    // We use LateUpdate to grab the rotation from the Transform after all Updates from
    // other scripts have occured
    void LateUpdate()
    {
        //base of arm rotation.
        float rotationZ = boneClamps[0].transform.localEulerAngles.z;

        if (rotationZ < 360 && rotationZ > 270 || rotationZ < 5.0f && rotationZ >= 0.0f)
            rotationZ = 5.0f;
        else if (rotationZ > 180 && rotationZ < 270)
            rotationZ = 180.0f;


        newV3.z = rotationZ;
        boneClamps[0].transform.localRotation = Quaternion.Euler(newV3);

        //middle of arm rotation
        rotationZ = boneClamps[1].transform.localEulerAngles.z;
        if (rotationZ > 110.0f && rotationZ < 180.0f)
            rotationZ = 110.0f;
        else if (rotationZ < 250.0f && rotationZ > 180.0f)
            rotationZ = 250.0f;

        newV3.z = rotationZ;
        boneClamps[1].transform.localRotation = Quaternion.Euler(newV3);

        //top of arm rotation
        rotationZ = boneClamps[2].transform.localEulerAngles.z;

        if (rotationZ > 110.0f && rotationZ < 180.0f)
            rotationZ = 110.0f;
        else if (rotationZ < 250.0f && rotationZ > 180.0f)
            rotationZ = 250.0f;

        newV3.z = rotationZ;
        boneClamps[2].transform.localRotation = Quaternion.Euler(newV3);
    }
}
