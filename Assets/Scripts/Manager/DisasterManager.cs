using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisasterManager : MonoBehaviour
{
    public float warningDuration = 2;

    [Header("Disaster GameObjects")]
    public GameObject cannon;
    public GameObject epicenter;
    public GameObject epicenterBase;
    public GameObject hailCloud;
    public GameObject monster;
    public GameObject explosionPrefab;

    [Header("Warning Sign")]
    public GameObject cannonWarning;
    public GameObject earthquakeWarning;
    public GameObject hailWarning;
    public GameObject monsterWarning;
    public float warningFlashSpeed = 1f;

    private int rand;
    private bool isDisplayWarning = false;
    private GameObject currentWarning;
    private Text currentWarningText;
    private Image currentWarningImage;
    private Color signColor = Color.white;
    private float timer;

    private void Awake()
    {
        StopAll();
        rand = 0;
        destroyAnythingOffBase();
    }

    private void Update()
    {
        if (isDisplayWarning == true)
        {
            timer += Time.deltaTime;
            WarningFlash(currentWarning);
        }
    }

    public void CannonTrigger()
    {
        StartCoroutine(DisasterCoroutine(cannon, cannonWarning));
    }

    public void EarthquakeTrigger()
    {
        StartCoroutine(DisasterCoroutine(epicenter, earthquakeWarning));
        StartCoroutine(DisasterCoroutine(epicenterBase, earthquakeWarning));
    }

    public void HailTrigger()
    {
        StartCoroutine(DisasterCoroutine(hailCloud, hailWarning));
    }

    public void MonsterTrigger()
    {
        StartCoroutine(DisasterCoroutine(monster, monsterWarning));
    }

    public void RandomTrigger()
    {
        rand = Random.Range(1, 5);
        switch (rand)
        {
            case 1:
                CannonTrigger();
                break;
            case 2:
                EarthquakeTrigger();
                break;
            case 3:
                HailTrigger();
                break;
            case 4:
                MonsterTrigger();
                break;
        }
    }

    private void StopAll()
    {
        GameMaster.AllowDisaster = false;
        cannon.SetActive(false);
        epicenter.SetActive(false);
        epicenterBase.SetActive(false);
        hailCloud.SetActive(false);
        monster.SetActive(false);
    }

    IEnumerator DisasterCoroutine(GameObject disaster, GameObject warningSign)
    {
        StopAll();

        // Turn on warning sign
        currentWarning = warningSign;
        warningSign.SetActive(true);
        isDisplayWarning = true;

        // get warning sign text component
        currentWarningText = warningSign.GetComponent<Text>();
        currentWarningImage = warningSign.GetComponentInChildren<Image>();

        yield return new WaitForSeconds(warningDuration);

        // Turn off warning sign
        warningSign.SetActive(false);
        isDisplayWarning = false;
        disaster.SetActive(true);
        if (disaster != epicenterBase)
            GameMaster.AllowDisaster = !GameMaster.AllowDisaster;
    }

    private void WarningFlash(GameObject warningSign)
    {
        signColor.a = (float)(0.5 + (Mathf.Sin(warningFlashSpeed * timer) / 2));
        currentWarningText.color = signColor;
        currentWarningImage.color = signColor;
    }

    void destroyAnythingOffBase()
    {
        foreach (GameObject obj in GameMaster.gameBlocks)
        {
            if (obj.tag == "Bunny" && obj.GetComponent<BunnyController>().bunnyInScoringArea == false)
            {
                Instantiate(explosionPrefab, obj.transform.position, obj.transform.rotation);
                Destroy(obj);
            }
            if (obj.tag == "Block" && obj.GetComponent<BlockScoreChecker>().objectInScoringArea == false)
            {
                Instantiate(explosionPrefab, obj.transform.position, obj.transform.rotation);
                Destroy(obj);
            }
        }
    }
}
