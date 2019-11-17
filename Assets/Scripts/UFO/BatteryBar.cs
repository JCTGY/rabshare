using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryBar : MonoBehaviour
{
    public GameObject backgroundImage;
    public GameObject chargeInitPoint;
    public int numberOfCharges = 3;
    public Sprite boxSprite;
    public Color chargeColor;
    public bool empty = false;

    private int curChargesHeld;
    private GameObject box;
    private int boxCount = 0;
    private float xOffset = 0.0f;
    private Vector3 boxScale;
    List<GameObject> chargeList = new List<GameObject>();

    public enum BatteryTypes {BigBeam, SmallBeam, Fuel, Glue };
    public BatteryTypes batteryType;

    private void Start()
    {
        setNumberOfCharges();

        xOffset = backgroundImage.transform.localScale.x / (numberOfCharges * 4); // arbitrary division. just want small offset based off number of charges
        float targetBoxSize = (backgroundImage.transform.localScale.x - xOffset) / numberOfCharges; //subtract by offset to create a more centered border
        boxScale = backgroundImage.transform.localScale;
        boxScale.x = targetBoxSize - xOffset;

        curChargesHeld = numberOfCharges;

        buildChargesAsBoxes();
    }

    void buildChargesAsBoxes()
    {
        Vector3 curPos = chargeInitPoint.transform.localPosition;
        curPos.x += xOffset;

        for (int i = 0; i < numberOfCharges; i++)
        {
            box = new GameObject("Charge" + boxCount);
            boxCount++;

            box.transform.SetParent(this.transform);
            box.transform.localScale = boxScale;
            box.transform.localPosition = curPos;

            //make sure boxes aren't rotated. should take parent's rotation.
            Quaternion rot = box.transform.localRotation;
            rot.z = 0;
            box.transform.localRotation = rot;

            curPos.x += boxScale.x;
            curPos.x += xOffset;

            box.AddComponent<SpriteRenderer>();
            box.GetComponent<SpriteRenderer>().sprite = boxSprite;
            box.GetComponent<SpriteRenderer>().color = chargeColor;
            chargeList.Add(box);
        }
    }

    public void setNumberOfCharges()
    {
        if (batteryType == BatteryTypes.BigBeam)
            numberOfCharges = GameMaster.UFObigBeamCharges;
        else if (batteryType == BatteryTypes.SmallBeam)
            numberOfCharges = GameMaster.UFOsmallBeamCharges;
        else if (batteryType == BatteryTypes.Fuel)
            numberOfCharges = GameMaster.UFOfuelCharges;
        else if (batteryType == BatteryTypes.Glue)
            numberOfCharges = GameMaster.UFOglueCharges;
    }

    public void useCharge()
    {
        if (curChargesHeld > 0)
        {
            Destroy(chargeList[chargeList.Count - 1]);
            chargeList.RemoveAt(chargeList.Count - 1);
            curChargesHeld--;
            boxCount--;
            if (curChargesHeld == 0)
                empty = true;
        }
    }

    public void gainCharge()
    {
        if (curChargesHeld < numberOfCharges)
        {
            Vector3 curPos;
            if (chargeList.Count == 0)
                curPos = chargeInitPoint.transform.localPosition;
            else
            {
                curPos = chargeList[chargeList.Count - 1].transform.localPosition;
                curPos.x += boxScale.x;
            }

            curPos.x += xOffset;

            box = new GameObject("Charge" + boxCount);
            boxCount++;

            box.transform.SetParent(this.transform);
            box.transform.localScale = boxScale;
            box.transform.localPosition = curPos;

            box.AddComponent<SpriteRenderer>();
            box.GetComponent<SpriteRenderer>().sprite = boxSprite;
            box.GetComponent<SpriteRenderer>().color = chargeColor;
            chargeList.Add(box);

            curChargesHeld++;
            empty = false;
        }
    }

    public void fadeOutCharge(float currentEnergy, float lastEnergyCap)
    {
        float alphaRatio = currentEnergy - (lastEnergyCap - 12.0f);
        alphaRatio = alphaRatio / 10.0f;

        Color color = chargeList[chargeList.Count - 1].gameObject.GetComponent<SpriteRenderer>().color;
        color.a = alphaRatio;
        chargeList[chargeList.Count - 1].gameObject.GetComponent<SpriteRenderer>().color = color;
    }
}
