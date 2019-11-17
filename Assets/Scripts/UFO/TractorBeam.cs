using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    public float distanceBetweenBeams = 0.2f;
    float currentHeight;
    public static bool readyToDestroyBeam = false;
    bool firingTractorBeam = false;
    List<GameObject> beamSegments;
    int beamSegmentCount = 0;
    int curOrder = 0;
    GameObject beamSegment;
    public Sprite beamSprite;
    public GameObject blockHoverSpot;
    public GameObject UFO;
    RaycastHit2D firstHitObject;
    public static bool holdingSomething = false;
    public static GameObject heldBlock;
    Texture2D tractorBeamTex;
    float maxXScale;
    float minXScale;
    float beamScaleAdjustment;

    public float energyCostForBeam = 1.0f;

    public GameObject beamTimerBar;
    private FillBar beamTimerScript;

    public GameObject bigBattery;
    public GameObject smallBattery;

    private BatteryBar bigBeamBattery;
    private BatteryBar smallBeamBattery;
    private BatteryBar currentBattery;

    public float weightCapacityForBigBeam = 50.0f;
    public float weightCapacityForSmallBeam = 30.0f;

    private GameObject smallBeamShadow;
    private GameObject bigBeamShadow;
    private GameObject currentShadow;

    public static bool bigBeamEquipped = false;

    public static string origPickupTag;
    public static int origPickupLayer;

    IEnumerator timerVar;

    public float beamTimeLimit = 30.0f; // in seconds

    // Start is called before the first frame update
    void Start()
    {
        beamSegments = new List<GameObject>();
        currentHeight = this.transform.position.y;
        maxXScale = UFO.transform.localScale.x / 2;
        minXScale = 0.4f;
        beamScaleAdjustment = minXScale;

        bigBeamShadow = new GameObject("BigBeamShadow");
        smallBeamShadow = new GameObject("SmallBeamShadow");

        createBeamShadow(bigBeamShadow, maxXScale);
        createBeamShadow(smallBeamShadow, minXScale);

        bigBeamShadow.SetActive(false);
        currentShadow = smallBeamShadow;

        bigBeamBattery = bigBattery.GetComponent<BatteryBar>();
        smallBeamBattery = smallBattery.GetComponent<BatteryBar>();
        currentBattery = smallBeamBattery;
        bigBattery.SetActive(false);

        beamTimerScript = beamTimerBar.GetComponent<FillBar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (firingTractorBeam == false)
            currentHeight = this.transform.position.y;

        if (Input.GetButtonDown("Magnet"))
        {
            if (firingTractorBeam)
            {
                readyToDestroyBeam = true;
                //beamSegment.GetComponent<BeamSegment>().hitPickup = false;
            }
            else if (currentBattery.empty == false && holdingSomething == false && firingTractorBeam == false)
            {
                firingTractorBeam = true;
                StartCoroutine(createTractorBeam());
                //fillBar.depleteEnergy(energyCostForBeam);
                currentBattery.useCharge();
                currentShadow.SetActive(false);
                beamTimerBar.SetActive(true);
                beamTimerScript.setTimer(beamTimeLimit);
            }
        }

        if (beamTimerScript.isEmpty())
            readyToDestroyBeam = true;

        if (readyToDestroyBeam == true)
        {
            firingTractorBeam = false;
            destroyTractorBeam();
            currentShadow.SetActive(true);
            beamTimerScript.resetTimer();
            beamTimerBar.SetActive(false);

            if (holdingSomething == true)
                StartCoroutine(dropBlock());
        }

        //if (holdingSomething == true && firingTractorBeam == false && Input.GetButtonDown("Magnet"))
        //{
        //    Debug.Log("Drop");
        //    StartCoroutine(dropBlock());
        //}

        if (bigBeamEquipped && firingTractorBeam == false && Input.GetButtonDown("GrowBeam"))
        {
            beamScaleAdjustment = maxXScale;
            bigBattery.SetActive(true);
            smallBattery.SetActive(false);
            currentBattery = bigBeamBattery;
            bigBeamShadow.SetActive(true);
            smallBeamShadow.SetActive(false);
            currentShadow = bigBeamShadow;
        }

        if (firingTractorBeam == false && Input.GetButtonDown("ShrinkBeam"))
        {
            beamScaleAdjustment = minXScale;
            bigBattery.SetActive(false);
            smallBattery.SetActive(true);
            currentBattery = smallBeamBattery;
            bigBeamShadow.SetActive(false);
            smallBeamShadow.SetActive(true);
            currentShadow = smallBeamShadow;
        }

        if (Input.GetButtonDown("Recharge"))
            currentBattery.gainCharge();
    }

    IEnumerator createTractorBeam()
    {
        yield return null;

        beamSegment = new GameObject("BeamSegment" + beamSegmentCount);
        beamSegment.transform.position = this.transform.position;
        Vector3 pos = beamSegment.transform.localPosition;
        //pos.y = currentHeight;
        currentHeight -= distanceBetweenBeams;
        beamSegment.transform.SetParent(this.transform);
        beamSegment.transform.position = Vector3.zero;
        beamSegment.transform.position = pos;
        beamSegment.transform.rotation = Quaternion.identity;
        beamSegment.transform.rotation = UFO.transform.rotation;
        beamSegment.transform.localScale = new Vector3(beamScaleAdjustment, UFO.transform.localScale.y, UFO.transform.localScale.z);


        beamSegment.layer = LayerMask.NameToLayer("UFO");

        SpriteRenderer sr = beamSegment.AddComponent<SpriteRenderer>();
        createTractorBeamSprite(sr, 1.0f, Color.green);
        //SpriteRenderer sr = beamSegment.AddComponent<SpriteRenderer>();
        //sr.sprite = beamSprite;
        sr.sortingOrder = -1;
        //curOrder--;

        BoxCollider2D bc = beamSegment.AddComponent<BoxCollider2D>();
        bc.isTrigger = true;

        BeamSegment bs = beamSegment.AddComponent<BeamSegment>();
        bs.destination = blockHoverSpot;
        bs.UFO = UFO;
        bs.bigBeamSize = maxXScale;
        bs.smallBeamSize = minXScale;
        bs.bigBeamWeightLimit = weightCapacityForBigBeam;
        bs.smallBeamWeightLimit = weightCapacityForSmallBeam;
        bs.Init();

        beamSegmentCount++;

        //if (beamSegments.Count >= 1)
        //{
        //    Destroy(beamSegments[0].GetComponent<BoxCollider2D>());
        //    Destroy(beamSegments[0].GetComponent<BeamSegment>());
        //}
        beamSegments.Add(beamSegment);
    }

    private void createBeamShadow(GameObject beam, float xScale)
    {

        beam.transform.position = this.transform.position;
        Vector3 pos = beam.transform.localPosition;
        beam.transform.SetParent(this.transform);
        beam.transform.position = Vector3.zero;
        beam.transform.position = pos;
        beam.transform.rotation = Quaternion.identity;
        beam.transform.rotation = UFO.transform.rotation;
        beam.transform.localScale = new Vector3(xScale, UFO.transform.localScale.y, UFO.transform.localScale.z);

        beam.layer = LayerMask.NameToLayer("UFO");

        SpriteRenderer sr = beam.AddComponent<SpriteRenderer>();
        createTractorBeamSprite(sr, 0.5f, Color.gray);
        //sr.sprite = beamSprite;
        //sr.color = Color.gray;
        //Color tempColor = sr.color;
        //tempColor.a = 0.251f;
        //sr.color = tempColor;
        sr.sortingOrder = -2;
    }

    private void createTractorBeamSprite(SpriteRenderer sr, float alpha, Color pixelColor)
    {
        sr.sprite = beamSprite;
        string tempName = sr.sprite.name;

        tractorBeamTex = copyTexture2D(sr.sprite.texture, alpha, pixelColor);

        sr.sprite = Sprite.Create(tractorBeamTex, sr.sprite.rect, new Vector2(0.5f, 0.5f));
        sr.sprite.name = tempName;

        sr.material.mainTexture = tractorBeamTex;
    }

    private Texture2D copyTexture2D(Texture2D copiedTexture, float startingAlpha, Color pixelColor)
    {
        Texture2D texture = new Texture2D(copiedTexture.width, copiedTexture.height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        float alphaVal = startingAlpha;

        for (int y = texture.height - 1; y >= 0; y--)
        {
            for (int x = texture.width - 1; x >= 0; x--)
            {
                Color curPixel = copiedTexture.GetPixel(x, y);
                float tempAlpha = (Mathf.Approximately(curPixel.a, 0.0f)) ? curPixel.a : alphaVal;
                texture.SetPixel(x, y, (new Color(curPixel.r, curPixel.g, curPixel.b, tempAlpha) * pixelColor));
            }
            alphaVal -= (Mathf.Approximately(1.0f, startingAlpha)) ? 0.01f : 0.005f;
        }
        texture.Apply();
        return texture;
    }

    private void destroyTractorBeam()
    {
        if (beamSegments.Count >= 1)
        {
            Destroy(beamSegments[beamSegments.Count - 1]);
            beamSegments.RemoveAt(beamSegments.Count - 1);
        }
        if (beamSegments.Count == 0)
        {
            beamSegmentCount = 0;
            curOrder = 0;
            readyToDestroyBeam = false;
            currentHeight = this.transform.position.y;
        }
    }

    IEnumerator dropBlock()
    {
        heldBlock.GetComponent<Rigidbody2D>().isKinematic = false;
        heldBlock.transform.SetParent(null);
        if (origPickupTag == "Bunny") heldBlock.GetComponent<BunnyController>().bunnyIsDropped = true;
        heldBlock.tag = origPickupTag;
        heldBlock.layer = origPickupLayer;
        holdingSomething = false;

        heldBlock.GetComponent<DontDestroy>().enabled = false;
        heldBlock.GetComponent<DontDestroy>().enabled = true;
        yield return null;

    }
}
