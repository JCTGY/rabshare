using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class CraneAgent : Agent
{
    public GameObject block;
    public GameObject magnet;
    public Rope ropeScript;

    private Rigidbody2D magnetRb2d;
    private Rigidbody2D craneRb2d;

    private Transform craneTransform;
    private Transform blockTransform;
    private Transform magnetTransform;

    private Vector3 craneResetPosition;
    private Vector3 magnetResetPosition;
    private Vector3 blockResetPosition;

    private int count = 0;


    void Start()
    {
        // Get Rb for velocity
        craneRb2d = GetComponent<Rigidbody2D>();
        magnetRb2d = magnet.GetComponent<Rigidbody2D>();

        // Get transform for Obs
        craneTransform = GetComponent<Transform>();
        blockTransform = block.GetComponent<Transform>();
        magnetTransform = magnet.GetComponent<Transform>();

        // record start position
        craneResetPosition = craneTransform.position;
        magnetResetPosition = magnetTransform.position;
        blockResetPosition = blockTransform.position;
    }

    public override void AgentReset()
    {
        // Turn off magnet
        if (Snapper.magnetOn == true)
        {
            EventManager.TriggerEvent("Use");
            Snapper.magnetOn = false;
        }
        // Move crane back to start position
        craneTransform.position = craneResetPosition;
        // Reset rope
        //ropeScript.resetRope();
        // Reset magnet and block
        magnetTransform.position = magnetResetPosition;
        blockTransform.position = blockResetPosition;
        count = 0;
    }

    // Observation space size: 8
    // 2 + 1 + 2 + 1 + 2 = 8
    public override void CollectObservations()
    {
        // Observation:
        // 1. Block position
        AddVectorObs(blockTransform.position.x);
        AddVectorObs(blockTransform.position.y);
        // 2. crane position
        AddVectorObs(craneTransform.position.x);
        // 3. magnet position
        AddVectorObs(magnetTransform.position.x);
        AddVectorObs(magnetTransform.position.y);
        // 4. crane velocity x
        AddVectorObs(craneRb2d.velocity.x);
        // 5. magnet velocity
        AddVectorObs(magnetRb2d.velocity);
    }

    // Make action discrete space size: 6
    // 0: not move 1~4: wsad 5: space
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        var movement = (int)vectorAction[0];

        count++;
        //Debug.Log("move: " + movement);
        switch (movement)
        {
            case 1:
                EventManager.TriggerEvent("RopeMoveUp");
                break;
            case 2:
                EventManager.TriggerEvent("RopeMoveDown");
                break;
            case 3:
                EventManager.TriggerEvent("CraneMoveRight");
                break;
            case 4:
                EventManager.TriggerEvent("CraneMoveLeft");
                break;
            case 5:
                EventManager.TriggerEvent("Use");
                break;
        }

        // Easiest goal: pick up a block
        if (Snapper.haveBlock == true)
        {
            Done();
            AddReward(1f);
        }
    }

    private void Update()
    {
        //Debug.Log("Step: " + count);
        //Debug.Log("craneV: (" + craneRb2d.velocity + ")");
        //Debug.Log("mag: (" + magnetRb2d.velocity + ")");
        //Debug.Log("magnetResetPosition: " + magnetResetPosition);
        //if (Snapper.haveBlock)
            //Debug.Log("haveBlock: " + Snapper.haveBlock);
    }
}
