using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetIoUCameraLocation : MonoBehaviour
{
    public GameObject  basePixelLocation;
    public Camera mainCamera;
    Vector3 startLocation;

    // Start is called before the first frame update
    void Start()
    {
        //startLocation = mainCamera.WorldToScreenPoint(basePixelLocation.transform.position);
        //Rect cameraRect = GetComponent<Camera>().rect;
        //cameraRect.x = startLocation.x;
        //cameraRect.y = startLocation.y;
        //GetComponent<Camera>().rect = cameraRect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
