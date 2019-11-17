using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InputManager
{
    private int spawnBunnyCost;
    private int spawnSquareCost;
    private int spawnRectangleCost;
    private int spawnTriangleCost;
    private int spawnWeightCost;

    private bool complexMovement;

    public InputManager(
        int _spawnBunnyCost,
        int _spawnSquareCost,
        int _spawnRectangleCost,
        int _spawnTriangleCost,
        int _spawnWeightCost,
        bool _complexMovement = false)
    {
        spawnBunnyCost = _spawnBunnyCost;
        spawnSquareCost = _spawnSquareCost;
        spawnRectangleCost = _spawnRectangleCost;
        spawnTriangleCost = _spawnTriangleCost;
        spawnWeightCost = _spawnWeightCost;
        complexMovement = _complexMovement;
    }

    // User Input
    public void SpawnInput()
    {
        // Spawn Bunny
        if (Input.GetKeyDown(KeyCode.B))
        {
            // check if enough score
            if (GameMaster.IsEnoughScore(spawnBunnyCost) && SceneManager.GetActiveScene().name.Contains("Build"))
            {
                EventManager.TriggerEvent("BunnySpawn");
                GameMaster.CurrentScore = GameMaster.CurrentScore - spawnBunnyCost;
                EventManager.TriggerEvent("UpdateScore");
            }
            else
                Debug.Log("Score Too Low");
        }

        // Spawn Square
        if (Input.GetKeyDown(KeyCode.Y))
        {
            // check if enough score
            if (GameMaster.IsEnoughScore(spawnSquareCost))
            {
                EventManager.TriggerEvent("SquareSpawn");
                GameMaster.CurrentScore = GameMaster.CurrentScore - spawnSquareCost;
                EventManager.TriggerEvent("UpdateScore");
            }
            else
                Debug.Log("Score Too Low");
        }

        // Spawn Rectangle
        if (Input.GetKeyDown(KeyCode.R))
        {
            // check if enough score
            if (GameMaster.IsEnoughScore(spawnRectangleCost))
            {
                EventManager.TriggerEvent("RectangleSpawn");
                GameMaster.CurrentScore = GameMaster.CurrentScore - spawnRectangleCost;
                EventManager.TriggerEvent("UpdateScore");
            }
            else
                Debug.Log("Score Too Low");
        }

        // Spawn Triangle
        if (Input.GetKeyDown(KeyCode.T))
        {
            // check if enough score
            if (GameMaster.IsEnoughScore(spawnTriangleCost))
            {
                EventManager.TriggerEvent("TriangleSpawn");
                GameMaster.CurrentScore = GameMaster.CurrentScore - spawnTriangleCost;
                EventManager.TriggerEvent("UpdateScore");
            }
            else
                Debug.Log("Score Too Low");
        }

        // Spawn Pack of Building
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // check if enough score
            for (int i = 0; i < 5; i++) {
                if (i == 0 && GameMaster.IsEnoughScore(spawnRectangleCost))
                {
                    EventManager.TriggerEvent("RectangleSpawn");
                    GameMaster.CurrentScore = GameMaster.CurrentScore - spawnRectangleCost;
                    EventManager.TriggerEvent("UpdateScore");
                }
                else if (i >= 1 && GameMaster.IsEnoughScore(spawnSquareCost))
                {
                    EventManager.TriggerEvent("SquareSpawn");
                    GameMaster.CurrentScore = GameMaster.CurrentScore - spawnSquareCost;
                    EventManager.TriggerEvent("UpdateScore");
                }
                else
                    Debug.Log("Score Too Low");
            }
        }

        // Spawn Weight
        if (Input.GetKeyDown(KeyCode.P))
        {
            // check if enough score
            if (GameMaster.IsEnoughScore(spawnWeightCost) && SceneManager.GetActiveScene().name.Contains("Weight"))
            {
                int rand = Random.Range(1, 4);
                switch (rand)
                {
                    case 1:
                        EventManager.TriggerEvent("WeightSpawn30");
                        break;
                    case 2:
                        EventManager.TriggerEvent("WeightSpawn60");
                        break;
                    case 3:
                        EventManager.TriggerEvent("WeightSpawn90");
                        break;
                }
                GameMaster.CurrentScore = GameMaster.CurrentScore - spawnWeightCost;
                EventManager.TriggerEvent("UpdateScore");
            }
            else
                Debug.Log("Score Too Low");
        }

        // Spawn House
        if (Input.GetKeyDown(KeyCode.H))
        {
            EventManager.TriggerEvent("HouseSpawn");
        }

        //Spawn HouseOnBase
        if (Input.GetKeyDown(KeyCode.O))
        {
            EventManager.TriggerEvent("HouseOnBase");
        }
    }

    public void CraneInput()
    {
        if (Input.GetKey(KeyCode.D))
        {
            EventManager.TriggerEvent("CraneMoveRight");
        }
        if (Input.GetKey(KeyCode.A))
        {
            EventManager.TriggerEvent("CraneMoveLeft");
        }
        if (Input.GetKey(KeyCode.W))
        {
            EventManager.TriggerEvent("RopeMoveUp");
        }
        if (Input.GetKey(KeyCode.S))
        {
            EventManager.TriggerEvent("RopeMoveDown");
        }
    }

    public void ToolInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventManager.TriggerEvent("Use");
        }
        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            EventManager.TriggerEvent("UFOGlue");
        }
    }

    public void RoboticArmInput()
    {
        //Cart Movement
        if (Input.GetKey(KeyCode.RightArrow))
            EventManager.TriggerEvent("CartMoveRight");
        if (Input.GetKey(KeyCode.LeftArrow))
            EventManager.TriggerEvent("CartMoveLeft");

        //Arm Movement
        if (complexMovement)
        {
            if (Input.GetKey(KeyCode.Q))
                EventManager.TriggerEvent("BaseMoveUp");
            if (Input.GetKey(KeyCode.A))
                EventManager.TriggerEvent("BaseMoveDown");
            if (Input.GetKey(KeyCode.W))
                EventManager.TriggerEvent("MidMoveUp");
            if (Input.GetKey(KeyCode.S))
                EventManager.TriggerEvent("MidMoveDown");
            if (Input.GetKey(KeyCode.E))
                EventManager.TriggerEvent("TopMoveUp");
            if (Input.GetKey(KeyCode.D))
                EventManager.TriggerEvent("TopMoveDown");
        }
        else
        {
            if (Input.GetKey(KeyCode.D))
                EventManager.TriggerEvent("ArmMoveRight");
            if (Input.GetKey(KeyCode.A))
                EventManager.TriggerEvent("ArmMoveLeft");
            if (Input.GetKey(KeyCode.W))
                EventManager.TriggerEvent("ArmMoveUp");
            if (Input.GetKey(KeyCode.S))
                EventManager.TriggerEvent("ArmMoveDown");
        }

    }

}
