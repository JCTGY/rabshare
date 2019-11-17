using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControl : MonoBehaviour
{
    public int eachBunnyReward = 100;
    public int eachWeightReward = 5;
    private float timerNextScene;

    // gamemode for next scene
    private static string gameMode;
    public static string GameMode
    {
        get { return gameMode; }
        set { gameMode = value; }
    }

    // Scene Management
    public void LoadBuildScene()
    {
        SceneManager.LoadScene("_Scenes/Build Scene");
        //EventManager.TriggerEvent("BunnyOnBuild");
        //GameMaster.CurrentStage = 1;
        gameMode = "bunny";
    }

	public void LoadUFOBuildScene()
	{
		SceneManager.LoadScene("_Scenes/UFO Build Scene");
        //EventManager.TriggerEvent("BunnyOnBuild");
        //GameMaster.CurrentStage = 1;
		gameMode = "bunny";
	}

    public void LoadRoboticArmBuildScene()
    {
        SceneManager.LoadScene("_Scenes/Robotic Arm Build Scene");
        //EventManager.TriggerEvent("BunnyOnBuild");
        //GameMaster.CurrentStage = 1;
        gameMode = "bunny";
    }

    public void LoadDisasterScene()
    {
        // Start Disaster
        SceneManager.LoadScene("_Scenes/Disaster Scene");
        //EventManager.TriggerEvent("BunnyOnDisaster");
        //EventManager.TriggerEvent("DisableDisasterbutton");
        //GameMaster.CurrentStage = 2;
        GameMaster.CurrentDisasterLevel += 1;
    }

    public void LoadUFODisasterScene()
    {
        // Start Disaster
        SceneManager.LoadScene("_Scenes/UFO Disaster Scene");
        //EventManager.TriggerEvent("BunnyOnDisaster");
        //EventManager.TriggerEvent("DisableDisasterbutton");
        //GameMaster.CurrentStage = 2;
        GameMaster.CurrentDisasterLevel += 1;
    }

    public void LoadRoboticArmDisasterScene()
    {
        // Start Disaster
        SceneManager.LoadScene("_Scenes/Robotic Arm Disaster Scene");
        //EventManager.TriggerEvent("BunnyOnDisaster");
        //EventManager.TriggerEvent("DisableDisasterbutton");
        //GameMaster.CurrentStage = 2;
        GameMaster.CurrentDisasterLevel += 1;
    }

    public void LoadRoboticArmReplayScene()
    {
        // Start Disaster
        SceneManager.LoadScene("_Scenes/Robotic Arm Replay Scene");
    }
    public void LoadScoreScene()
    {
        SceneManager.LoadScene("_Scenes/Score Scene");
        //EventManager.TriggerEvent("BunnyOnBuild");
        //GameMaster.CurrentStage = 3;
        StartCoroutine(ScoreCoroutine());
    }

    public void LoadUFOScoreScene()
    {
        SceneManager.LoadScene("_Scenes/UFO Score Scene");
        //EventManager.TriggerEvent("BunnyOnBuild");
        //GameMaster.CurrentStage = 3;
        StartCoroutine(ScoreCoroutine());
    }

    public void LoadRoboticArmScoreScene()
    {
        SceneManager.LoadScene("_Scenes/Robotic Arm Score Scene");
        //EventManager.TriggerEvent("BunnyOnBuild");
        //GameMaster.CurrentStage = 3;
        StartCoroutine(ScoreCoroutine());
    }

    public void LoadRoboticArmGameOver()
    {
        SceneManager.LoadScene("_Scenes/Robotic Arm Game Over");
        StartCoroutine(ScoreCoroutine());
    }

    public void LoadWeightScene()
    {
        SceneManager.LoadScene("_Scenes/Weight Scene");
        EventManager.TriggerEvent("BunnyOnBuild");
        //GameMaster.CurrentStage = 4;
        gameMode = "weight";
    }

    public void LoadNextScene()
    {
        if (GameMaster.currentGameMode == GameMaster.GameModes.UFO)
            LoadUFOBuildScene();
        else if (GameMaster.currentGameMode == GameMaster.GameModes.RoboticArm)
            LoadRoboticArmBuildScene();
        else if (gameMode == "bunny")
            LoadBuildScene();
        else if (gameMode == "weight")
            LoadWeightScene();
    }
    // Coroutine
    IEnumerator ScoreCoroutine()
    {
        int BunnyRemain = 0;

        yield return new WaitForSeconds(0.5f);
        if (GameObject.FindGameObjectWithTag("Block") != null)
        {
            Debug.Log("no blocks");
            while (GameMaster.BlockMomentum == true)
                yield return new WaitForSeconds(1);
        }
        if (GameMode == "bunny")
        {
            // Count survive bunnies
            foreach (var SurviveBunnies in GameObject.FindGameObjectsWithTag("Bunny"))
            {
                BunnyRemain += 1;
                SurviveBunnies.GetComponent<BunnyController>().RewardTextSpawn();
            }
            GameMaster.CurrentScore += GameMaster.EachBunnyReward * BunnyRemain;
        }
        else if (GameMode == "weight")
        {
            // count Score of weight
            foreach (var SurvieWeights in GameObject.FindGameObjectsWithTag("Weight"))
            {
                string weightName = SurvieWeights.name;
                int weightMassScale = 1;

                switch (weightName)
                {
                    case "weightPrefab30":
                        weightMassScale = 2;
                        break;
                    case "weightPrefab60":
                        weightMassScale = 4;
                        break;
                    case "weightPrefab90":
                        weightMassScale = 6;
                        break;
                }
                float positionWeight = SurvieWeights.transform.position.y + 5.0f;
                int scoreByWeight = (int)(eachWeightReward * positionWeight) * weightMassScale;
                SurvieWeights.GetComponent<WeightController>().RewardTextSpawn(scoreByWeight);
                GameMaster.CurrentScore += scoreByWeight;
            }
        }
        EventManager.TriggerEvent("UpdateScore");

        yield return new WaitUntil(() => ScoreBoardEvent.scoring == false);

        if (SceneManager.GetActiveScene().name.Contains("Score"))
            setScoreSceneButtons();
        else
            setGameOverButtons();

    }

    void setScoreSceneButtons()
    {
        GameObject NextButton = null;
        // Find NextButton
        GameObject[] buttons = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (var button in buttons)
        {
            if (button.name.Equals("NextButton"))
            {
                // GameMaster.ResetScene();
                NextButton = button;
                break;
            }
        }
        if (NextButton != null)
        {
            // Activate Button, Assign button event
            NextButton.SetActive(true);
            if (GameMaster.currentGameMode == GameMaster.GameModes.UFO)
                NextButton.GetComponent<Button>().onClick.AddListener(LoadUFOBuildScene);
            else if (GameMaster.currentGameMode == GameMaster.GameModes.RoboticArm)
                NextButton.GetComponent<Button>().onClick.AddListener(LoadRoboticArmBuildScene);
            else if (GameMaster.currentGameMode == GameMaster.GameModes.Magnet && gameMode == "bunny")
                NextButton.GetComponent<Button>().onClick.AddListener(LoadBuildScene);
            else if (GameMaster.currentGameMode == GameMaster.GameModes.Magnet && gameMode == "weight")
                NextButton.GetComponent<Button>().onClick.AddListener(LoadWeightScene);
            NextButton.GetComponent<Button>().onClick.AddListener(GameMaster.ResetScene);
            //yield return new WaitForSeconds(0.5f);

            //Automates the Scoring Scene
            //LoadNextScene();
            //GameMaster.ResetScene();
        }
        else
            Debug.Log("Next Button Not Found");
    }

   void setGameOverButtons()
    {
        //GameObject QuitButton = null;
        //GameObject RestartButton = null;
        GameObject FeedBack = null;
        GameObject Name = null;
        GameObject Send = null;

        // Find NextButton
        GameObject[] buttons = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (var button in buttons)
        {
            //if (button.name.Equals("QuitButton"))
            //    QuitButton = button;
            //if (button.name.Equals("RestartButton"))
            //    RestartButton = button;
            //if (QuitButton != null && RestartButton != null)
            //    break;
            if (button.name.Equals("FeedBack"))
                FeedBack = button;
            if (button.name.Equals("Name"))
                Name = button;
            if (button.name.Equals("Send"))
                Send = button;
            if (Send != null && Name != null && FeedBack != null)
                break;
        }
        //if (QuitButton != null && RestartButton != null)
        if (Send != null && Name != null && FeedBack != null)
        {
            // Activate Button, Assign button event
            //QuitButton.SetActive(true);
            //RestartButton.SetActive(true);
            FeedBack.SetActive(true);
            Name.SetActive(true);
            Send.SetActive(true);
            //if (GameMaster.currentGameMode == GameMaster.GameModes.UFO)
            //    NextButton.GetComponent<Button>().onClick.AddListener(LoadUFOBuildScene);
            //else if (GameMaster.currentGameMode == GameMaster.GameModes.RoboticArm)
            //    NextButton.GetComponent<Button>().onClick.AddListener(LoadRoboticArmBuildScene);
            //else if (GameMaster.currentGameMode == GameMaster.GameModes.Magnet && gameMode == "bunny")
            //    NextButton.GetComponent<Button>().onClick.AddListener(LoadBuildScene);
            //else if (GameMaster.currentGameMode == GameMaster.GameModes.Magnet && gameMode == "weight")
            //    NextButton.GetComponent<Button>().onClick.AddListener(LoadWeightScene);
            //NextButton.GetComponent<Button>().onClick.AddListener(GameMaster.ResetScene);
            //yield return new WaitForSeconds(0.5f);

            //Automates the Scoring Scene
            //LoadNextScene();
            //GameMaster.ResetScene();
        }
        else
            Debug.Log("Game Over Buttons Not Found");
    }
}
