using UnityEngine;

public class Board : MonoBehaviour
{
    public int width = 8;
    public int height = 8;
    TileColors[,] grid;

    void Start()
    {
        grid = new TileColors[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = (TileColors)Random.Range(0, 5);
                //=> Enum'i type casting ile int e cevirdik.
                //int sayi = (int)TileColor.Green; ==> 2
            }
        }
        //8x8 board da grid in belirledigimiz renklerle random doldurulmasi.

        PrintBoard();

    }

    void PrintBoard()
    {
        string text = "";

        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                text += grid[x, y].ToString()[0] + " ";
                //burada da grid in console da gozukebilmesi icin string e cevirme islemi yapildi ve [0] ile ilk harfi alindi.            }
                
            }
            //y yukardan basliyor cunku board umuzun y= 0 noktasi asagilarda olmasi lazim.
            text += "\n";


        }
        Debug.Log(text);


    }
}
