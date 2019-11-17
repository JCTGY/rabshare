using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDuplicateOnLoad : MonoBehaviour
{
    private static DestroyDuplicateOnLoad instanceRef;

    private void Awake()
    {
        if (instanceRef == null)
        {
            instanceRef = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }
}