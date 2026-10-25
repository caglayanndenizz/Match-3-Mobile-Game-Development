using DG.Tweening;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x;
    public int y;





    public void MoveTo(Vector2 destination , float duration, float delay)
    {
        transform.DOMove(destination, duration).SetDelay(delay);
    }
}
