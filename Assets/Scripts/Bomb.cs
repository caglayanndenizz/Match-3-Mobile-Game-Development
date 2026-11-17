using UnityEngine;
using System.Collections.Generic;

public class Bomb : MonoBehaviour
{

    int xPos;
    int yPos;

   public List<Vector2Int> GetTargets(Board board)
    {
        List<Vector2Int> targets = new List<Vector2Int>();
        Tile tile = GetComponent<Tile>();

        yPos = tile.y;
        xPos = tile.x;

        AddTiles(board, targets);

        return targets;
    }



    void AddTiles(Board board , List<Vector2Int> targets) 
    {
        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if(xPos + x < board.width && yPos + y < board.height && xPos + x >= 0 && yPos + y >= 0)
                {
                    targets.Add(new Vector2Int(xPos + x , yPos + y));
                }
            }
        }
    }
}
