using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    public LevelType type;
    public int targetPoint;
    public int maxMoveCount;
    public float timer;
}
