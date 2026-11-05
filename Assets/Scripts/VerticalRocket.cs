using System.Collections.Generic;
using UnityEngine;

public class VerticalRocket : MonoBehaviour
{
    public int xPos;

    public List<Vector2Int> GetTargets(Board board)
    {
        List<Vector2Int> targets = new List<Vector2Int>();
        Tile tile = GetComponent<Tile>();

        xPos = tile.x;

        AddColumnTiles(board, targets);

        return targets;
    }

    void AddColumnTiles(Board board, List<Vector2Int> targets)
    {
        for(int y = 0; y < board.height; y++)
        {
            targets.Add(new Vector2Int(xPos, y));
        }
    }
}