using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    //public GameObject skillRewardTextPrefab;
    public GameObject BPRewardTextPrefab;

    //public float skillRewardTextOffsetX = 0;
    //public float skillRewardTextOffsetY = 2;

    public float BPRewardTextOffsetX = -2;
    public float BPRewardTextOffsetY = 2;

    //private TextMesh skillRewardTextMesh;
    private TextMesh BPRewardTextMesh;
    //private Vector3 skillRewardTextOffset;
    private Vector3 BPRewardTextOffset;
    // Start is called before the first frame update
    void Start()
    {
        //skillRewardTextMesh = skillRewardTextPrefab.GetComponent<TextMesh>();
        BPRewardTextMesh = BPRewardTextPrefab.GetComponent<TextMesh>();
        //skillRewardTextOffset = new Vector3(skillRewardTextOffsetX, skillRewardTextOffsetY, 0);
        BPRewardTextOffset = new Vector3(BPRewardTextOffsetX, BPRewardTextOffsetY, 0);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //public void skillRewardTextSpawn(int SkillScore)
    //{
    //    if (SkillScore > 0)
    //    {
    //        skillRewardTextMesh.text = "+" + SkillScore;
    //        Instantiate(skillRewardTextPrefab, this.transform.position + skillRewardTextOffset, Quaternion.identity);
    //    }
    //    else if (SkillScore < 0)
    //    {
    //        SkillScore = SkillScore * -1;
    //        skillRewardTextMesh.text = "-" + SkillScore;
    //        Instantiate(skillRewardTextPrefab, this.transform.position + skillRewardTextOffset, Quaternion.identity);
    //    }

    //}
    public void BPRewardTextSpawn(int BPScore)
    {
        if (BPScore > 0)
        {
            BPRewardTextMesh.text = "+" + BPScore;
            Instantiate(BPRewardTextPrefab, this.transform.position + BPRewardTextOffset, Quaternion.identity);
        }
        else if (BPScore < 0)
        {
            BPScore = BPScore * -1;
            BPRewardTextMesh.text = "-" + BPScore;
            Instantiate(BPRewardTextPrefab, this.transform.position + BPRewardTextOffset, Quaternion.identity);
        }

    }
}