using System.Collections.Generic;
using UnityEngine;

public class HorizontalRocket : MonoBehaviour
{
    public int yPos;

    public List<Vector2Int> GetTargets(Board board)
    {
        List<Vector2Int> targets = new List<Vector2Int>();
        Tile tile = GetComponent<Tile>();

        yPos = tile.y;

        AddRowTiles(board, targets);

        return targets;
    }

    void AddRowTiles(Board board, List<Vector2Int> targets)
    {
        for(int x = 0; x < board.width; x++)
        {
            targets.Add(new Vector2Int(x, yPos));
        }
    }
}