using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int maxMoveCount = 10;
    public int remainingMoveCount;
    public int targetPoint;
    public int currentPoint;
    public bool isGameOver = false;

    void Start()
    {
        remainingMoveCount = maxMoveCount;
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
}
