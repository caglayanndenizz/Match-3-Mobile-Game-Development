using UnityEngine;

public class Cup : MonoBehaviour
{
   public TileColors color;
   public int targetCount;
   [SerializeField] private int currentCount;
   public CupObjective objective;

   public void AddAmount(int points)
   {

        if(IsFull()) return;
     
        currentCount = Mathf.Min(targetCount , currentCount + points);
        //currentCount un targetCount u gecmeyecek sekilde ustune points eklenerek devam etmesini sagliyoruz.
        if(IsFull())
        {
          Debug.Log("CUP FULL");
        }
   }
   public bool IsFull()
   {
        return currentCount >= targetCount;
   }


   
}
