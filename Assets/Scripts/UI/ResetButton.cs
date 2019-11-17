using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(resetBuildScene);
    }

    public void resetBuildScene()
    {
        LevelDataManagerMagnet.currentLevel -= 1;
        LevelDataManagerMagnet.reset = true;
        GameMaster.ResetScene();
    }
}
