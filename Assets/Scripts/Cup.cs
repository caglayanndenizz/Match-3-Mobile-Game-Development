using UnityEngine;

public class Cup : MonoBehaviour
{
   public TileColors color;
   public int targetCount;
   [SerializeField] private int currentCount;

   public void AddTile()
   {

        if(IsFull())
        {
            return;
        }

        currentCount++;

   }
   public bool IsFull()
   {
        return currentCount >= targetCount;
   }


   
}
