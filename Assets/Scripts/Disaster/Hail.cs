using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hail : MonoBehaviour
{
    public float torque = 5f;

    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.AddTorque(torque);
        Destroy(this.gameObject, 3f);
    }
}
