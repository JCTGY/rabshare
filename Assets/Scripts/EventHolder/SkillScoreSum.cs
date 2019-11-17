using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class SkillScoreSum : MonoBehaviour
{
    public int eachSquareSkill = 5;
    public int eachTriangleSkill = 5;
    public int eachRectangleSkill = 5;
    public static List<GameObject> _lsquareBlocks;
    public static List<GameObject> _ltriangleBlocks;
    public static List<GameObject> _lrectangleBlocks;

    public static float stableSquare;
    public static float stableTriangle;
    public static float stableRectangle;
    public static float _stableSquare;
    public static float _stableTriangle;
    public static float _stableRectangle;
    public static float skillScore;
    public static float stableHeight;
    public static float stableWidth;

    void Awake()
    {


    }

    void Update()
    {
        SkillScore();
    }

    //everything is duplicated with gameBlocks. originally was different for each type of block.
    void SkillScore()
    {
        stableWidth = 0;
        if (GameMaster.gameBlocks != null)
        {
			stableSquare = 0;
            _stableSquare = 0;
            foreach (var squarBlock in GameMaster.gameBlocks)
            {
				if (squarBlock != null)
				{
					if (squarBlock.transform.position.x > 8.0f && squarBlock.transform.position.x < 19.0f)
						stableSquare += (squarBlock.transform.position.y + 4.28f);
                    _stableSquare += (squarBlock.transform.position.y);
                    stableWidth += Mathf.Abs(squarBlock.transform.position.x);
                }
			}
        }
        if (GameMaster.gameBlocks != null)
        {
            stableTriangle = 0;
            _stableTriangle = 0;
            foreach (var triangleBlock in GameMaster.gameBlocks)
            {
                if (triangleBlock != null)
				{
					if (triangleBlock.transform.position.x > 8.0f && triangleBlock.transform.position.x < 19.0f)
					    stableTriangle += (triangleBlock.transform.position.y + 4.26f);
                    _stableTriangle += (triangleBlock.transform.position.y);
                    stableWidth += Mathf.Abs(triangleBlock.transform.position.x);
                }    
            }
        }
        if (GameMaster.gameBlocks != null)
        {
            stableRectangle = 0;
            _stableRectangle = 0;
            foreach (var rectangleBlock in GameMaster.gameBlocks)
            {
                if (rectangleBlock != null)
				{
					if (rectangleBlock.transform.position.x > 8.0f && rectangleBlock.transform.position.x < 19.0f)
						stableRectangle += (rectangleBlock.transform.position.y + 4.60f);
                    _stableRectangle += (rectangleBlock.transform.position.y);
                    stableWidth += Mathf.Abs(rectangleBlock.transform.position.x);
                }
            }
        }
        stableHeight = _stableSquare + _stableTriangle + _stableRectangle;
        skillScore = stableSquare * eachSquareSkill + stableTriangle * eachTriangleSkill + stableRectangle * eachRectangleSkill;
    }
}
