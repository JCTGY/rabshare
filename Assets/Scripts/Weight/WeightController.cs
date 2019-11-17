using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightController : MonoBehaviour
{
    public GameObject rewardTextPrefab;
    public float rewardTextOffsetX = 0;
    public float rewardTextOffsetY = 2;

    private TextMesh rewardTextMesh;
    private Vector3 rewardTextOffset;

    // Start is called before the first frame update
    void Start()
    {
        rewardTextMesh = rewardTextPrefab.GetComponent<TextMesh>();
        rewardTextOffset = new Vector3(rewardTextOffsetX, rewardTextOffsetY, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RewardTextSpawn(int ScoreByWeight)
    {
        // ScoreByHeight = ?
        rewardTextMesh.text = "+" + ScoreByWeight;
        Instantiate(rewardTextPrefab, this.transform.position + rewardTextOffset, Quaternion.identity);
    }
}
