using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using UnityEngine.SceneManagement;

public class RobotArmAgent : Agent
{
    private Transform[] blockTransforms;

    private Rigidbody2D robotClawRB;
    private Rigidbody2D robotClawCartRB;

    private Transform robotClawCartTransform;
    private Transform robotClawIKTargetTransform;

    private Quaternion robotClawBaseBoneRotation;
    private Quaternion robotClawMidBoneRotation;
    private Quaternion robotClawTopBoneRotation;

    private int count = 0;
    private int maxVectorObs = 29;

    private float lastBPScore = 0.0f;

    int maxBlocks = 10;

    bool gotBuildSceneVariables;
    static public bool pickedUpBlock = false;
    static public bool droppedBlockOffBase = false;

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
        GameMaster.ResetScene();
        SceneManager.LoadScene("_Scenes/Robotic Arm Build Scene");

        count = 0;
    }

    // Observation space size: 8
    // 2 + 1 + 2 + 1 + 2 = 8
    public override void CollectObservations()
    {
        //do not collect observations if we are not in the build scene.
        if (SceneManager.GetActiveScene().name.Contains("Build") == false)
        {
            for (int i = 0; i < maxVectorObs; i++)
                AddVectorObs(0.0f);
            return;
        }

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
        // 2. cart position (only need horizontal x position)
        AddVectorObs(robotClawCartTransform.position.x);
        // 3. claw position
        AddVectorObs(robotClawIKTargetTransform.position.x);
        AddVectorObs(robotClawIKTargetTransform.position.y);
        // 4. Bottom bone (bottom third) robot claw rotation
        AddVectorObs(robotClawBaseBoneRotation.eulerAngles.z);
        // 5. Middle bone (second third) robot claw rotation
        AddVectorObs(robotClawMidBoneRotation.eulerAngles.z);
        // 6. Top bone (last third) robot claw rotation
        AddVectorObs(robotClawTopBoneRotation.eulerAngles.z);
        // 7. cart velocity (x)
        AddVectorObs(robotClawCartRB.velocity.x);
        // 8. robot arm velocity (x and y)
        AddVectorObs(robotClawRB.velocity);
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
                EventManager.TriggerEvent("ArmMoveUp");
                break;
            case 2:
                EventManager.TriggerEvent("ArmMoveDown");
                break;
            case 3:
                EventManager.TriggerEvent("ArmMoveRight");
                break;
            case 4:
                EventManager.TriggerEvent("ArmMoveLeft");
                break;
            case 5:
                EventManager.TriggerEvent("CartMoveRight");
                break;
            case 6:
                EventManager.TriggerEvent("CartMoveLeft");
                break;
            case 7:
                EventManager.TriggerEvent("Use");
                break;
        }

        //get a reward for picking up block first time
        if (pickedUpBlock && robotClawRB.gameObject.GetComponentInChildren<RobotClaw>().blockTransform.GetComponent<BlockController>().beenPickedUp == false)
        {
            robotClawRB.gameObject.GetComponentInChildren<RobotClaw>().blockTransform.GetComponent<BlockController>().beenPickedUp = true;
            pickedUpBlock = false;
            AddReward(1.0f);
        }

        //end episode with no reward if block is dorpped off base
        if (droppedBlockOffBase)
        {
            droppedBlockOffBase = false;
            Done();
        }

        //get reward for dropping block on base. get higher reward for dropping on blueprint. episode finishes and resets after dropping block
        if (GameMaster.justScored)
        {
            GameMaster.justScored = false;
            float dropBlockReward = GameMaster.CurrentScoreBP - lastBPScore;
            if (dropBlockReward <= 0.0f)
                AddReward(0.1f);
            else
                AddReward(dropBlockReward);
            lastBPScore = GameMaster.CurrentScoreBP;
            Done();
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

    private IEnumerator setCamerasForVisualObs()
    {
        //suspend function until BluePrintCamera is active in scene
        yield return new WaitUntil(() => GameObject.Find(" BluePrintCamera") != null);
        List<Camera> cameras = new List<Camera>();

        //cameras.Add(GameObject.Find("Main Camera").GetComponent<Camera>());
        cameras.Add(GameObject.Find(" BluePrintCamera").GetComponent<Camera>());
        cameras.Add(GameObject.Find(" BlocksCamera").GetComponent<Camera>());

        this.agentParameters.agentCameras = cameras;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name.Contains("Build") && !gotBuildSceneVariables)
        {
            robotClawCartTransform = GameObject.Find("/ClawArm/CartForClaw/body").GetComponent<Transform>();
            robotClawIKTargetTransform = GameObject.Find("/ClawArm/PrototypeArmWithClaw/New FabrikSolver2D/robotClawIKTarget").GetComponent<Transform>();

            robotClawBaseBoneRotation = GameObject.Find("/ClawArm/PrototypeArmWithClaw/bone_1").GetComponent<Transform>().rotation;
            robotClawMidBoneRotation = GameObject.Find("/ClawArm/PrototypeArmWithClaw/bone_1/bone_3").GetComponent<Transform>().rotation;
            robotClawTopBoneRotation = GameObject.Find("/ClawArm/PrototypeArmWithClaw/bone_1/bone_3/bone_5").GetComponent<Transform>().rotation;

            robotClawCartRB = robotClawCartTransform.gameObject.GetComponent<Rigidbody2D>();
            robotClawRB = GameObject.Find("/ClawArm/PrototypeArmWithClaw").GetComponent<Rigidbody2D>();

            getBlockTransforms();
            StartCoroutine(setCamerasForVisualObs());
            gotBuildSceneVariables = true;
        }
        else if (!SceneManager.GetActiveScene().name.Contains("Build") && gotBuildSceneVariables)
            gotBuildSceneVariables = false;
    }
}
