using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is attached to the Explosion prefab, which is a 1.2 second long animation.
//The 2 second delay before destroying the GameObject allows
//  the animation to complete before the GameObject is destroyed.
public class ExplosionController : MonoBehaviour
{
    void Start()
    {
        Destroy(this.gameObject, 2.0f);
    }
}
