using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour
{
    
    


   public List<Vector2Int> GetTargets(Board board)
   {
        List<Vector2Int> pos = new List<Vector2Int>();
        Tile tile = GetComponent<Tile>();
         
        int xPos = tile.x;
        int yPos = tile.y;

        pos.Add(new Vector2Int(xPos,yPos));

        if(xPos + 1 < board.width)
        {
            pos.Add(new Vector2Int(xPos + 1 , yPos));
        }
        if(xPos - 1 >= 0)
        {
            pos.Add(new Vector2Int(xPos - 1 , yPos));
        }
        if(yPos + 1 < board.height)
        {
            pos.Add(new Vector2Int(xPos, yPos + 1));
        }
        if(yPos - 1 >= 0)
        {
            pos.Add(new Vector2Int(xPos, yPos - 1));
        }

        

        TileColors color = (TileColors)Random.Range(0,5);
        List<Vector2Int> candidates = new List<Vector2Int>();

        for(int x = 0; x < board.width; x++)
        {
            for(int y = 0; y < board.height; y++)
            {
                Vector2Int cell = new Vector2Int(x,y);

                if(board.GetColorAt(x,y) == color && !pos.Contains(cell))
                {
                    candidates.Add(cell);
                }
            }
        }

        int count = Mathf.Min(2, candidates.Count);

        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, candidates.Count);
            pos.Add(candidates[index]);
            candidates.RemoveAt(index);
        }

        return pos;
   }
}
