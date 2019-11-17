using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOController : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveForce = 400.0f;
    float rotationSpeed = 40.0f;
    public int controlScheme = 1; // different movement settings. 1 uses wasd. 2 uses arrow keys
    private string vert;
    private string hoz;

    public BatteryBar energyBar;
    public float energyToMove = 1.0f;

    private float lastEnergyCap = 100.0f;
    private float currentEnergy = 100.0f;

    bool outOfEnergy = false;

    // Start is called before the first frame update
    void Start()
    {
        if (controlScheme == 1)
        {
            vert = "Vertical";
            hoz = "Horizontal";
        }
        else if (controlScheme == 2)
        {
            vert = "Vertical2";
            hoz = "Horizontal2";
        }

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float step = rotationSpeed * Time.deltaTime;

        if (outOfEnergy == false)
        {
            if (Input.GetAxis(vert) > 0.0f)
            {
                rb.AddForce(Vector2.up * moveForce, ForceMode2D.Force);
                currentEnergy -= energyToMove;
                energyBar.fadeOutCharge(currentEnergy, lastEnergyCap);
            }
            if (Input.GetAxis(vert) < 0.0f)
            {
                rb.AddForce(Vector2.down * moveForce, ForceMode2D.Force);
                currentEnergy -= energyToMove;
                energyBar.fadeOutCharge(currentEnergy, lastEnergyCap);
            }
            if (Input.GetAxis(hoz) > 0.0f)
            {
                rb.AddForce(Vector2.right * moveForce, ForceMode2D.Force);
                currentEnergy -= energyToMove;
                energyBar.fadeOutCharge(currentEnergy, lastEnergyCap);
                if (controlScheme != 2)
                    this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.Euler(0, 0, -39), step);
            }
            if (Input.GetAxis(hoz) < 0.0f)
            {
                rb.AddForce(Vector2.left * moveForce, ForceMode2D.Force);
                currentEnergy -= energyToMove;
                energyBar.fadeOutCharge(currentEnergy, lastEnergyCap);
                if (controlScheme != 2)
                    this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.Euler(0, 0, 39), step);
            }
        }

        if (currentEnergy <= lastEnergyCap - 10.0f)
        {
            energyBar.useCharge();
            lastEnergyCap -= 10.0f;
            if (energyBar.empty)
            {
                outOfEnergy = true;
                this.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
            }
        }

        if (Mathf.Approximately(Input.GetAxis(hoz), 0.0f))
        {
            this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.Euler(0, 0, 0), step);
        }
    }

    //not being used. angular drag working better right now.
    IEnumerator slowDownUFO(float slowDownTime)
    {
        float time = 0.0f;
        while (time < slowDownTime)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, time / slowDownTime);
            rb.angularVelocity = Mathf.Lerp(rb.angularVelocity, 0.0f, time / slowDownTime);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
