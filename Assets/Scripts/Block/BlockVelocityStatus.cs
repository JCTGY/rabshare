using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BlockVelocityStatus : MonoBehaviour
{
    public static float _lsumPosY;
    public static float sumPosY;
    public static float _lsumPosX;
    public static float sumPosX;
    // Start is called before the first frame update
    void Start()
    {
        sumPosY = 0;
        sumPosX = 0;
    }
    void Update()
    {
        if (GameMaster.gameBlocks != null)
        {
            sumPosY = System.Math.Abs(SkillScoreSum.stableHeight);
            _lsumPosY = System.Math.Abs(_lsumPosY);
            sumPosX = System.Math.Abs(SkillScoreSum.stableWidth);
            _lsumPosX = System.Math.Abs(_lsumPosX);
            if ((sumPosY - _lsumPosY) / Time.deltaTime > 0.1f || (sumPosY - _lsumPosY) / Time.deltaTime < -0.1f)
            {
                GameMaster.BlockMomentum = true;
                _lsumPosY = sumPosY;
            }
            else if ((sumPosX - _lsumPosX) / Time.deltaTime > 0.1f || (sumPosX - _lsumPosX) / Time.deltaTime < -0.1f)
            {
                GameMaster.BlockMomentum = true;
                _lsumPosX = sumPosX;
            }
            else
                GameMaster.BlockMomentum = false;
        }
    }
}
