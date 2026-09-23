using UnityEngine;

public class Board : MonoBehaviour
{
    public int width = 8;
    public int height = 8;
    TileColors[,] grid;
    Tile[,] tiles;
    Tile selectedTile;

    public GameObject tilePrefab;

    void Start()
    {
        
        CreateBoard();
        //PrintBoard();
        CreateTiles();


    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //mouse un koordinatlarini oyun dunyasinin icindeki koordinatlarla esitliyoruz.
            int x = Mathf.RoundToInt(mousePos.x);
            int y = Mathf.RoundToInt(mousePos.y);
            Debug.Log(x + "," + y);

            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                return;
            }

            if (selectedTile != null)
            {
                //AreNeighbors();
            }
            else
            {
                Debug.Log(selectedTile);
            }
        

        } 
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


    bool MakesMatch(int x, int y, TileColors color)
    {
        if (x >= 2)
        {
            TileColors left1 = grid[x - 1, y];
            TileColors left2 = grid[x - 2, y];

            if (left1 == color && left2 == color)
            {
                return true;
            }
        }

        if (y >= 2)
        {
            TileColors down1 = grid[x, y - 1];
            TileColors down2 = grid[x, y - 2];

            if (down1 == color && down2 == color)
            {
                return true;
            }
        }

        return false;

    }

    void CreateTiles()
    {
        tiles = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 position = new Vector2(x, y);
                GameObject tileObject = Instantiate(tilePrefab, position, Quaternion.identity);
                Tile tile = tileObject.GetComponent<Tile>();
                tile.x = x;
                tile.y = y;

                tiles[x, y] = tile;
                

                Color color = GetColor(grid[x, y]);
                tileObject.GetComponent<SpriteRenderer>().color = color;
            }
        }


    }

    Color GetColor(TileColors tileColor)
    {
        if (tileColor == TileColors.Red) return Color.red;
        if (tileColor == TileColors.Blue) return Color.blue;
        if (tileColor == TileColors.Green) return Color.green;
        if (tileColor == TileColors.Yellow) return Color.yellow;
        if (tileColor == TileColors.Purple) return Color.purple;

        return Color.white;

    }

    void CreateBoard()
    {
        grid = new TileColors[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileColors color;
                //hucreler icerisinde renkleri kontrol etmemiz gerektigi icin en ic
                //dongude tanimladik ve bu sekilde yazdirdik.
                do
                {
                    color = (TileColors)Random.Range(0, 5);
                    //=> Enum'i type casting ile int e cevirdik.
                    //int sayi = (int)TileColor.Green; ==> 2

                }
                while (MakesMatch(x, y, color));

                grid[x, y] = color;




            }
        }
         //8x8 board da grid in belirledigimiz renk degisenlleriyle random doldurulmasi.

    }

    
}
