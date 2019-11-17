using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockScoreChecker : MonoBehaviour
{
    [HideInInspector]
    public bool objectInScoringArea;

    bool readyToCheckScore;
    bool beginning;

    [HideInInspector]
    public bool droppedBlock = false;

    Rigidbody2D blockRB;

    // Start is called before the first frame update
    void Start()
    {
        blockRB = GetComponent<Rigidbody2D>();
        beginning = true;
        StartCoroutine(dontScoreInitialFrame());
    }

    // Update is called once per frame
    void Update()
    {
        if (objectInScoringArea && readyToCheckScore == false && blockRB.velocity != Vector2.zero && beginning == false &&
            Snapper.haveBlock == false && TractorBeam.holdingSomething == false && RobotClaw.holdingBlock == false)
            readyToCheckScore = true;
        else if (droppedBlock == true && objectInScoringArea == false &&
                readyToCheckScore == false && blockRB.velocity != Vector2.zero && beginning == false &&
                Snapper.haveBlock == false && TractorBeam.holdingSomething == false && RobotClaw.holdingBlock == false)
        {
            droppedBlock = true;
            RobotArmAgent.droppedBlockOffBase = true;
        }


        if (readyToCheckScore)
        {
            if (blockRB.velocity == Vector2.zero)
            {
                Debug.Log("CalculatingScore");
                StartCoroutine(calculateBPPoints());
                readyToCheckScore = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Base")
            objectInScoringArea = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Base")
            objectInScoringArea = false;
    }

    IEnumerator calculateBPPoints()
    {
        Debug.Log("Scoring");

        //wait to let block fall before calculating.
        yield return new WaitForEndOfFrame();

        if (SceneManager.GetActiveScene().name.Contains("Disaster") || SceneManager.GetActiveScene().name.Contains("Score"))
            yield break;

        BPIoUCalculation.IoUCalculation();
        if (GameMaster._lblockInBP != (int)BPIoUCalculation.IoU)
        {
            int IoU = (int)BPIoUCalculation.IoU;
            //var Base = GameObject.FindGameObjectWithTag("Base");
            GameObject Base = GameObject.Find("Base");
            Base.GetComponent<BaseController>().BPRewardTextSpawn(IoU - GameMaster._lblockInBP);
            GameMaster.CurrentScoreBP += IoU - GameMaster._lblockInBP;
            GameMaster._lblockInBP = IoU;
            GameMaster.justScored = true;
        }
    }

    IEnumerator dontScoreInitialFrame()
    {
        yield return new WaitForSeconds(2.0f);
        beginning = false;
    }
}
