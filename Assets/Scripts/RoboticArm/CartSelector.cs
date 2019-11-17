using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CartSelector : MonoBehaviour
{
    public GameObject clawArm;
    public GameObject pickupArm;

    public string controlMode;

    MeshRenderer[] clawCartRenderers;
    MeshRenderer[] pickupCartRenderers;

    Collider2D[] clawArmColliders;
    Collider2D[] pickupArmColliders;

    List<int> originalClawArmLayers = new List<int>();
    List<int> originalPickupArmLayers = new List<int>();

    List<Color> originalClawColors = new List<Color>();
    List<Color> originalFlipperColors = new List<Color>();

    RobotArmMover pickupMover;
    RobotArmMover clawMover;

    RobotArmRotationalMover complexPickupMover;
    RobotArmRotationalMover complexClawMover;

    CartMover clawCartMover;
    CartMover pickupCartMover;

    Color onColor;
    Color offColor;

    enum RobotSelections { Claw, Pickup};

    RobotSelections currentRobotArm = RobotSelections.Claw;

    // Start is called before the first frame update
    void Start()
    {
        onColor = Color.white;
        offColor = Color.grey;

        clawCartRenderers = clawArm.GetComponentsInChildren<MeshRenderer>();
        pickupCartRenderers = pickupArm.GetComponentsInChildren<MeshRenderer>();

        initializeCartColors();

        foreach (MeshRenderer mr in clawCartRenderers)
            originalClawColors.Add(mr.material.color);

        foreach (Collider2D col in pickupArmColliders)
        {
            if (col.gameObject.name == "FlipperCollider") continue;
            col.gameObject.layer = LayerMask.NameToLayer("DisabledRobot");
        }


        if (controlMode == "Complex")
        {
            complexPickupMover = pickupArm.GetComponentInChildren<RobotArmRotationalMover>();
            complexClawMover = clawArm.GetComponentInChildren<RobotArmRotationalMover>();
            complexPickupMover.enabled = false;
        }
        else
        {
            pickupMover = pickupArm.GetComponent<RobotArmMover>();
            clawMover = clawArm.GetComponent<RobotArmMover>();
            pickupMover.enabled = false;
        }

        clawCartMover = clawArm.GetComponentInChildren<CartMover>();
        pickupCartMover = pickupArm.GetComponentInChildren<CartMover>();
        pickupCartMover.enabled = false;
    }

    private void OnEnable()
    {
        clawArmColliders = clawArm.GetComponentsInChildren<Collider2D>();
        pickupArmColliders = pickupArm.GetComponentsInChildren<Collider2D>();

        foreach (Collider2D col in clawArmColliders)
            originalClawArmLayers.Add(col.gameObject.layer);

        foreach (Collider2D col in pickupArmColliders)
            originalPickupArmLayers.Add(col.gameObject.layer);

        ignoreCartCollision();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("SwitchRobot") && currentRobotArm == RobotSelections.Claw)
        {
            if (controlMode == "Complex")
            {
                complexPickupMover.enabled = true;
                complexClawMover.enabled = false;
                pickupCartMover.enabled = true;
                clawCartMover.enabled = false;
            }
            else
            {
                pickupMover.enabled = true;
                clawMover.enabled = false;
                pickupCartMover.enabled = true;
                clawCartMover.enabled = false;
            }

            foreach (MeshRenderer mr in clawCartRenderers)
                mr.material.color = Color.grey;

            foreach (Collider2D col in clawArmColliders)
                col.gameObject.layer = LayerMask.NameToLayer("DisabledRobot");

            for (int i = 0; i < pickupCartRenderers.Length; i++)
                pickupCartRenderers[i].material.color = originalFlipperColors[i];

            for (int i = 0; i < pickupArmColliders.Length; i++)
                pickupArmColliders[i].gameObject.layer = originalPickupArmLayers[i];


            currentRobotArm = RobotSelections.Pickup;
        }
        else if (Input.GetButtonDown("SwitchRobot") && currentRobotArm == RobotSelections.Pickup)
        {
            if (controlMode == "Complex")
            {
                complexPickupMover.enabled = false;
                complexClawMover.enabled = true;
                pickupCartMover.enabled = false;
                clawCartMover.enabled = true;
            }
            else
            {
                pickupMover.enabled = false;
                clawMover.enabled = true;
                pickupCartMover.enabled = false;
                clawCartMover.enabled = true;
            }

            for (int i = 0; i < clawCartRenderers.Length; i++)
                clawCartRenderers[i].material.color = originalClawColors[i];

            for (int i = 0; i < clawArmColliders.Length; i++)
                clawArmColliders[i].gameObject.layer = originalClawArmLayers[i];

            foreach (MeshRenderer mr in pickupCartRenderers)
                mr.material.color = Color.grey;

            foreach (Collider2D col in pickupArmColliders)
            {
                if (col.gameObject.name == "FlipperCollider") continue;
                col.gameObject.layer = LayerMask.NameToLayer("DisabledRobot");
            }



            currentRobotArm = RobotSelections.Claw;
        }
    }

    void ignoreCartCollision()
    {
        //Collider2D[] clawCartColliders = clawCart.GetComponentsInChildren<Collider2D>();
        //Collider2D[] pickupCartColliders = pickupCart.GetComponentsInChildren<Collider2D>();


        //ignores collision between both carts
        //foreach (Collider2D clawCol in clawCartColliders)
        //{
        //    foreach (Collider2D pickupCol in pickupCartColliders)
        //        Physics2D.IgnoreCollision(clawCol, pickupCol);
        //}

        //ignores collision between claw arm and pickup cart
        foreach (Collider2D clawArmCol in clawArmColliders)
        {
            foreach (Collider2D pickupArmCol in pickupArmColliders)
                Physics2D.IgnoreCollision(clawArmCol, pickupArmCol);
        }

        //ignores collision between pickup arm and claw cart
        foreach (Collider2D pickupArmCol in pickupArmColliders)
        {
            foreach (Collider2D clawArmCol in clawArmColliders)
                Physics2D.IgnoreCollision(pickupArmCol, clawArmCol);
        }
    }


    //initializes clawCart to OnColor and pickupCart to offColor
    //also deletes the outline sprites that don't need to change colors.
    void initializeCartColors()
    {
        int i = 0;
        while (i < clawCartRenderers.Length)
        {
            clawCartRenderers[i].sortingOrder = 3;
            clawCartRenderers[i].sortingLayerName = "Foreground";
            if (clawCartRenderers[i].gameObject.name.Contains("Outline"))
            {
                clawCartRenderers[i] = clawCartRenderers[clawCartRenderers.Length - 1];
                Array.Resize(ref clawCartRenderers, clawCartRenderers.Length - 1);
            }
            else
                i++;
        }
        i = 0;
        while (i < pickupCartRenderers.Length)
        {
            pickupCartRenderers[i].sortingOrder = 1;
            if (pickupCartRenderers[i].gameObject.name.Contains("Outline"))
            {
                pickupCartRenderers[i] = pickupCartRenderers[pickupCartRenderers.Length - 1];
                Array.Resize(ref pickupCartRenderers, pickupCartRenderers.Length - 1);
            }
            else
            {
                originalFlipperColors.Add(pickupCartRenderers[i].material.color);
                pickupCartRenderers[i].material.color = Color.grey;
                i++;
            }
        }
    }
}
