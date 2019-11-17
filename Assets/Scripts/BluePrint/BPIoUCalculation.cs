using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class BPIoUCalculation : MonoBehaviour
{
    public static float IoU;
    public  BPCamera _cameraBP;
    public  BPCamera _cameraBlock;
    private static BPCamera cameraBP;
    private static BPCamera cameraBlock;
    public static Color32[] imageArrayBP;
    public static Color32[] imageArrayBlock;
    //private static NativeArray<Color32> imageNativeArrayBP;
    //private static NativeArray<Color32> imageNativeArrayBlock;
    // Start is called before the first frame update
    void Start()
    {
        IoU = 0;
        cameraBP = _cameraBP;
        cameraBlock = _cameraBlock;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9))
            IoUCalculation();
    }

    public static void IoUCalculation()
    {
        int sumBuildBlocks =0;
        int sumBluePrint = 0;
        int sumIntersection = 0;

        if (cameraBP != null)
        {
            cameraBlock.CamCapture();
            cameraBP.CamCapture();
            imageArrayBP = cameraBP.imageArray;
            //imageArrayBP = cameraBP.imageArray;
            //imageArrayBlock = cameraBlock.imageArray;

            imageArrayBlock = cameraBlock.imageArray;
            for (int i = 0; i < imageArrayBP.Length; i++)
            {
                if (imageArrayBP[i].a != 0)
                    sumBluePrint++;
                if (imageArrayBlock[i].a != 0)
                    sumBuildBlocks++;
                if (imageArrayBP[i].a != 0 && imageArrayBlock[i].a != 0)
                    sumIntersection++;
            }
            IoU = (float)sumIntersection / (sumBuildBlocks + sumBluePrint - sumIntersection) * 100;
        }
    }
}
