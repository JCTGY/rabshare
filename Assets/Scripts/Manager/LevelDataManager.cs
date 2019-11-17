using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using ProtoBuf;
using System.IO;

public class LevelDataManager : MonoBehaviour
{
    public int currentLevel = 0;
    public List<Level> levelList = new List<Level>();
    public List<GameObject> blueprints = new List<GameObject>();
    public string directoryPath;
    private string[] filesInSaveDir;

    string[] blockSpawnEvents =
    {
        "RectangleSpawn",
        "SquareSpawn",
        "TriangleSpawn",
        "BunnySpawn"
    };

    int[] numberOfBlocksInEvent;

    // Start is called before the first frame update
    void Start()
    {
        directoryPath = Application.dataPath + directoryPath;

        DontDestroyOnLoad(this);

        if (!string.IsNullOrEmpty(directoryPath) && Directory.Exists(directoryPath))
            filesInSaveDir = Directory.GetFiles(directoryPath);

        numberOfBlocksInEvent = new int[blockSpawnEvents.Length];
    }

    private void Update()
    {
        if (Input.GetButtonDown("Save"))
            saveLevels();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Level loaded: " + scene.name);
        if (scene.name.Contains("Build"))
            loadNextLevel();
    }

    //LEVELS MUST BE SAVED AS PROTOBUF FIRST
    public void loadNextLevel()
    {
        //if (currentLevel > filesInSaveDir.Length - 1)
        //    return;

        blueprints[currentLevel].SetActive(true);
        if (currentLevel >= 1)
            blueprints[currentLevel - 1].SetActive(false);

        //if (filesInSaveDir.Length == 0)
        //    return;

        Debug.Log("Loading");

        //Level level = Serializer.Deserialize<Level>(new FileStream(filesInSaveDir[currentLevel], FileMode.Open, FileAccess.ReadWrite));
        Level level = levelList[currentLevel];

        GameMaster.timeLeftToCompleteLevel += level.timeToCompleteLevel; //keep whatever time is left as bonus
        GameMaster.UFObigBeamCharges = level.UFONumberOfBigBeamUses;
        GameMaster.UFOsmallBeamCharges = level.UFONumberOfSmallBeamUses;
        GameMaster.UFOfuelCharges = level.UFOFuelChargeNumber;
        GameMaster.UFOglueCharges = level.UFOGlueUses;

        assignNumberOfBlocks(level);

        StartCoroutine(triggerSpawnBlockEvent(0));


        //probably should make a better way of storing loadout information rather than bools.
        if (level.bigBeamEquipped)
            TractorBeam.bigBeamEquipped = true;
        else
            TractorBeam.bigBeamEquipped = false;

        if (level.triangleShipEnabled)
        {
            GameObject[] UFOs = GameObject.FindGameObjectsWithTag("UFO");
            for (int i = 0; i < UFOs.Length; i++)
            {
                if (UFOs[i].name == "FlipperUFO")
                    UFOs[i].SetActive(true);
            }
        }
        else
        {
            GameObject[] UFOs = GameObject.FindGameObjectsWithTag("UFO");
            for (int i = 0; i < UFOs.Length; i++)
            {
                if (UFOs[i].name == "FlipperUFO")
                    UFOs[i].SetActive(false);
            }
        }

        //levelList[currentLevel] = level;

        currentLevel++;
    }

    //recursively spawns each type of block according to order of blockSpawnEvents
    //recursive coroutine allows each block to spawn one at a time with a short delay in between.
    IEnumerator triggerSpawnBlockEvent(int spawnEventsIdx)
    {
        if (spawnEventsIdx >= blockSpawnEvents.Length)
            yield break;

        for (int i = 0; i < numberOfBlocksInEvent[spawnEventsIdx]; i++)
        {
            EventManager.TriggerEvent(blockSpawnEvents[spawnEventsIdx]);
            yield return new WaitForSeconds(0.07f);
        }

        spawnEventsIdx++;

        StartCoroutine(triggerSpawnBlockEvent(spawnEventsIdx));
    }

    private void saveLevels()
    {
        if (string.IsNullOrEmpty(directoryPath))
            return;
        if (!Directory.Exists(directoryPath))
            return;

        Debug.Log("Saving");

        //for (int i = 0; i < filesInSaveDir.Length; i++)
        //    File.Delete(filesInSaveDir[i]);

        for (int i = 0; i < levelList.Count; i++)
        {
            FileStream stream = new FileStream(Path.Combine(directoryPath, "Level" + i), FileMode.Truncate, FileAccess.Write);
            Serializer.Serialize<Level>(stream, levelList[i]);
        }
    }

    void assignNumberOfBlocks(Level level)
    {
        numberOfBlocksInEvent[0] = level.numberOfRectangleBlocks;
        numberOfBlocksInEvent[1] = level.numberOfSquareBlocks;
        numberOfBlocksInEvent[2] = level.numberOfTriangleBlocks;
        numberOfBlocksInEvent[3] = level.numberOfBunnies;
    }
}

//level data structure. contains any variables that make each level different
[System.Serializable]
[ProtoContract]
public class Level
{
    [ProtoMember(1, DataFormat = DataFormat.FixedSize, IsRequired = true)]
    public float timeToCompleteLevel;

    [ProtoMember(2, DataFormat = DataFormat.FixedSize, IsRequired = true)]
    public int UFONumberOfBigBeamUses;

    [ProtoMember(3, DataFormat = DataFormat.FixedSize, IsRequired = true)]
    public int UFONumberOfSmallBeamUses;

    [ProtoMember(4, DataFormat = DataFormat.FixedSize, IsRequired = true)]
    public int UFOFuelChargeNumber;

    [ProtoMember(5, DataFormat = DataFormat.FixedSize, IsRequired = true)]
    public int UFOGlueUses;

    [ProtoMember(6, DataFormat = DataFormat.FixedSize, IsRequired = true)]
    public int numberOfSquareBlocks;

    [ProtoMember(7, DataFormat = DataFormat.FixedSize, IsRequired = true)]
    public int numberOfRectangleBlocks;

    [ProtoMember(8, DataFormat = DataFormat.FixedSize, IsRequired = true)]
    public int numberOfTriangleBlocks;

    [ProtoMember(9, DataFormat = DataFormat.FixedSize, IsRequired = true)]
    public bool bigBeamEquipped;

    [ProtoMember(10, DataFormat = DataFormat.FixedSize, IsRequired = true)]
    public bool triangleShipEnabled;

    [ProtoMember(11, DataFormat = DataFormat.FixedSize, IsRequired = true)]
    public int numberOfBunnies;
}
