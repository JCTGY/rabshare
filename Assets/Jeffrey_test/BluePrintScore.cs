using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePrintScore : MonoBehaviour
{
    public static int numBlocksInBP;
    public static GameObject[] bluePrintBlocks;
    // Start is called before the first frame update
    void Start()
    {
        numBlocksInBP = 0;
        bluePrintBlocks = GameObject.FindGameObjectsWithTag("BluePrint");

    }

    // Update is called once per frame

    void Update()
    {
        //if (GameMaster.BlockMomentum == false && Snapper.haveBlock == false)
        //{
        int count = 0;
        foreach (var bluePrintBlock in bluePrintBlocks)
        {
            //Debug.Log(bluePrintBlock.name);
            //Debug.Log(bluePrintBlock.GetComponent<BluePrintBlock>().inBluePrint);
            if (bluePrintBlock.GetComponent<BluePrintBlock>().inBluePrint == true)
                count += 1;
        }
        numBlocksInBP = count;
        //Debug.Log(numBlocksInBP);
        //}
    }
}

