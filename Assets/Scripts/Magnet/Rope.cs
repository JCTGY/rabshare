using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Rope : MonoBehaviour
{
    public int numberOfJoints;
    public GameObject tetherPoint;
    public GameObject magnet;
    public Material ropeMaterial;

    public float moveSpeed = 0.5f;
    public float minDistance = 0.1f;
    public float maxDistance = 20.0f;

    public float jointMass = 10000000;
    public float jointLinearDrag = 1.5f;
    public float jointAngularDrag = 100.0f;

    private GameObject magnetJoint;
    private GameObject ropeSpawn;
    public List<GameObject> ropeJoints;

    private Vector3 magnetPos;
    private Vector3 origMagnetPos;

    private int ropeJointCount = 0;

    private int numPoints = 50;
    private Vector3[] positions = new Vector3[51];

    private bool ropeIsBuilt = false;

    private LineRenderer lineRenderer;

    private List<Vector3> origRopePositions;
    private List<Quaternion> origRopeRotations;
    private List<float> origRopeDistances;

    // Start is called before the first frame update
    void Start()
    {
        magnetPos = magnet.transform.position;
        origMagnetPos = magnetPos;
        ropeJoints = new List<GameObject>();
        origRopePositions = new List<Vector3>();
        origRopeRotations = new List<Quaternion>();
        origRopeDistances = new List<float>();
        spawnRopeJoints();
        copyOriginalValues();
        ropeIsBuilt = true;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numPoints + 1;
    }

    // Update is called once per frame
    void Update()
    {
        magnetPos = magnet.transform.position;
        //setRopePositions();
        if (ropeIsBuilt)
            drawRopeCurve();

        if (Input.GetButtonDown("Reset"))
            resetRope();
    }

    private void FixedUpdate()
    {
        constrainMagnetRotation();
        //moveRope();
    }

    private void constrainMagnetRotation()
    {
        ropeJoints[0].transform.up = ropeJoints[1].transform.position - ropeJoints[0].transform.position;
        magnet.transform.up = ropeJoints[1].transform.position - magnet.transform.position;
    }

    private void spawnRopeJoints()
    {
        float yDistance = Mathf.Abs(tetherPoint.transform.position.y - magnetPos.y);
        float yOffset = yDistance / (numberOfJoints); // + 1 to account for magnet
        for (int i = 0; i < numberOfJoints; i++)
        {
            buildRopeJoint(yOffset * (i));
            ropeJoints.Add(ropeSpawn);
            configureDistanceJoints(i);
        }
    }

    private void configureDistanceJoints(int i)
    {
        if (i == 0)
            return;

        DistanceJoint2D distanceJoint = ropeJoints[i - 1].GetComponent<DistanceJoint2D>();
        distanceJoint.connectedBody = ropeJoints[i].GetComponent<Rigidbody2D>();
        float distance = Vector2.Distance(ropeJoints[i - 1].transform.position, ropeJoints[i].transform.position);
        distanceJoint.distance = distance;

        if (i == ropeJointCount - 1)
        {
            distanceJoint = ropeJoints[i].GetComponent<DistanceJoint2D>();
            distanceJoint.connectedBody = tetherPoint.GetComponent<Rigidbody2D>();
            distance = Vector2.Distance(ropeJoints[i].transform.position, tetherPoint.transform.position);
            distanceJoint.distance = distance;
        }
    }

    /*private void moveRope()
    {
        float distance = 0.0f;

        if (Input.GetAxis("Vertical") > 0.0f)
        {
            foreach (GameObject ropeJoint in ropeJoints)
            {
                DistanceJoint2D distanceJoint = ropeJoint.GetComponent<DistanceJoint2D>();
                distance = distanceJoint.distance;
                distance -= moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
                //if (distance <= maxDistance && distance >= minDistance)
                distanceJoint.distance = distance;
            }
            //magnet.GetComponent<DistanceJoint2D>().distance -= moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        }

        if (Input.GetAxis("Vertical") < 0.0f && Magnet.magnetHitSomething == false)
        {
            foreach (GameObject ropeJoint in ropeJoints)
            {
                DistanceJoint2D distanceJoint = ropeJoint.GetComponent<DistanceJoint2D>();
                distance = distanceJoint.distance;
                distance -= moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
                //if (distance <= maxDistance && distance >= minDistance)
                    distanceJoint.distance = distance;
            }
            //magnet.GetComponent<DistanceJoint2D>().distance -= moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;

            constrainRopeRotation();
        }
    }*/

    //constraining the rotation of the first joint (which is fixed to the magnet) to always be rotated up towards the chain.
    //made the rope look more pleasant.
    private void constrainRopeRotation()
    {
        for (int i = 1; i < ropeJointCount; i++)
        {
            ropeJoints[i - 1].GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
            ropeJoints[i - 1].transform.up = ropeJoints[i].transform.position - ropeJoints[i - 1].transform.position;
        }
        ropeJoints[ropeJointCount - 1].transform.up = tetherPoint.transform.position - ropeJoints[ropeJointCount - 1].transform.position;
    }

    //UNUSED.
    //draws the rope line based on the positions of each distance joint. very rigid.
    private void setRopePositions()
    {
        LineRenderer line;

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

    //builds rope joints individually based off numberOfJoints.
    private void buildRopeJoint(float yOffset)
    {
        ropeSpawn = new GameObject("RopeJoint" + ropeJointCount);
        ropeSpawn.transform.position = magnet.transform.position;
        Vector3 pos = ropeSpawn.transform.position;
        pos.y += yOffset;
        ropeSpawn.transform.SetParent(this.transform);
        ropeSpawn.transform.localPosition = Vector3.zero;
        ropeSpawn.transform.position = pos;
        ropeSpawn.AddComponent<DistanceJoint2D>();
        ropeSpawn.GetComponent<DistanceJoint2D>().autoConfigureConnectedAnchor = false;
        ropeSpawn.GetComponent<DistanceJoint2D>().autoConfigureDistance = false;
        ropeSpawn.GetComponent<DistanceJoint2D>().maxDistanceOnly = true;
        ropeSpawn.GetComponent<Rigidbody2D>().mass = jointMass;
        ropeSpawn.GetComponent<Rigidbody2D>().drag = jointLinearDrag;
        ropeSpawn.GetComponent<Rigidbody2D>().angularDrag = jointAngularDrag;

        //first rope joint should be fixed to the magnet so that it doesn't fall.
        if (ropeJointCount == 0)
        {
            ropeSpawn.AddComponent<FixedJoint2D>();
            ropeSpawn.GetComponent<FixedJoint2D>().autoConfigureConnectedAnchor = false;
            ropeSpawn.GetComponent<FixedJoint2D>().connectedAnchor = Vector2.zero;
            ropeSpawn.GetComponent<FixedJoint2D>().connectedBody = magnet.GetComponent<Rigidbody2D>();
        }
        //ropeSpawn.AddComponent<BoxCollider2D>();
        ropeSpawn.layer = LayerMask.NameToLayer("Magnet");
        ropeJointCount++;
    }

    //using bezier curve to draw rope for a smoother feel.
    private void drawRopeCurve()
    {
        //Quadratic Curve
        //int ropeMidIdx = ropeJoints.Count / 2;
        //positions[0] = calculateQuadraticBezierPoint(0, ropeJoints[0].transform.position, ropeJoints[ropeMidIdx].transform.position, tetherPoint.transform.position);
        //for (int i = 1; i <= numPoints; i++)
        //{
        //    float t = i / (float)numPoints;
        //    positions[i - 1] = calculateQuadraticBezierPoint(t, magnet.transform.position, ropeJoints[ropeMidIdx].transform.position, tetherPoint.transform.position);
        //}
        //lineRenderer.SetPositions(positions);

        //Cubic Curve
        int ropeQtrIdx = ropeJoints.Count / 4;
        int ropeQtrIdx2 = (int)(ropeJoints.Count * .75f);
        positions[0] = calculateCubicBezierPoint(0, magnet.transform.position, ropeJoints[ropeQtrIdx].transform.position, ropeJoints[ropeQtrIdx2].transform.position, tetherPoint.transform.position);
        for (int i = 1; i <= numPoints; i++)
        {
            float t = i / (float)numPoints;
            positions[i] = calculateCubicBezierPoint(t, magnet.transform.position, ropeJoints[ropeQtrIdx].transform.position, ropeJoints[ropeQtrIdx2].transform.position, tetherPoint.transform.position);
        }
        lineRenderer.SetPositions(positions);
    }

    private Vector3 calculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        //B(t) = (1 - t)2P0 + 2(1 - t)tP1 + t2P2 , 0 < t < 1

        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }

    private Vector3 calculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        //B(t) = (1 - t)2P0 + 2(1 - t)tP1 + t2P2 , 0 < t < 1

        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;
        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;
        return p;
    }

    //needed to store all original values for a reset. everything is grabbed in the Start() function.
    private void copyOriginalValues()
    {
        foreach (GameObject joint in ropeJoints)
        {
            Vector3 pos = new Vector3(joint.transform.position.x, joint.transform.position.y, joint.transform.position.z);
            Quaternion rot = new Quaternion(joint.transform.localRotation.x, joint.transform.localRotation.y, joint.transform.localRotation.z, joint.transform.localRotation.w);
            float distance = joint.GetComponent<DistanceJoint2D>().distance;

            origRopePositions.Add(pos);
            origRopeRotations.Add(rot);
            origRopeDistances.Add(distance);
        }
    }

    public void resetRope()
    {
        for (int i = 0; i < ropeJoints.Count; i++)
        {
            ropeJoints[i].transform.position = origRopePositions[i];
            ropeJoints[i].transform.rotation = origRopeRotations[i];
            ropeJoints[i].GetComponent<DistanceJoint2D>().distance = origRopeDistances[i];
        }
    }

    // By Juo: Create Event
    private UnityAction ropeMoveUpListsener;
    private UnityAction ropeMoveDownListsener;

    void Awake()
    {
        ropeMoveUpListsener = new UnityAction(RopeMoveUp);
        ropeMoveDownListsener = new UnityAction(RopeMoveDown);
    }

    void OnEnable()
    {
        EventManager.StartListening("RopeMoveUp", ropeMoveUpListsener);
        EventManager.StartListening("RopeMoveDown", ropeMoveDownListsener);
    }

    void OnDisable()
    {
        EventManager.StopListening("RopeMoveUp", ropeMoveUpListsener);
        EventManager.StopListening("RopeMoveDown", ropeMoveDownListsener);
    }

    void RopeMoveUp()
    {
        float distance = 0.0f;

        foreach (GameObject ropeJoint in ropeJoints)
        {
            DistanceJoint2D distanceJoint = ropeJoint.GetComponent<DistanceJoint2D>();
            distance = distanceJoint.distance;
            distance -= moveSpeed * Time.deltaTime;
            if (distance <= maxDistance && distance >= minDistance)
                distanceJoint.distance = distance;
        }
        //Debug.Log("distance: " + distance);
    }

    void RopeMoveDown()
    {
        float distance = 0.0f;
        if (Magnet.magnetHitSomething == false)
        {
            foreach (GameObject ropeJoint in ropeJoints)
            {
                DistanceJoint2D distanceJoint = ropeJoint.GetComponent<DistanceJoint2D>();
                distance = distanceJoint.distance;
                distance += moveSpeed * Time.deltaTime;
                if (distance <= maxDistance && distance >= minDistance)
                    distanceJoint.distance = distance;
            }
            //Debug.Log("distance: " + distance);
            constrainRopeRotation();
        }
    }
}
