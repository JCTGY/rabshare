using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSelector : MonoBehaviour
{
    public GameObject magnet;
    public GameObject cementDispenser;

    public GameObject magnetRope;

    private Rope rope;
    private CementDispenser dispenserScript;

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        cementDispenser.GetComponent<BoxCollider2D>().enabled = false;
        rope = magnetRope.GetComponent<Rope>();
        dispenserScript = cementDispenser.GetComponent<CementDispenser>();
        hideCementDispenser();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("SelectMagnet"))
            equipMagnet();
        else if (Input.GetButtonDown("SelectCementDispenser"))
            equipCementDispenser();

        //this makes it so the tools don't look like they teleport when setting distance on a switch
        if (magnet.activeSelf == true && cementDispenser.activeSelf == false)
            cementDispenser.transform.position = magnet.transform.position;
        if (magnet.activeSelf == false && cementDispenser.activeSelf == true)
            magnet.transform.position = cementDispenser.transform.position;
    }

    private void equipMagnet()
    {
        magnet.gameObject.SetActive(true);

        FixedJoint2D fj = rope.ropeJoints[0].GetComponent<FixedJoint2D>();
        fj.enabled = false;
        fj.enabled = true;

        //this is used for one single curved line renderer
        Color color = magnetRope.GetComponent<LineRenderer>().startColor;
        color.a = 1;
        magnetRope.GetComponent<LineRenderer>().startColor = color;
        color = magnetRope.GetComponent<LineRenderer>().endColor;
        color.a = 1;
        magnetRope.GetComponent<LineRenderer>().endColor = color;

        foreach (GameObject ropeJoint in rope.ropeJoints)
        {
            //this is used if using multiple straight line renderers for each joint
            //makes each joint opague
            //Color color = ropeJoint.GetComponent<LineRenderer>().startColor;
            //color.a = 1;
            //ropeJoint.GetComponent<LineRenderer>().startColor = color;
            //color = ropeJoint.GetComponent<LineRenderer>().endColor;
            //color.a = 1;
            //ropeJoint.GetComponent<LineRenderer>().endColor = color;

            //match distance in each joint to cementDispenser
            ropeJoint.GetComponent<DistanceJoint2D>().distance = cementDispenser.GetComponent<DistanceJoint2D>().distance / rope.GetComponent<Rope>().numberOfJoints;
        }
        cementDispenser.GetComponent<BoxCollider2D>().enabled = false;
        hideCementDispenser();
		cementDispenser.GetComponent<Glue>().enabled = false;
	}

    private void equipCementDispenser()
    {
        //drops block if currently holding one and try to switch tools
        Snapper snapperScript = magnet.transform.GetComponentInChildren<Snapper>(true);
        if (Snapper.haveBlock)
            snapperScript.turnOffMagnet();
        magnet.gameObject.SetActive(false);

        //use this if using one single curved line renderer in rope parent
        Color color = magnetRope.GetComponent<LineRenderer>().startColor;
        color.a = 0;
        magnetRope.GetComponent<LineRenderer>().startColor = color;
        color = magnetRope.GetComponent<LineRenderer>().endColor;
        color.a = 0;
        magnetRope.GetComponent<LineRenderer>().endColor = color;

        //use this if using multiple straight line renderers in each rope joint
        //foreach (GameObject ropeJoint in rope.ropeJoints)
        //{
        //    Color color = ropeJoint.GetComponent<LineRenderer>().startColor;
        //    color.a = 0;
        //    ropeJoint.GetComponent<LineRenderer>().startColor = color;
        //    color = ropeJoint.GetComponent<LineRenderer>().endColor;
        //    color.a = 0;
        //    ropeJoint.GetComponent<LineRenderer>().endColor = color;
        //}

        DistanceJoint2D[] cementDispenserDistanceJoints = cementDispenser.GetComponents<DistanceJoint2D>();
        for (int i = 0; i < cementDispenserDistanceJoints.Length; i++)
            cementDispenserDistanceJoints[i].distance = rope.ropeJoints[0].GetComponent<DistanceJoint2D>().distance * rope.GetComponent<Rope>().numberOfJoints;

        cementDispenser.GetComponent<BoxCollider2D>().enabled = true;
        unhideCementDispenser();
		cementDispenser.GetComponent<Glue>().enabled = true;
		//magnetRope.gameObject.SetActive(false);
		//cementDispenser.gameObject.SetActive(true);
		//cementDispenserRope.gameObject.SetActive(true);
	}

    private void hideCementDispenser()
    {

        Color transparentColor = cementDispenser.GetComponent<SpriteRenderer>().color;
        transparentColor.a = 0.0f;
        cementDispenser.GetComponent<SpriteRenderer>().color = transparentColor;

        Color color = dispenserScript.leftRope.startColor;
        color.a = 0;
        dispenserScript.leftRope.startColor = color;
        color = dispenserScript.leftRope.endColor;
        color.a = 0;
        dispenserScript.leftRope.endColor = color;

        color = dispenserScript.rightRope.startColor;
        color.a = 0;
        dispenserScript.rightRope.startColor = color;
        color = dispenserScript.rightRope.endColor;
        color.a = 0;
        dispenserScript.rightRope.endColor = color;
    }

    private void unhideCementDispenser()
    {
        Color transparentColor = cementDispenser.GetComponent<SpriteRenderer>().color;
        transparentColor.a = 1.0f;
        cementDispenser.GetComponent<SpriteRenderer>().color = transparentColor;

        Color color = dispenserScript.leftRope.startColor;
        color.a = 1.0f;
        dispenserScript.leftRope.startColor = color;
        color = dispenserScript.leftRope.endColor;
        color.a = 1.0f;
        dispenserScript.leftRope.endColor = color;

        color = dispenserScript.rightRope.startColor;
        color.a = 1.0f;
        dispenserScript.rightRope.startColor = color;
        color = dispenserScript.rightRope.endColor;
        color.a = 1.0f;
        dispenserScript.rightRope.endColor = color;
    }
}
