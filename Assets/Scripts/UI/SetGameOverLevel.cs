using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetGameOverLevel : MonoBehaviour
{
    private void Awake()
    {
        setGameOverLevelText();
    }

    public void setGameOverLevelText()
    {
        GetComponent<Text>().text = "You reached level " + LevelDataManagerMagnet.currentLevel;
    }
}
