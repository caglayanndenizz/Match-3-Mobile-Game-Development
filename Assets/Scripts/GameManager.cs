using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int maxMoveCount = 10;
    public int remainingMoveCount;
    public int targetPoint;
    public int currentPoint;
    public bool isGameOver = false;

    List<AugmentType> activeAugments = new List<AugmentType>();

    void Start()
    {
        remainingMoveCount = maxMoveCount;
        ApplyAugment(AugmentType.RedBonus);
    }


    public void AddAmount(int points)
   {

        currentPoint = Mathf.Min(targetPoint , currentPoint + points);
   }

    public void MakeMove()
    {

        if(!isGameOver)
        {
            remainingMoveCount = remainingMoveCount - 1;
            
        }
    }

    public void GameOver()
    {
        
        if(currentPoint >= targetPoint)
        {
            Debug.Log("you win");
            isGameOver = true;
        }
        else if(remainingMoveCount <= 0)
        {
            Debug.Log("you lose");
            isGameOver = true;
        }
    }

    public void ApplyAugment(AugmentType type)
    {
        activeAugments.Add(type);
    }


    public void ApplyPoints(int amount , TileColors color)
    {
        int points = amount;
        if(amount >= 4)
        {
            points++;
        }

        if(color == TileColors.Red && activeAugments.Contains(AugmentType.RedBonus))
        {
            points += amount;
        }
        if(color == TileColors.Blue && activeAugments.Contains(AugmentType.BlueBonus))
        {
            points += amount;
        }
        if(color == TileColors.Purple && activeAugments.Contains(AugmentType.PurpleBonus))
        {
            points += amount;
        }

        AddAmount(points * 10);
    }
}
