using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width = 8;
    public int height = 8;
    TileColors[,] grid;
    Tile[,] tiles;
    Tile selectedTile;
    Vector2 startPos;
    HashSet<Tile> matchedTiles;

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
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //mouse un koordinatlarini oyun dunyasinin icindeki koordinatlarla esitliyoruz.
            int x = Mathf.RoundToInt(startPos.x);
            int y = Mathf.RoundToInt(startPos.y);
            Debug.Log(x + "," + y);

            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                return;
            }


            selectedTile = tiles[x, y];

        }

        if (Input.GetMouseButtonUp(0) && selectedTile != null)
        {
            Vector2 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 delta = endPos - startPos;

            if (delta.magnitude > 0.5f)
            {
                TrySwap(delta);
            }
            selectedTile = null;
        }
    }


    //deltadan yonu bulmak ve swapTiles metodu sonradan yazilacak.




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

    void TrySwap(Vector2 delta)
    {
        int targetX = selectedTile.x;
        int targetY = selectedTile.y;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > 0)
            {
                targetX++;
            }
            else
            {
                targetX--;
            }

        }
        else
        {
            if (delta.y > 0)
            {
                targetY++;
            }
            else
            {
                targetY--;
            }
        }

        if (targetX < 0 || targetX >= width || targetY < 0 || targetY >= height)
        {
            return;
        }


        SwapTiles(selectedTile.x, selectedTile.y, targetX, targetY);

        matchedTiles = FindMatches();

        while (matchedTiles.Count > 0)
        {
            DestroyMatches(matchedTiles);
            DropTiles();
            FillBoard();
            matchedTiles = FindMatches();
        }


    }


    void SwapTiles(int x1, int y1, int x2, int y2)
    {
        TileColors tempColor = grid[x1, y1];
        grid[x1, y1] = grid[x2, y2];
        grid[x2, y2] = tempColor;

        Tile tempTile = tiles[x1, y1];
        tiles[x1, y1] = tiles[x2, y2];
        tiles[x2, y2] = tempTile;


        tiles[x1, y1].x = x1;
        tiles[x1, y1].y = y1;

        tiles[x2, y2].x = x2;
        tiles[x2, y2].y = y2;


        tiles[x1, y1].transform.position = new Vector2(x1, y1);
        tiles[x2, y2].transform.position = new Vector2(x2, y2);
    }


    HashSet<Tile> FindMatches()
    {
        matchedTiles = new HashSet<Tile>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileColors color = grid[x, y];
                if (color == TileColors.None)
                {
                    continue;
                }
                if (x + 1 < width && y + 1 < height && grid[x + 1, y] == color && grid[x, y + 1] == color && grid[x + 1, y + 1] == color)
                {
                    matchedTiles.Add(tiles[x, y]);
                    matchedTiles.Add(tiles[x + 1, y]);
                    matchedTiles.Add(tiles[x, y + 1]);
                    matchedTiles.Add(tiles[x + 1, y + 1]);
                }
                if (x + 2 < width && grid[x + 1, y] == color && grid[x + 2, y] == color)
                {
                    matchedTiles.Add(tiles[x, y]);
                    matchedTiles.Add(tiles[x + 1, y]);
                    matchedTiles.Add(tiles[x + 2, y]);

                }

                if (y + 2 < height && grid[x, y + 1] == color && grid[x, y + 2] == color)
                {
                    matchedTiles.Add(tiles[x, y]);
                    matchedTiles.Add(tiles[x, y + 1]);
                    matchedTiles.Add(tiles[x, y + 2]);

                }
            }
        }

        return matchedTiles;

    }

    void DestroyMatches(HashSet<Tile> matchedTiles)
    {
        foreach (Tile tile in matchedTiles)
        {
            grid[tile.x, tile.y] = TileColors.None;
            tiles[tile.x, tile.y] = null;
            Destroy(tile.gameObject);
        }
    }


    void DropTiles()
    {
        for (int x = 0; x < width; x++)
        {
            int emptyY = 0;
            for (int y = 0; y < height; y++)
            {
                if (tiles[x, y] != null)
                {
                    Tile tile = tiles[x, y];
                    if (y != emptyY)
                    {
                        grid[x, emptyY] = grid[x, y];
                        grid[x, y] = TileColors.None;

                        tiles[x, emptyY] = tile;
                        tiles[x, y] = null;

                        tile.y = emptyY;

                        tile.transform.position = new Vector2(x, emptyY);

                    }
                    emptyY++;
                }
            }

        }
    }
    
    void SpawnTile(int x , int y)
    {
        Vector2 pos = new Vector2(x,y);
        GameObject tileObject = Instantiate(tilePrefab , pos , Quaternion.identity);
        Tile tile = tileObject.GetComponent<Tile>();
        tile.x = x;
        tile.y = y;
        tiles[x,y] = tile;
        Color color = GetColor(grid[x,y]);
        tileObject.GetComponent<SpriteRenderer>().color = color;
    }

    void FillBoard()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if(tiles[x,y] == null)
                {
                    grid[x,y] = (TileColors)Random.Range(0,5);
                    SpawnTile(x,y);
                }
            }
        }
    }



}
