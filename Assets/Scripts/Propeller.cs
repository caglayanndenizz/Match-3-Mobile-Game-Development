using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour
{
    
    public List<Vector2Int> targets;
    public int xPos;
    public int yPos;

   public List<Vector2Int> GetTargets(Board board)
   {
        List<Vector2Int> targets = new List<Vector2Int>();
        Tile tile = GetComponent<Tile>();
         
        xPos = tile.x;
        yPos = tile.y;

        targets.Add(new Vector2Int(xPos,yPos));

        AddNeighbors(board , targets);
        AddRandomColorTiles(board, targets);

        return targets;
   }


   void AddNeighbors(Board board , List<Vector2Int> targets)
   {
        if(xPos + 1 < board.width)
        {
            targets.Add(new Vector2Int(xPos + 1 , yPos));
        }
        if(xPos - 1 >= 0)
        {
            targets.Add(new Vector2Int(xPos - 1 , yPos));
        }
        if(yPos + 1 < board.height)
        {
            targets.Add(new Vector2Int(xPos, yPos + 1));
        }
        if(yPos - 1 >= 0)
        {
            targets.Add(new Vector2Int(xPos, yPos - 1));
        }

   }


   void AddRandomColorTiles(Board board , List<Vector2Int> targets)
   {
        TileColors color = (TileColors)Random.Range(0,5);
        List<Vector2Int> candidates = new List<Vector2Int>();

        for(int x = 0; x < board.width; x++)
        {
            for(int y = 0; y < board.height; y++)
            {
                Vector2Int cell = new Vector2Int(x,y);

                if(board.GetColorAt(x,y) == color && !targets.Contains(cell))
                {
                    candidates.Add(cell);
                }
            }
        }

        int count = Mathf.Min(2, candidates.Count);

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, candidates.Count);
            targets.Add(candidates[index]);
            candidates.RemoveAt(index);
        }
   }
}
