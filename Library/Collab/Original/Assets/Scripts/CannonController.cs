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
 *  This class is designed so the cannon can be triggered over
 *  multiple distinct periods of time.
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

    private int numberOfShotsSoFar = 0;
    //angle to compensate for the offset between angles of cannon and cannonball visual coordinates
    private readonly float ballOffsetAngle = 25f;
    private bool isCannonTriggered = false;
    private float timeSinceLastShot = 0f;

    private void Fire(float targetBallAngle)
	{
        GameObject ballInsta = Instantiate(ballPrefab, this.transform.position, this.transform.rotation);
        Rigidbody2D ballRigid2d = ballInsta.GetComponent<Rigidbody2D>();
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
		}
		Destroy(ballInsta, ballLifeInSeconds);
	}

    private void Rotate(float currentCannonAngle, float targetCannonAngle)
	{
		cannonAngularSpeed = Mathf.Sign(currentCannonAngle - targetCannonAngle) *
				Mathf.Abs(cannonAngularSpeed);
		cannon.transform.Rotate(new Vector3(0, 0,
			-cannonAngularSpeed * Time.fixedDeltaTime),
			Space.Self);
	}

	private void RunCannonDecisionTree()
	{
		timeSinceLastShot += Time.fixedDeltaTime;
        if (timeSinceLastShot >= 1 / shotsPerSecond
            && numberOfShotsSoFar < numberOfShots)
		{
			float currentCannonAngle = cannon.transform.rotation.z;
			float targetBallAngle = Random.Range(minShotAngleInDegrees, maxShotAngleInDegrees);
			//approximation for conversion between quaternion and euler angle
			float targetCannonAngle = targetBallAngle / -110f;
			if (Mathf.Abs(targetCannonAngle - currentCannonAngle) > epsilon)
			{
				Rotate(currentCannonAngle, targetCannonAngle);
			}
			else
			{
				Fire(targetBallAngle);
				timeSinceLastShot = 0f;
                numberOfShotsSoFar += 1;
            }
		}
        else if (numberOfShotsSoFar >= numberOfShots)
        {
            isCannonTriggered = false;
            numberOfShotsSoFar = 0;
        }
	}

    private void FixedUpdate()
	{
		if (isCannonTriggered == true)
		{
			RunCannonDecisionTree();
		}
	}

	public void TriggerCannon()
	{
		isCannonTriggered = true;
	}

	private void Start()
	{
		TriggerCannon();
	}
}
