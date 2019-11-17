using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeHinge : MonoBehaviour
{
    public int numberOfJoints;
    public GameObject tetherPoint;
    public GameObject magnet;
    public Material ropeMaterial;

    public float moveSpeed = 2.0f;
    public float minDistance = 0.5f;
    public float maxDistance = 20.0f;

    public float jointMass = 10000000;
    public float jointLinearDrag = 1.0f;
    public float jointAngularDrag = 1.0f;

    private GameObject magnetJoint;
    private GameObject ropeSpawn;
    private List<GameObject> ropeJoints;
    private DistanceJoint2D[] magnetDistanceJoints;

    public string whichMagnet;
    private int magnetDistanceJointIndex = 0;
    private Vector3 magnetPos;

    private int ropeJointCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        /*Debug.Log(magnetDistanceJointIndex);
        magnetDistanceJoints = magnet.GetComponents<DistanceJoint2D>();

        for (int i = 0; i < magnetDistanceJoints.Length; i++)
        {
            if (whichMagnet == "Center" && magnetDistanceJoints[i].anchor.x == 0)
                magnetDistanceJointIndex = i;
            else if (whichMagnet == "Left" && magnetDistanceJoints[i].anchor.x < 0)
                magnetDistanceJointIndex = i;
            else if (whichMagnet == "Right" && magnetDistanceJoints[i].anchor.x > 0)
                magnetDistanceJointIndex = i;
        }

        float magnetOffset = magnetDistanceJoints[magnetDistanceJointIndex].anchor.x;*/

        magnetPos = magnet.transform.position;
        //magnetPos.x -= magnetOffset;

        ropeJoints = new List<GameObject>();
        spawnRopeJoints();
    }

    // Update is called once per frame
    void Update()
    {
        magnetPos = magnet.transform.position;
        moveRope();
        setRopePositions();
    }

    private void FixedUpdate()
    {
        //if (whichMagnet == "Center")
        magnet.transform.up = ropeJoints[1].transform.position - magnetPos;
    }

    private void spawnRopeJoints()
    {
        float yDistance = Mathf.Abs(tetherPoint.transform.position.y - magnetPos.y);
        float yOffset = yDistance / (numberOfJoints); // + 1 to account for magnet
        for (int i = 0; i < numberOfJoints; i++)
        {
            buildRopeSpawn(yOffset * i);
            ropeJoints.Add(ropeSpawn);
            configureHingeJoints(i);
        }
    }

    private void configureHingeJoints(int i)
    {
        if (i == 0)
        {
            magnetJoint = ropeJoints[i];
            magnet.GetComponent<HingeJoint2D>().connectedBody = ropeJoints[i].GetComponent<Rigidbody2D>();
            float height = magnetPos.y - ropeJoints[i].transform.position.y;
            magnet.GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0.0f, height);
        }
        else
        {
            HingeJoint2D hingeJoint = ropeJoints[i - 1].GetComponent<HingeJoint2D>();
            hingeJoint.connectedBody = ropeJoints[i].GetComponent<Rigidbody2D>();
            float height = ropeJoints[i - 1].transform.position.y - ropeJoints[i].transform.position.y;
            hingeJoint.connectedAnchor = new Vector2(hingeJoint.connectedAnchor.x, height);
        }

        if (i == ropeJointCount - 1)
        {
            HingeJoint2D hingeJoint = ropeJoints[i].GetComponent<HingeJoint2D>();
            hingeJoint.connectedBody = tetherPoint.GetComponent<Rigidbody2D>();
            float height = (ropeJoints[i].transform.position.y - 0.5f) - tetherPoint.transform.position.y;
            hingeJoint.connectedAnchor = new Vector2(hingeJoint.connectedAnchor.x, height);
        }
    }

    private void moveRope()
    {
        float distance = 0.0f;

        foreach (GameObject ropeJoint in ropeJoints)
        {
            HingeJoint2D hingeJoint = ropeJoint.GetComponent<HingeJoint2D>();
            //distance = distanceJoint.distance;
            Vector2 anchorPos = hingeJoint.connectedAnchor;
            anchorPos.y += moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
            //if (distance <= maxDistance && distance >= minDistance)
            hingeJoint.connectedAnchor = anchorPos;
        }

        magnet.GetComponent<DistanceJoint2D>().distance -= moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;

        if (Input.GetAxis("Vertical") < 0.0f)
        {
            for (int i = 1; i < ropeJointCount; i++)
                ropeJoints[i - 1].transform.up = ropeJoints[i].transform.position - ropeJoints[i - 1].transform.position;
            ropeJoints[ropeJointCount - 1].transform.up = tetherPoint.transform.position - ropeJoints[ropeJointCount - 1].transform.position;
        }
        /*Vector2 anchor = magnet.GetComponent<HingeJoint2D>().connectedAnchor;
        anchor.y += moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        magnet.GetComponent<HingeJoint2D>().connectedAnchor = anchor;*/
    }

    private void setRopePositions()
    {
        LineRenderer line = magnet.GetComponent<LineRenderer>();
        line.SetPosition(0, magnetPos);
        line.SetPosition(1, ropeJoints[0].transform.position);

        for (int i = 1; i < ropeJointCount; i++)
        {
            line = ropeJoints[i - 1].GetComponent<LineRenderer>();
            line.SetPosition(0, ropeJoints[i - 1].transform.position);
            line.SetPosition(1, ropeJoints[i].transform.position);
        }

        line = ropeJoints[ropeJointCount - 1].GetComponent<LineRenderer>();
        line.SetPosition(0, ropeJoints[ropeJointCount - 1].transform.position);
        line.SetPosition(1, tetherPoint.transform.position);
    }

    private void buildRopeSpawn(float yOffset)
    {
        ropeSpawn = new GameObject("RopeJoint" + ropeJointCount);
        ropeSpawn.transform.position = magnet.transform.position;
        Vector3 pos = ropeSpawn.transform.position;
        pos.y += yOffset;
        ropeSpawn.transform.SetParent(this.transform);
        ropeSpawn.transform.localPosition = Vector3.zero;
        ropeSpawn.transform.position = pos;
        ropeSpawn.AddComponent<HingeJoint2D>();
        ropeSpawn.GetComponent<HingeJoint2D>().autoConfigureConnectedAnchor = false;
        ropeSpawn.GetComponent<HingeJoint2D>().connectedAnchor = Vector2.zero;
        //ropeSpawn.GetComponent<HingeJoint2D>().autoConfigureDistance = false;
        ropeSpawn.GetComponent<Rigidbody2D>().mass = (jointMass + (10000 * ropeJointCount));
        ropeSpawn.GetComponent<Rigidbody2D>().drag = jointLinearDrag;
        ropeSpawn.GetComponent<Rigidbody2D>().angularDrag = jointAngularDrag;
        ropeSpawn.AddComponent<LineRenderer>();
        ropeSpawn.GetComponent<LineRenderer>().startWidth = .10f;
        ropeSpawn.GetComponent<LineRenderer>().endWidth = .10f;
        ropeSpawn.GetComponent<LineRenderer>().positionCount = 2;
        ropeSpawn.GetComponent<LineRenderer>().material = ropeMaterial;
        if (ropeJointCount == 0)
        {
            ropeSpawn.AddComponent<FixedJoint2D>();
            ropeSpawn.GetComponent<FixedJoint2D>().autoConfigureConnectedAnchor = false;
            ropeSpawn.GetComponent<FixedJoint2D>().connectedAnchor = Vector2.zero;
            ropeSpawn.GetComponent<FixedJoint2D>().connectedBody = magnet.GetComponent<Rigidbody2D>();
        }
        ropeJointCount++;
    }
}
