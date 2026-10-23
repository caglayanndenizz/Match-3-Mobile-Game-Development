using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public int maxMoveCount = 10;
    public int remainingMoveCount;
    public int targetPoint;
    public int currentPoint;
    public bool isGameOver = false;
    public TextMeshProUGUI pointsText;


    void Start()
    {
        remainingMoveCount = maxMoveCount;
    }


    void AddAmount(int points)
   {

        currentPoint = Mathf.Min(targetPoint , currentPoint + points);
        //UpdatePoints();
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


    public void ApplyPoints(int groupSize)
    {
        int points = groupSize;
        if(groupSize >= 4)
        {
            points++;
        }
        AddAmount(points * 10);
    }

    public void UpdatePoints()
    {
        pointsText.text = currentPoint.ToString();
    }

}
