using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    #region property
    [Header("Spawn")]
    public int spawnBunnyCost = 10;
    public int spawnSquareCost = 5;
    public int spawnRectangleCost = 5;
    public int spawnTriangleCost = 5;
    public int spawnWeightCost = 10;
    public int eachBlockBPScore = 20;

    [Header("Score")]
    public static int fullScore = 100;
    // Current Score with getter and setter
    private static int currentScore;
    public static int CurrentScore
    {
        get { return currentScore; }
        set { currentScore = value; }
    }
    // score on blue print
    private int BPControl;
    public static int BPScore;
    private static int currentScoreBP;
    public static int CurrentScoreBP
    {
        get { return currentScoreBP; }
        set { currentScoreBP = value; }
    }
    // Score active on build
    public static int skillSum;
    public static int actScore;
    private static int currentScoreAct;
    public static int CurrentScoreAct
    {
        get { return currentScoreAct; }
        set { currentScoreAct = value; }
    }

    public int eachBunnyReward = 50;
    private static int bunnyReward;
    public static int EachBunnyReward
    {
        get { return bunnyReward; }
        set { bunnyReward = value; }
    }

    private static bool blockMomentum;
    public static bool BlockMomentum
    {
        get { return blockMomentum; }
        set { blockMomentum = value; }
    }

    public static int _lskillScore;
    public static int skillScore;
    public static int _lblockInBP;


    // Timer
    private static float currentTime;
    public static float CurrentTime
    {
        get { return currentTime; }
        set { currentTime = value; }
    }
    public static float timeLeftToCompleteLevel;

    public static bool justScored;

    [Header("Disaster")]
    public float disasterDuration = 5f;
    private static bool allowDisaster;
    public static bool AllowDisaster
    {
        get { return allowDisaster; }
        set { allowDisaster = value; }
    }

    public int startDisasterLevel = 0;
    private static int currentDisasterLevel;
    public static int CurrentDisasterLevel
    {
        get { return currentDisasterLevel; }
        set { currentDisasterLevel = value; }
    }

    public float disasterIncreaseRatio = 0.2f;
    private static float disasterRatio;
    public static float DisasterIncreaseRatio
    {
        get { return disasterRatio; }
        set { disasterRatio = value; }
    }

    // 1: build, 2: disaster, 3, score
    //private static int currentStage;
    //public static int CurrentStage
    //{
    //    get { return currentStage; }
    //    set { currentStage = value; }
    //}

    bool loadSceneCoroutineRunning;
    delegate void buildSceneFunc();

    public enum UFOSceneName
    {
        UFOLoadingScene,
        UFOBuildScene,
        UFODisasterScene,
        UFOScoreScene
    }

    public enum MagnetSceneName
    {
        LoadingScene,
        BuildScene,
        DisasterScene,
        ScoreScene,
        WeightScene
    }

    public enum RoboticArmSceneNames
    {
        Loading,
        Build,
        Disaster,
        Score
    }

    public enum GameModes { Magnet, UFO, RoboticArm};
    public static GameModes currentGameMode;

    public static List<GameObject> gameBlocks;

    //UFO level variables
    public static int UFObigBeamCharges;
    public static int UFOsmallBeamCharges;
    public static int UFOfuelCharges;
    public static int UFOglueCharges;

    // Managers
    InputManager inputManager;
    SceneControl sceneControl;
    DisasterManager disasterManager;
    LevelDataManager levelDataManager;
    LevelDataManagerMagnet levelDataManagerMagnet;
    #endregion

    bool paused;
    public static bool isReplay;
    #region Singleton
    // Singleton of GameMaster with GameMaster Instantce Getter
    private static GameMaster gameMaster;
    public static GameMaster GMinstance
    {
        get
        {
            if (!gameMaster)
            {
                gameMaster = FindObjectOfType(typeof(GameMaster)) as GameMaster;
                if (!gameMaster)
                    Debug.LogError("There needs to be one active GameMaster script on a GameObject in your scene.");
                else
                    gameMaster.Init();
            }
            return gameMaster;
        }
    }

    void Init()
    {
        Debug.Log("Game Master initiated!");
    }
    #endregion

    public bool complexRobotMovement;

    #region Monobehavior
    private void Start()
    {
        gameBlocks = new List<GameObject>();
        inputManager = new InputManager(
            spawnBunnyCost,
            spawnSquareCost,
            spawnRectangleCost,
            spawnTriangleCost,
            spawnWeightCost,
            complexRobotMovement
        );
        sceneControl = GetComponent<SceneControl>();
        //CurrentStage = 0;
        currentScore = 0;
        currentScoreAct = actScore;
        currentScoreBP = BPScore;
        allowDisaster = false;
        blockMomentum = false;
        isReplay = false;
        CurrentDisasterLevel = startDisasterLevel;
        DisasterIncreaseRatio = disasterIncreaseRatio;
        EachBunnyReward = eachBunnyReward;
        _lskillScore = 0;
        skillScore = 0;
        _lblockInBP = 0;
        BPControl = 0;
        CurrentTime = disasterDuration;

       levelDataManager = GetComponent<LevelDataManager>();
       levelDataManagerMagnet = GetComponent<LevelDataManagerMagnet>();

        if (SceneManager.GetActiveScene().name.Contains("UFO"))
            currentGameMode = GameModes.UFO;
        else if (SceneManager.GetActiveScene().name.Contains("Robotic"))
            currentGameMode = GameModes.RoboticArm;
        else if (SceneManager.GetActiveScene().name.Contains("Magnet"))
            currentGameMode = GameModes.Magnet;
    }

    private void Update()
    {
		//Automatically skips the title screen (used with MLAgent)
		//automateLanding();

		if (SceneManager.GetActiveScene().name.Contains("Build"))
        {
            //if (Snapper.haveBlock == true || TractorBeam.holdingSomething == true || RobotClaw.holdingBlock == true)
            //    BPControl = 1;
            //if (blockMomentum == false && Snapper.haveBlock == false && TractorBeam.holdingSomething == false && RobotClaw.holdingBlock == false)
            //{
                //Old code
                //skillSum = 0;
                //if (((int)SkillScoreSum.skillScore - _lskillScore) > 4)
                //{
                //    skillScore = (int)SkillScoreSum.skillScore;
                //    skillSum = skillScore - _lskillScore;
                //}
                //else if (((int)SkillScoreSum.skillScore - _lskillScore) < -4)
                //{
                //    skillScore = (int)SkillScoreSum.skillScore;
                //    skillSum = skillScore - _lskillScore;
                //}
                //if ((int)SkillScoreSum.skillScore != _lskillScore)
                //{
                //    _lskillScore = skillScore;
                //    var Base = GameObject.FindGameObjectWithTag("Base");
                //    Base.GetComponent<BaseController>().skillRewardTextSpawn(skillSum);
                //    currentScoreAct += (skillSum);
                //}
                //if (BPControl == 1)
                //{
                //    BPControl = 0;
                    //this is a temporary fix. should find a more reliable way to calculate when block touches ground.
                    //StartCoroutine(calculateBPPoints());
                    //BPControl = 0;
                    //BPIoUCalculation.IoUCalculation();
                    //if (_lblockInBP != (int)BPIoUCalculation.IoU)
                    //{
                    //    int IoU = (int)BPIoUCalculation.IoU;
                    //    var Base = GameObject.FindGameObjectWithTag("Base");
                    //    Base.GetComponent<BaseController>().BPRewardTextSpawn(IoU - _lblockInBP);
                    //    currentScoreBP += IoU - _lblockInBP;
                    //    _lblockInBP = IoU;
                    //}
            //    }
            //}

            if (CurrentScoreBP >= 75)
            {
                if (currentGameMode == GameModes.UFO)
                {
                    sceneControl.LoadUFODisasterScene();
                    //currentStage = (int)UFOSceneName.UFODisasterScene;
                }
                else if (currentGameMode == GameModes.RoboticArm)
                {
					sceneControl.LoadRoboticArmDisasterScene();
                    //CurrentStage = (int)RoboticArmSceneNames.Disaster;
                }
                else
                {
					sceneControl.LoadDisasterScene();
                    //CurrentStage = (int)MagnetSceneName.DisasterScene;
                }
            }

            //terminate the game if out of time. later should make a "Game Over" screen for human players
            //magnet build would shut off until it gets level data that sets the timer. checking the scene for UFO is a temporary fix until that's implemented.
            if (timeLeftToCompleteLevel <= 0.0f && SceneManager.GetActiveScene().name.Contains("Build") && currentGameMode == GameModes.RoboticArm)
                sceneControl.LoadRoboticArmGameOver();
            else if (timeLeftToCompleteLevel <= 0.0f && SceneManager.GetActiveScene().name.Contains("Build"))
                quitGame();

            //if (LevelDataManagerMagnet.currentLevel >= LevelDataManagerMagnet.totalLevels)
            //    sceneControl.LoadRoboticArmGameOver();

            //timeLeftToCompleteLevel -= Time.deltaTime;


            if (currentGameMode == GameModes.Magnet)
                inputManager.CraneInput();



            inputManager.ToolInput();
            //inputManager.SpawnInput(); // Spawn input disabled (Agent shouldn't use it)
            EventManager.TriggerEvent("UpdateScore");
            //EventManager.TriggerEvent("UpdateSkill");
            EventManager.TriggerEvent("UpdateBP");
        }
        else if (SceneManager.GetActiveScene().name.Contains("Disaster")) // or Regular Disaster Scene
        {
            // Update UI
            EventManager.TriggerEvent("UpdateScore");
            EventManager.TriggerEvent("UpdateLevel");
            _lblockInBP = 0;
            _lskillScore = 0;
            skillSum = 0;

            if (!disasterManager)
            {
                disasterManager = GameObject.Find("DisasterManager").GetComponent<DisasterManager>();
                if (currentGameMode == GameModes.UFO)
                    automateDisaster(levelDataManager.currentLevel);
                else if (currentGameMode == GameModes.Magnet)
                    automateDisaster(LevelDataManagerMagnet.currentLevel);
                else if (currentGameMode == GameModes.RoboticArm)
                    automateDisaster(LevelDataManagerMagnet.currentLevel);

            }
            if (AllowDisaster == true)
            {
                // Disaster count down
                EventManager.TriggerEvent("UpdateTimer");
                if (CurrentTime <= 0.0f)
                {
                    // Stop disaster
                    if (currentGameMode == GameModes.RoboticArm && LevelDataManagerMagnet.currentLevel == LevelDataManagerMagnet.totalLevels)
                        sceneControl.LoadRoboticArmGameOver();
                    if (currentGameMode == GameModes.UFO) sceneControl.LoadUFOScoreScene();
                    if (currentGameMode == GameModes.Magnet) sceneControl.LoadScoreScene();
                    if (currentGameMode == GameModes.RoboticArm && LevelDataManagerMagnet.currentLevel < LevelDataManagerMagnet.totalLevels)
                        sceneControl.LoadRoboticArmScoreScene();
                    CurrentTime = 0;
                    AllowDisaster = false;
                    //currentScoreBP = 0;
                    currentScoreAct = 0;
                    CurrentTime = disasterDuration;
                    //EventManager.TriggerEvent("UpdateSkill");
                    //EventManager.TriggerEvent("UpdateBP");
                }
                else
                    CurrentTime -= Time.deltaTime;
                CurrentTime = Mathf.Clamp(CurrentTime, 0.0f, float.MaxValue);
            }
        }


    }

    private void FixedUpdate()
    {
        if (currentGameMode == GameModes.RoboticArm)
            inputManager.RoboticArmInput();
    }

    #endregion

    #region Method
    // Check Score
    public static bool IsEnoughScore(int minus)
    {
        if (CurrentScore - minus < 0)
            return false;
        return true;
    }

    public static bool IsFullScore(int add)
    {
        if (CurrentScore + add > fullScore)
            return true;
        return false;
    }

    public static void ResetScene()
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
        GameObject[] bunnys = GameObject.FindGameObjectsWithTag("Bunny");
        GameObject[] weights = GameObject.FindGameObjectsWithTag("Weight");

        foreach (var block in blocks)
            Destroy(block);
        foreach (var bunny in bunnys)
            Destroy(bunny);
        foreach (var weight in weights)
            Destroy(weight);
        CurrentScoreBP = 0;
        _lblockInBP = 0;
        gameBlocks.Clear();
        Destroy(GameObject.Find("Particles"));
    }

    public void automateDisaster(int currentLevel)
    {
        if (currentLevel < 5)
            disasterManager.EarthquakeTrigger();
        else
            disasterManager.HailTrigger();
    }

    public void automateLanding()
    {
        if (SceneManager.GetActiveScene().name.Contains("Landing"))
        {
            if (currentGameMode == GameModes.Magnet && loadSceneCoroutineRunning == false)
                StartCoroutine(loadSceneAfterXSeconds(sceneControl.LoadBuildScene, 0.1f));
            else if (currentGameMode == GameModes.RoboticArm && loadSceneCoroutineRunning == false)
                StartCoroutine(loadSceneAfterXSeconds(sceneControl.LoadRoboticArmBuildScene, 0.1f));

        }
    }
    #endregion

    IEnumerator calculateBPPoints()
    {
        //wait to let block fall before calculating.
        yield return new WaitForSeconds(1.0f);


        BPIoUCalculation.IoUCalculation();
        if (_lblockInBP != (int)BPIoUCalculation.IoU)
        {
            int IoU = (int)BPIoUCalculation.IoU;
            var Base = GameObject.FindGameObjectWithTag("Base");
            Base.GetComponent<BaseController>().BPRewardTextSpawn(IoU - _lblockInBP);
            currentScoreBP += IoU - _lblockInBP;
            _lblockInBP = IoU;
        }
    }

    IEnumerator loadSceneAfterXSeconds(buildSceneFunc callback, float x)
    {
        loadSceneCoroutineRunning = true;
        yield return new WaitForSeconds(x);
        callback();
        loadSceneCoroutineRunning = false;
    }

    static public void quitGame()
    {
        //Application.Quit doesn't work while testing in editor.
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
