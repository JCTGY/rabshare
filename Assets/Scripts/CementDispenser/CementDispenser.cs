using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CementDispenser : MonoBehaviour
{
    public float moveSpeed = 0.8f;
    public float breakForce = 1500.0f;
    //public Transform cementSpawnLoc;
    //public GameObject particleParent;
    //private List<GameObject> cementList;
    //private int particleCount = 0;

    //public float cementMass;
    //public float cementLinearDrag;
    //public float cementAngularDrag;
    //public float timeUntilDry = 4.0f;

    //public Sprite cementSprite;
    //public PhysicsMaterial2D cementPhysicsMaterial;

    public LineRenderer leftRope;
    public LineRenderer rightRope;
    public GameObject tetherPoint;
    //public bool dispenserEquipped = false;

    // Start is called before the first frame update
    void Start()
    {
        //cementList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        setRopePos();
    }

    private void FixedUpdate()
    {
        //dispenseCement();
        //moveRope();
    }

    /*private void moveRope()
    {
        DistanceJoint2D[] distanceJoints = GetComponents<DistanceJoint2D>();
        foreach (DistanceJoint2D distanceJoint in distanceJoints)
        {
            float distance = distanceJoint.distance;
            distance -= moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
            //if (distance <= maxDistance && distance >= minDistance)
            distanceJoint.distance = distance;
        }
    }*/

    private void setRopePos()
    {
        DistanceJoint2D[] distanceJoints = GetComponents<DistanceJoint2D>();
        Vector3 dispenserPos = this.transform.position;
        Vector2 anchorPoint = distanceJoints[0].anchor;
        anchorPoint = this.transform.rotation * anchorPoint;
        dispenserPos.x += anchorPoint.x;
        dispenserPos.y += anchorPoint.y;
        leftRope.SetPosition(0, dispenserPos);
        leftRope.SetPosition(1, tetherPoint.transform.position);
        dispenserPos = this.transform.position;
        anchorPoint = distanceJoints[1].anchor;
        anchorPoint = this.transform.rotation * anchorPoint;
        dispenserPos.x += anchorPoint.x;
        dispenserPos.y += anchorPoint.y;
        rightRope.SetPosition(0, dispenserPos);
        rightRope.SetPosition(1, tetherPoint.transform.position);
    }

    //private void dispenseCement()
    //{
    //    if (dispenserEquipped && Input.GetButtonDown("Magnet"))
    //    {
    //        GameObject cement = new GameObject("Cement Particle" + particleCount);
    //        cement.AddComponent<SpriteRenderer>();
    //        cement.GetComponent<SpriteRenderer>().sprite = cementSprite;
    //        Rigidbody2D rb = cement.AddComponent<Rigidbody2D>();
    //        rb.sharedMaterial = cementPhysicsMaterial;
    //        rb.mass = cementMass;
    //        rb.drag = cementLinearDrag;
    //        rb.angularDrag = cementAngularDrag;
    //        PolygonCollider2D pc = cement.AddComponent<PolygonCollider2D>();
    //        pc.sharedMaterial = cementPhysicsMaterial;
    //        cement.transform.position = cementSpawnLoc.transform.position;
    //        Vector3 scale = cement.transform.localScale;
    //        scale.x = 0.2f;
    //        scale.y = 0.2f;
    //        cement.transform.localScale = scale;
    //        cement.transform.SetParent(particleParent.transform);
    //        cement.layer = LayerMask.NameToLayer("Glue");
    //        cement.tag = "Glue";
    //        cementList.Add(cement);
    //        StartCoroutine(waitForCementDry(cement));
    //        particleCount++;
    //    }
    //}

    //IEnumerator waitForCementDry(GameObject cement)
    //{
    //    yield return new WaitForSeconds(timeUntilDry);

    //    cement.AddComponent<Cement>();
    //    cement.GetComponent<Cement>().particleParent = particleParent;
    //}

    // By Juo: Create Event
    private UnityAction ropeMoveUpListsener;
    private UnityAction ropeMoveDownListsener;
    //private UnityAction UseToolListener;

    void Awake()
    {
        ropeMoveUpListsener = new UnityAction(RopeMoveUp);
        ropeMoveDownListsener = new UnityAction(RopeMoveDown);
        //UseToolListener = new UnityAction(Use);
    }

    void OnEnable()
    {
        EventManager.StartListening("RopeMoveUp", ropeMoveUpListsener);
        EventManager.StartListening("RopeMoveDown", ropeMoveDownListsener);
        //EventManager.StartListening("Use", UseToolListener);
    }

    void OnDisable()
    {
        EventManager.StopListening("RopeMoveUp", ropeMoveUpListsener);
        EventManager.StopListening("RopeMoveDown", ropeMoveDownListsener);
        //EventManager.StopListening("Use", UseToolListener);
    }

    void RopeMoveUp()
    {
        DistanceJoint2D[] distanceJoints = GetComponents<DistanceJoint2D>();
        foreach (DistanceJoint2D distanceJoint in distanceJoints)
        {
            float distance = distanceJoint.distance;
            distance -= moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
            //if (distance <= maxDistance && distance >= minDistance)
            distanceJoint.distance = distance;
        }
    }

    void RopeMoveDown()
    {
        DistanceJoint2D[] distanceJoints = GetComponents<DistanceJoint2D>();
        foreach (DistanceJoint2D distanceJoint in distanceJoints)
        {
            float distance = distanceJoint.distance;
            distance -= moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
            //if (distance <= maxDistance && distance >= minDistance)
            distanceJoint.distance = distance;
        }
    }

    //private void Use()
    //{
    //    dispenseCement();
    //}
}
