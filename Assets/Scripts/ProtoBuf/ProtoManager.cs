using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;

using System.Runtime.InteropServices;

/// <summary>
/// ProtoManager to Manage GameObject Save and Load data
///
/// </summary>
public class ProtoManager : MonoBehaviour
{
    /// <summary>
    ///  Input the GameObject Want to save and load
    /// </summary>
    public static ProtoGameDetail gameDetail;

    public RobotArmRotationalMover RobotClawMover;
    public RobotArmRotationalMover RobotStickMover;
    public GameObject RobotClawBase;
    public GameObject RobotStickBase;
    public RobotClawRotation RobotClawControler;

    public bool PressQ;
    public bool PressA;
    public bool PressW;
    public bool PressS;
    public bool PressE;
    public bool PressD;
    public bool PressRightArrow;
    public bool PressLeftArrow;
    public bool PressTab;
    public bool PressSpace;

    public GameObject[] Bunnies;
    private bool addSwitchScene;

    // Start is called before the first frame update
    void Start()
	{
        // Wait util the game start and assign GameObject rope to <param = rope>
        //StartCoroutine(delayStart());
        foreach (var path in LoadManager.fileNames)
        {
            Debug.Log(path);
        }
        if (gameDetail == null)
            gameDetail = new ProtoGameDetail();
        Bunnies = GameObject.FindGameObjectsWithTag("Bunny");
	}

	// Update is called once per frame
	void Update()
	{
        if (SceneManager.GetActiveScene().name.Contains("Build") && GameMaster.isReplay == false)
        {
            SaveProtoFrame();
        }
        else if (addSwitchScene == false && GameMaster.isReplay == false)
        {
            addSwitchScene = true;
            gameDetail.switchScene.Add(true);
        }
	}

    /// <summary>
    /// Main function for Recording different parts of the game
    /// robotClawSave(): save both stick and claw robot position
    /// objectsave(): save all the bunnies and blocks
    /// KeyPressSave(): save all the input
    /// CurrentScore: add current score
    /// CurrentBPScore: add blueprint score
    /// </summary>
	public void SaveProtoFrame()
	{
        if (RobotClawBase == null || RobotStickBase == null)
            return;
        robotClawSave();
        imageByteSave();
        ObjectsSave();
        KeyPressSave();
        gameDetail.CurrentScore.Add(GameMaster.CurrentScore);
        gameDetail.CurrentBPScore.Add(GameMaster.CurrentScoreBP);
        gameDetail.switchScene.Add(false);
        addSwitchScene = false;
    }

    /// <summary>
    /// Recording: every frame
    /// Rotation of Claw and Stick in float
    /// Position of Claw and Stick in float (x, y, z)
    /// Opening of Claw in bool
    /// </summary>
    void robotClawSave()
    {
        gameDetail.rotationClawArm.Add(RobotClawMover.rotationLowArm);
        gameDetail.rotationClawArm.Add(RobotClawMover.rotationMidArm);
        gameDetail.rotationClawArm.Add(RobotClawMover.rotationTopArm);
        gameDetail.ClawBodyPosition.Add(RobotClawBase.transform.position.x);
        gameDetail.ClawBodyPosition.Add(RobotClawBase.transform.position.y);
        gameDetail.ClawBodyPosition.Add(RobotClawBase.transform.position.z);

        gameDetail.ClawController.Add(RobotClawControler.clawOpen);

        gameDetail.rotationStickArm.Add(RobotStickMover.rotationLowArm);
        gameDetail.rotationStickArm.Add(RobotStickMover.rotationMidArm);
        gameDetail.rotationStickArm.Add(RobotStickMover.rotationTopArm);
        gameDetail.StickBodyPosition.Add(RobotStickBase.transform.position.x);
        gameDetail.StickBodyPosition.Add(RobotStickBase.transform.position.y);
        gameDetail.StickBodyPosition.Add(RobotStickBase.transform.position.z);
    }

    /// <summary>
    /// Convert Color32ArraytoByteArray and store it in Byte Array
    /// </summary>
    void imageByteSave()
	{

		if (Color32ArrayToByteArray(BPIoUCalculation.imageArrayBlock) != null && GameMaster.justScored == true)
		{
            GameMaster.justScored = false;
            int byteSize = BPIoUCalculation.imageArrayBlock.Length;
			byte[] imageArrayBlock = new byte[byteSize * 4];
			byte[] imageArrayBP = new byte[byteSize * 4];
			Color32ArrayToByteArray(BPIoUCalculation.imageArrayBlock).CopyTo(imageArrayBlock, 0);
			gameDetail.ImageByteBlock.Add(imageArrayBlock);
			Color32ArrayToByteArray(BPIoUCalculation.imageArrayBP).CopyTo(imageArrayBP, 0);
			gameDetail.ImageByteBP.Add(imageArrayBP);
		} else {
			byte[] imageArrayBlock = new byte[0];
			byte[] imageArrayBP = new byte[0];
			gameDetail.ImageByteBlock.Add(imageArrayBlock);
			gameDetail.ImageByteBP.Add(imageArrayBP);
		}
	}

    /// <summary>
    /// Record: BlockPosition
    /// BlockCount: Number of Block in int32
    /// BlocksPosition: Position in (x, y, x) and append to each one
    /// </summary>
    void ObjectsSave()
    {
        gameDetail.BlockCount.Add(GameMaster.gameBlocks.Count);
        foreach (var block in GameMaster.gameBlocks) {
            gameDetail.BlocksPosition.Add(block.transform.position.x);
            gameDetail.BlocksPosition.Add(block.transform.position.y);
            gameDetail.BlocksPosition.Add(block.transform.position.z);
        }

        gameDetail.BunnyCount.Add(Bunnies.Length);
        foreach (var bunny in Bunnies){
            gameDetail.BunniesPosition.Add(bunny.transform.position.x);
            gameDetail.BunniesPosition.Add(bunny.transform.position.y);
            gameDetail.BunniesPosition.Add(bunny.transform.position.z);
        }
    }

    /// <summary>
    /// Save all the input as bool
    /// </summary>
    void KeyPressSave()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        { PressQ = true; }
        else { PressQ = false; }

        if (Input.GetKeyDown(KeyCode.A))
        { PressA = true; }
        else { PressA = false; }

        if (Input.GetKeyDown(KeyCode.W))
        { PressW = true; }
        else { PressW = false; }

        if (Input.GetKeyDown(KeyCode.S))
        { PressS = true; }
        else { PressS = false; }

        if (Input.GetKeyDown(KeyCode.E))
        { PressE = true; }
        else { PressE = false; }

        if (Input.GetKeyDown(KeyCode.D))
        { PressD = true; }
        else { PressD = false; }

        if (Input.GetKeyDown(KeyCode.Tab))
        { PressTab = true; }
        else { PressTab = false; }

        if (Input.GetKeyDown(KeyCode.Space))
        { PressSpace = true; }
        else { PressSpace = false; }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        { PressRightArrow = true; }
        else { PressRightArrow = false; }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        { PressLeftArrow = true; }
        else { PressLeftArrow = false; }
        gameDetail.PlayerControls.Add(PressQ);
        gameDetail.PlayerControls.Add(PressA);
        gameDetail.PlayerControls.Add(PressW);
        gameDetail.PlayerControls.Add(PressS);
        gameDetail.PlayerControls.Add(PressE);
        gameDetail.PlayerControls.Add(PressD);
        gameDetail.PlayerControls.Add(PressTab);
        gameDetail.PlayerControls.Add(PressSpace);
        gameDetail.PlayerControls.Add(PressRightArrow);
        gameDetail.PlayerControls.Add(PressLeftArrow);
    }

    //public void BlockLoad()
    //{
    //	float block1X = ProtoFile.gameDetail.Block1X[0], block1Y = ProtoFile.gameDetail.Block1Y[0], block1Z = ProtoFile.gameDetail.Block1Z[0];
    //	float block2X = ProtoFile.gameDetail.Block2X[0], block2Y = ProtoFile.gameDetail.Block2Y[0], block2Z = ProtoFile.gameDetail.Block2Z[0];
    //	BlockOne.transform.position = new Vector3(block1X, block1Y, block1Z);
    //	BlockSed.transform.position = new Vector3(block2X, block2Y, block2Z);

    //	ProtoFile.gameDetail.Block1X.RemoveAt(0);
    //	ProtoFile.gameDetail.Block1Y.RemoveAt(0);
    //	ProtoFile.gameDetail.Block1Z.RemoveAt(0);

    //	ProtoFile.gameDetail.Block2X.RemoveAt(0);
    //	ProtoFile.gameDetail.Block2Y.RemoveAt(0);
    //	ProtoFile.gameDetail.Block2Z.RemoveAt(0);
    //}

    //private IEnumerator delayStart()
    //{
    //	yield return new WaitUntil(() => _RobotClawControl.GetComponent<RobotClaw>() != null && GameObject.FindGameObjectsWithTag("Block").Length == 2);
    //	//yield return new WaitForSeconds(1.5f);
    //	RobotClawControl = _RobotClawControl.GetComponent<RobotClaw>();
    //	var Blocks = GameObject.FindGameObjectsWithTag("Block");
    //	//        Debug.Log("number blocks : " + Blocks.Length);
    //	//      Debug.Log("Block1" + Blocks[0]);
    //	//    Debug.Log("Block2" + Blocks[1]);
    //	BlockOne = Blocks[0];
    //	BlockSed = Blocks[1];
    //	readyStart = true;

    //}

    void vector3AssingePosition(Vector3 assigne, ref float x, ref float y, ref float z)
	{
		x = assigne.x;
		y = assigne.y;
		z = assigne.z;
	}

	/// <summary>
	/// Convert color32 to byte array
	/// </summary>
	/// <param name="colors"></param>
	/// <returns> byte array </returns>
	private static byte[] Color32ArrayToByteArray(Color32[] colors)
	{
		if (colors == null || colors.Length == 0)
			return null;

		int lengthOfColor32 = Marshal.SizeOf(typeof(Color32));
		int length = lengthOfColor32 * colors.Length;
		byte[] bytes = new byte[length];

		GCHandle handle = default(GCHandle);
		try
		{
			handle = GCHandle.Alloc(colors, GCHandleType.Pinned);
			IntPtr ptr = handle.AddrOfPinnedObject();
			Marshal.Copy(ptr, bytes, 0, length);
		}
		finally
		{
			if (handle != default(GCHandle))
				handle.Free();
		}

		return bytes;
	}
}
