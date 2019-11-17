using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using UnityEngine.SceneManagement;

public class MagnetAgent : Agent
{
    private Rigidbody2D magnetRb2d;
    private Rigidbody2D craneRb2d;

    private Transform craneTransform;
    private Transform[] blockTransforms;
    private Transform magnetTransform;

    private Vector3 craneResetPosition;
    private Vector3 magnetResetPosition;
    private Vector3 blockResetPosition;

    private int count = 0;

    private float lastBPScore = 0.0f;

    int maxBlocks = 10;

    bool gotBuildSceneVariables;

    void Start()
    {
        //// Get Rb for velocity
        //craneRb2d = GetComponent<Rigidbody2D>();
        //magnetRb2d = magnet.GetComponent<Rigidbody2D>();

        //// Get transform for Obs
        //craneTransform = GetComponent<Transform>();
        //blockTransform = block.GetComponent<Transform>();
        //magnetTransform = magnet.GetComponent<Transform>();

        //// record start position
        //craneResetPosition = craneTransform.position;
        //magnetResetPosition = magnetTransform.position;
        //blockResetPosition = blockTransform.position;
    }

    public override void AgentReset()
    {
        //old reset. might be necessary if we stay in one scene

        // Turn off magnet
        //if (Snapper.magnetOn == true)
        //{
        //    EventManager.TriggerEvent("Use");
        //    Snapper.magnetOn = false;
        //}
        //// Move crane back to start position
        //craneTransform.position = craneResetPosition;
        //// Reset rope
        ////ropeScript.resetRope();
        //// Reset magnet and block
        //magnetTransform.position = magnetResetPosition;
        //blockTransform.position = blockResetPosition;

        LevelDataManagerMagnet.currentLevel = 0;
        SceneManager.LoadScene("Landing Scene");

        count = 0;
    }

    // Observation space size: 8
    // 2 + 1 + 2 + 1 + 2 = 8
    public override void CollectObservations()
    {
        //do not collect observations if we are not in the build scene.
        if (SceneManager.GetActiveScene().name.Contains("Build") == false)
            return;

        // Observation:
        // 1. 10 Block positions
        for (int i = 0; i < maxBlocks; i++)
        {
            if (blockTransforms[i] == null)
            {
                AddVectorObs(0.0f);
                AddVectorObs(0.0f);
            }
            else
            {
                AddVectorObs(blockTransforms[i].position.x);
                AddVectorObs(blockTransforms[i].position.y);
            }
        }
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

        if (GameMaster.justScored)
        {
            GameMaster.justScored = false;
            AddReward(Mathf.Clamp((GameMaster.CurrentScoreBP - lastBPScore) / 100.0f, 0.0f, float.MaxValue));
            lastBPScore = GameMaster.CurrentScoreBP;
        }

        //Episode is done when levels 1 is complete.
        //**this might be a bit clunky. would be better to eventually stop at the scoring scene.
        if (LevelDataManagerMagnet.currentLevel > 1)
            Done();
    }

    private void getBlockTransforms()
    {
        blockTransforms = new Transform[maxBlocks];
        int i = 0;

        foreach (GameObject block in GameMaster.gameBlocks)
        {
            blockTransforms[i] = block.GetComponent<Transform>();
            i++;
        }

        for (; i < maxBlocks; i++)
            blockTransforms[i] = null;
    }

    private void setCamerasForVisualObs()
    {
        List<Camera> cameras = new List<Camera>();

        cameras.Add(GameObject.Find("Main Camera").GetComponent<Camera>());
        cameras.Add(GameObject.Find(" BluePrintCamera").GetComponent<Camera>());
        cameras.Add(GameObject.Find(" BlocksCamera").GetComponent<Camera>());

        this.agentParameters.agentCameras = cameras;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name.Contains("Build") && !gotBuildSceneVariables)
        {
            magnetTransform = GameObject.Find("Magnet").GetComponent<Transform>();
            craneTransform = GameObject.Find("Crane").GetComponent<Transform>();
            magnetRb2d = magnetTransform.gameObject.GetComponent<Rigidbody2D>();
            craneRb2d = craneTransform.gameObject.GetComponent<Rigidbody2D>();
            getBlockTransforms();
            setCamerasForVisualObs();
            gotBuildSceneVariables = true;
        }
        else if (!SceneManager.GetActiveScene().name.Contains("Build") && gotBuildSceneVariables)
            gotBuildSceneVariables = false;
    }
}
