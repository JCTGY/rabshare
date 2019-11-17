using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorClamp : MonoBehaviour
{
    public GameObject floor;
    public GameObject buildingBase;
    public List<Collider2D> colliders;
    bool hitFloor;
    RobotArmRotationalMover moveScript;

    // Start is called before the first frame update
    void Start()
    {
        moveScript = GetComponent<RobotArmRotationalMover>();
    }

    // Update is called once per frame
    void Update()
    {
        hitFloor = false;

        foreach (Collider2D col in colliders)
        {
            Collider2D[] overlaps = Physics2D.OverlapBoxAll(col.bounds.center, col.bounds.size, col.transform.localRotation.z);
            foreach (Collider2D overlap in overlaps)
            {
                if (overlap.gameObject == floor || overlap.gameObject == buildingBase)
                {
                    moveScript.hitFloor = true;
                    hitFloor = true;
                    break;
                }
            }
            if (hitFloor) break;
        }

        if (hitFloor == false)
            moveScript.hitFloor = false;
    }
}
