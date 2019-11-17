using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadProtoData : MonoBehaviour
{

    public RobotArmRotationalMover RobotClawMover;
    public RobotArmRotationalMover RobotStickMover;
    public GameObject RobotClawBase;
    public GameObject RobotStickBase;
    public RobotClawRotation RobotClawControler;
    public static ProtoGameDetail loadGameDetail = new ProtoGameDetail();

    /// <summary>
    /// use switchScene to break each level from Build Scene to Disaster Scene
    /// </summary>
    private bool switchScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (SceneManager.GetActiveScene().name.Contains("Build") && GameMaster.isReplay == true && loadGameDetail.switchScene[0] == false)
        {
            LoadProtoFrame();
            switchScene = false;
        }
        else if (SceneManager.GetActiveScene().name.Contains("Build") && GameMaster.isReplay == true && loadGameDetail.switchScene[0] == true && switchScene == false)
        {
            switchScene = true;
            loadGameDetail.switchScene.RemoveAt(0);
            GameObject.Find("GameMaster").GetComponent<SceneControl>().LoadRoboticArmDisasterScene();
        }
    }

    /// <summary>
    /// Load replay on different parts of the gameobjects
    /// </summary>
    public void LoadProtoFrame()
    {
        if (RobotClawBase == null || RobotStickBase == null)
            return;
        loadGameDetail.switchScene.RemoveAt(0);
        LoadRobotBase();
    }

    /// <summary>
    /// Load Robot base movement both stick and the claw
    /// </summary>
    void LoadRobotBase()
    {
        if (loadGameDetail.ClawBodyPosition.Count == 0 || loadGameDetail.StickBodyPosition.Count == 0)
        {
            return;
        }
        float robotClawBaseX = loadGameDetail.ClawBodyPosition[0], robotClawBaseY = loadGameDetail.ClawBodyPosition[1], robotClawBaseZ = loadGameDetail.ClawBodyPosition[2];
        float robotStickBaseX = loadGameDetail.StickBodyPosition[0], robotStickBaseY = loadGameDetail.StickBodyPosition[1], robotStickBaseZ = loadGameDetail.StickBodyPosition[2];

        loadGameDetail.ClawBodyPosition.RemoveRange(0, 3);
        loadGameDetail.StickBodyPosition.RemoveRange(0, 3);

        RobotClawBase.transform.position = new Vector3(robotClawBaseX, robotClawBaseY, robotClawBaseZ);
        RobotStickBase.transform.position = new Vector3(robotStickBaseX, robotStickBaseY, robotStickBaseZ);
    }
}
