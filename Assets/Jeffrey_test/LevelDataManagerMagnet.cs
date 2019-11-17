using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDataManagerMagnet : MonoBehaviour
{
    public static int currentLevel = 0;
	public static int totalLevels;
    public static bool reset;
    public static bool restartGame;
    public List<LevelMagnet> levelList = new List<LevelMagnet>();
    public List<GameObject> blueprints = new List<GameObject>();

    string[] blockSpawnEvents = { "SquareSpawn", "RectangleSpawn", "TriangleSpawn", "BunnySpawn"};
    int[] numberOfBlocksPerType;

    // Start is called before the first frame update
    void Start()
    {
        numberOfBlocksPerType = new int[blockSpawnEvents.Length];
        DontDestroyOnLoad(this);
		totalLevels = blueprints.Count;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += _OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= _OnLevelFinishedLoading;
    }

    void _OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (restartGame)
        {
            restartGame = false;
            currentLevel = 0;
            resetBlueprints();
        }
        if (currentLevel == totalLevels)
            return;
        if (scene.name.Contains("Build") || scene.name.Contains("Weight"))
            _loadNextLevel();
    }

    public void _loadNextLevel()
    {
        blueprints[currentLevel].SetActive(true);
        if (currentLevel >= 1)
            blueprints[currentLevel - 1].SetActive(false);
        LevelMagnet level = levelList[currentLevel];
        if (reset)
            reset = false;
        else
            GameMaster.timeLeftToCompleteLevel = level._timeToCompleteLevel; //add to keep whatever time is left as bonus
        assignNumberOfBlocks(level);
        StartCoroutine(triggerSpawnBlockEvent(0));
        currentLevel++;
    }


    //recursively spawns each type of block according to order of blockSpawnEvents
    //recursive coroutine allows each block to spawn one at a time with a short delay in between.
    IEnumerator triggerSpawnBlockEvent(int spawnEventsIdx)
    {
        if (spawnEventsIdx >= blockSpawnEvents.Length)
            yield break;

        for (int i = 0; i < numberOfBlocksPerType[spawnEventsIdx]; i++)
        {
            EventManager.TriggerEvent(blockSpawnEvents[spawnEventsIdx]);
            yield return new WaitForSeconds(0.07f);
        }

        spawnEventsIdx++;

        StartCoroutine(triggerSpawnBlockEvent(spawnEventsIdx));
    }

    void assignNumberOfBlocks(LevelMagnet level)
    {
        numberOfBlocksPerType[0] = level.numberOfSquareBlocks;
        numberOfBlocksPerType[1] = level.numberOfRectangleBlocks;
        numberOfBlocksPerType[2] = level.numberOfTriangleBlocks;
        numberOfBlocksPerType[3] = level.numberOfBunnies;
    }

    public void resetBlueprints()
    {
        foreach (GameObject blueprint in blueprints)
            blueprint.SetActive(false);
    }
}

//level data structure.contains any variables that make each level different
[System.Serializable]
public class LevelMagnet
{
    public float _timeToCompleteLevel;
    public int numberOfSquareBlocks;
    public int numberOfRectangleBlocks;
    public int numberOfTriangleBlocks;
    public int numberOfBunnies;
}