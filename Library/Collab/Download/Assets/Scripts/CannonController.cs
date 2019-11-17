using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  Handles the firing of the cannon.
 *  The cannon fires constantly at the given rate per second
 *  as long as the cannon is active,
 *  with the given constant force,
 *  at a random angle between the given min and max angles.
 *  The cannonball is destroyed after its given lifetime.
 */
public class CannonController : MonoBehaviour
{
    public GameObject ballPrefab;
    public float forceInNewtons = 1000f;
    public float minShotAngleInDegrees = -10f;
    public float maxShotAngleInDegrees = 85f;
    public float shotsPerSecond = 3f;
    public float ballLifeInSeconds = 20f;
    public int numberOfShots = 5;
    public float epsilon = 0.01f;
    public float cannonAngularSpeed = 10f;
    public Rigidbody2D cannon;

    private GameObject ballInsta;
    private Rigidbody2D ballRigid2d;
    private int numberOfShotsSoFar = 0;
    private bool isCycleOn;
    private float targetBallAngle;
    private float targetCannonAngle;
    private float timeSinceLastShot = 0f;
    private bool isEventTriggered = false;

    //angle to compensate for the offset between angles of cannon and cannonball visual coordinates
    private float ballOffsetAngle = 25f;

    void fire()
    {
        timeSinceLastShot = 0f;
        numberOfShotsSoFar += 1;
        ballInsta = Instantiate(ballPrefab, this.transform.position, this.transform.rotation);
        ballRigid2d = ballInsta.GetComponent<Rigidbody2D>();
        if (ballRigid2d != null)
        {
            ballRigid2d.AddForce(
                new Vector2(
                    -forceInNewtons * Mathf.Cos(
                        (targetBallAngle + ballOffsetAngle) * Mathf.PI / 180f
                    ),
                    forceInNewtons * Mathf.Sin(
                        (targetBallAngle + ballOffsetAngle) * Mathf.PI / 180f
                    )
                )
            );
            if (numberOfShotsSoFar < numberOfShots)
            {
                init_shot();
            }
            else
            {
                numberOfShotsSoFar = 0;
                isCycleOn = false;
            }
        }
        Destroy(ballInsta, ballLifeInSeconds);
    }


    void engageCannon()
    {
        float currentAngle = cannon.transform.rotation.z;
        //check cannon is in angular position
        //cannon uses the quaternion angle as above
        //ball uses nonquaternion angle
        if (Mathf.Abs(targetCannonAngle - currentAngle) < epsilon
            && timeSinceLastShot > 1 / shotsPerSecond)
        {
            fire();
        }
        else
        {
            cannonAngularSpeed = Mathf.Sign(currentAngle - targetCannonAngle) *
                Mathf.Abs(cannonAngularSpeed);
            cannon.transform.Rotate(new Vector3(0, 0,
                -cannonAngularSpeed * Time.fixedDeltaTime),
                Space.Self);
        }
    }

    void init_shot() {
        isCycleOn = true;
        targetBallAngle = Random.Range(minShotAngleInDegrees, maxShotAngleInDegrees);
        //approximation for conversion between quaternion and euler angle
        targetCannonAngle = targetBallAngle / -110f;
    }

   
    public void triggerCannon()
    {
        isEventTriggered = true;
    }

    private void Start()
    {
        triggerCannon();
    }

    private void FixedUpdate()
    {
        timeSinceLastShot += Time.fixedDeltaTime;
        if (isEventTriggered == true && isCycleOn == false)
        {
            init_shot();
            isEventTriggered = false;
        }
        if (isCycleOn)
        {
            engageCannon();
        }
    }

}
