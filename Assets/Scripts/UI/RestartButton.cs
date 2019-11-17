using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void restart()
    {
        GameMaster.ResetScene();
        GameMaster.CurrentDisasterLevel = 1;
        LevelDataManagerMagnet.restartGame = true;
        SceneManager.LoadScene("Robotic Arm Build Scene");
    }
}
