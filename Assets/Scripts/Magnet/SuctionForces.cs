using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuctionForces : MonoBehaviour
{
    public AreaEffector2D heavySuction;
    public AreaEffector2D leftSuction;
    public AreaEffector2D rightSuction;
    public AreaEffector2D leftAngleSuction;
    public AreaEffector2D rightAngleSuction;

    public int heavySuctionMag;
    public int leftSuctionMag;
    public int rightSuctionMag;
    public int leftAngleSuctionMag;
    public int rightAngleSuctionMag;

    // Start is called before the first frame update
    void Start()
    {
        heavySuction.forceMagnitude = heavySuctionMag;
        leftSuction.forceMagnitude = leftSuctionMag;
        rightSuction.forceMagnitude = rightSuctionMag;
        rightAngleSuction.forceMagnitude = rightSuctionMag;
        leftAngleSuction.forceMagnitude = leftAngleSuctionMag;
    }
}
