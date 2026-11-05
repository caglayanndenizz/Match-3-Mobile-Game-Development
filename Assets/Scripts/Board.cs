using System.Collections.Generic;
using UnityEngine;
using System.Collections;
public class Board : MonoBehaviour
{
    public int width = 8;
    public int height = 8;
    TileColors[,] grid; //taslarin rengini tutar.
    Tile[,] tiles; //tas objelerini tutar.
    Tile selectedTile; //secilen tasin kendisini tutar.
    Vector2 startPos; //mouse un basildigi andaki dunya konumu.
    //Tryswap metodunda delta ile kaydirma yonu hesaplanir.
    HashSet<Tile> matchedTiles; // eslesen taslari bulabilmek icin.
    // hashset olmasinin sebebi ustune bir kez daha yazilmak istenmemesiydi. Yani bir kere kontrol edilecek.
    // Liste olsaydi ustune binebilir ve match i iki katina cikarilabilirdi. 

    public Sprite[] sprites;
    List<Vector2Int> propellerPositions;
    List<Vector2Int> horizontalRocketPositions;
    List<Vector2Int> verticalRocketPositions;
    public GameManager gameManager;
    public GameObject tilePrefab;
    public GameObject propellerPrefab;
    public GameObject horizontalRocketPrefab;
    public GameObject verticalRocketPrefab;
    public float dropDuration = 0.3f;
    public float swapDuration = 0.2f;
    public float rowDelay = 0.05f;

    public bool isAnimating = false;
    
    

    IEnumerator Start()
    {
        isAnimating = true;
        CreateBoard(); // renkleri olusturuyor.
        yield return StartCoroutine(CreateTiles()); // renklere gore tas olusturuyor.
        yield return new WaitForSeconds(dropDuration);
        isAnimating = false;
        //CreateTiles() metodu, CreateBoard() methodunun icerisinde yazan gride gore kuruyor gridi.
        //yani eger createtiles ilk basa konsaydi grid = null olacagi icin null reference exception hatasi gelecekti.
    }
    void Update()
    {
        if(gameManager.isGameOver == false && !isAnimating)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //mouse un koordinatlarini oyun dunyasinin icindeki koordinatlarla esitliyoruz.
                int x = Mathf.RoundToInt(startPos.x);
                int y = Mathf.RoundToInt(startPos.y);

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
                    StartCoroutine(TrySwap(delta));
                }
                else
                {
                    Propeller propeller = selectedTile.GetComponent<Propeller>();
                    HorizontalRocket horizontalRocket = selectedTile.GetComponent<HorizontalRocket>();
                    VerticalRocket verticalRocket = selectedTile.GetComponent<VerticalRocket>();
                    
                    if(propeller != null)
                    {
                        StartCoroutine(ActivateBooster(propeller.GetTargets(this)));
                        //Activate Propeller ability
                    }
                    else if(verticalRocket != null)
                    {
                        StartCoroutine(ActivateBooster(verticalRocket.GetTargets(this)));
                    }
                    else if(horizontalRocket != null)
                    {
                        StartCoroutine(ActivateBooster(horizontalRocket.GetTargets(this)));
                    }
                    
                }
                selectedTile = null;
            }

            
        }
        
    }
    void CreateBoard()
    {
        grid = new TileColors[width, height]; //2D bir grid oldugu icin width x height

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileColors color;
                //hucreler icerisinde renkleri kontrol etmemiz gerektigi icin en ic
                //dongude tanimladik ve bu sekilde yazdirdik.
                //Eger do nun disinda tanimlanmasaydi while kosulu bunu goremeyecekti.
                do
                {
                    color = (TileColors)Random.Range(0, 5);
                    //=> Int i enum a cevirdik type casting ile.
                    //int sayi = (int)TileColor.Green; ==> 2
                    //8x8 board da grid in belirledigimiz renk degisenlleriyle random doldurulmasi.
                    // 5 dahil olmayacagi icin none i goremeyecek. 
                }
                while (MakesMatch(x, y, color));
                //board ilk yazildiginda herhangi bir eslesme olmayacak sekilde tasarlandi.

                grid[x, y] = color;
            }
        }
        
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

        //eger solundaki iki eleman da boardun icerisinde ve ayni renkteyse match = true;

        if (y >= 2)
        // asagisindaki iki tas da ayni renkteyse true donecek.
        {
            TileColors down1 = grid[x, y - 1];
            TileColors down2 = grid[x, y - 2];

            if (down1 == color && down2 == color)
            {
                return true;
            }
        }


        if(x >= 1 && y >= 1) // sol , alt ve sol alt capraz ayni renkteyse 2x2 eslesme yakalanir.
        {
            TileColors left = grid[x - 1 , y];
            TileColors down = grid[x , y - 1];
            TileColors diagonal = grid[x - 1 , y - 1];

            if(down == color && left == color && diagonal == color)
            {
                return true;
            }
        }
        //eger asagisindaki/yukarisindaki eleman board un icerisinde ve ayni renkteyse match = true;
        
        return false;
        //eger ucu de yoksa false i cak.
    }
    IEnumerator CreateTiles()
    {
        tiles = new Tile[width, height];

        //8x8 bos bir grid aciliyor. Tas yaratmiyor sadece veri olarak tutuluyor.

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                SpawnTile(x,y);
                tiles[x,y].transform.position = new Vector2(x, y + height);
                tiles[x,y].MoveTo(new Vector2(x,y),dropDuration, 0);
            }
            yield return new WaitForSeconds(rowDelay * 3);

        }
        //hicbir tas komsusuna bakmadigi icin buradaki sira onemsiz.
    }
    
    void SpawnTile(int x , int y)
    {
        Vector2 pos = new Vector2(x,y);
        GameObject tileObject = Instantiate(tilePrefab , pos , Quaternion.identity);
        Tile tile = tileObject.GetComponent<Tile>();
        tile.x = x;
        tile.y = y;
        tiles[x,y] = tile;
        tileObject.GetComponent<SpriteRenderer>().sprite = GetSprite(grid[x,y]);
    }
    public Sprite GetSprite(TileColors tileColor)
    {
        if (tileColor == TileColors.Red) return sprites[0];
        if (tileColor == TileColors.Blue) return sprites[1];
        if (tileColor == TileColors.Green) return sprites[2];
        if (tileColor == TileColors.Yellow) return sprites[3];
        if (tileColor == TileColors.Purple) return sprites[4];

        return null;
    }
    IEnumerator TrySwap(Vector2 delta)
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
            yield break;
        }


        int startX = selectedTile.x;
        int startY = selectedTile.y;
        isAnimating = true;

        yield return StartCoroutine(SwapTiles(startX, startY, targetX, targetY));

        matchedTiles = FindMatches();

        if(matchedTiles.Count == 0)
        {
            yield return StartCoroutine(SwapTiles(startX, startY, targetX, targetY));
            isAnimating = false;
            yield break;
        }
        
        gameManager.MakeMove();
        yield return StartCoroutine(ResolveMatches());
        gameManager.GameOver();
        isAnimating = false;
    }

    IEnumerator SwapTiles(int x1, int y1, int x2, int y2)
    {
        //once temp color ve temp tile olusturuyor. Eger olusturmasaydi kaydirilan tasin rengi kaybolurdu.
        // yer degistirme icin once temp olusturuyor diyelim ki kirmizi
        // daha sonra ilk yerde olan kirmiziyi diger yerde olan maviyle degistiriyor.
        // mavi olan tempcolor a geciyor. Digeri mavi oluyor.
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


        tiles[x1, y1].MoveTo(new Vector2(x1,y1), swapDuration, 0);
        tiles[x2, y2].MoveTo(new Vector2(x2,y2), swapDuration, 0);
        yield return new WaitForSeconds(swapDuration);
    }

    HashSet<Tile> FindMatches() //liste kendisine ekstra bir sey eklenmesine izin verir fakat hashset izin vermez.
    //dolayisiyla burada buldugumuz eslesmelerdeki taslarin 2 kere patlamasini engellemis oluyoruz.
    {
        matchedTiles = new HashSet<Tile>();
        propellerPositions = new List<Vector2Int>();
        horizontalRocketPositions = new List<Vector2Int>();
        verticalRocketPositions = new List<Vector2Int>();

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

                    propellerPositions.Add(new Vector2Int(x,y));
                    //2x2 match
                }
                if (x + 2 < width && grid[x + 1, y] == color && grid[x + 2, y] == color)
                {
                    matchedTiles.Add(tiles[x, y]);
                    matchedTiles.Add(tiles[x + 1, y]);
                    matchedTiles.Add(tiles[x + 2, y]);
                    //1x3 horizontal match
                }

                if (y + 2 < height && grid[x, y + 1] == color && grid[x, y + 2] == color)
                {
                    matchedTiles.Add(tiles[x, y]);
                    matchedTiles.Add(tiles[x, y + 1]);
                    matchedTiles.Add(tiles[x, y + 2]);
                    //1x3 vertical match
                }

                if(x + 3 < width && grid[x + 1, y] == color && grid[x + 2, y] == color && grid[x + 3 , y] == color)
                {
                    matchedTiles.Add(tiles[x, y]);
                    matchedTiles.Add(tiles[x + 1, y]);
                    matchedTiles.Add(tiles[x + 2, y]);
                    matchedTiles.Add(tiles[x + 3 , y]);

                    //1x4 horizontal match

                    horizontalRocketPositions.Add(new Vector2Int(x,y));
                }
                
                if (y + 3 < height && grid[x, y + 1] == color && grid[x, y + 2] == color && grid[x, y + 3 ] == color)
                {
                    matchedTiles.Add(tiles[x, y]);
                    matchedTiles.Add(tiles[x, y + 1]);
                    matchedTiles.Add(tiles[x, y + 2]);
                    matchedTiles.Add(tiles[x , y + 3]);
                    //1x4 vertical match
                    verticalRocketPositions.Add(new Vector2Int(x,y));
                }


                // L and T matches will be added to the method.
                // the overlaping tile should be ignored after it was counted.
                
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

                        tile.MoveTo(new Vector2(x, emptyY), dropDuration , 0);
                    }
                    emptyY++;
                }
            }
        }
    }

    void FillBoard()
    {
        for(int x = 0; x < width; x++)
        {
            int emptyCount = 0;
            for(int y = 0; y < height; y++)
            {
                if(tiles[x,y] == null) emptyCount++;
            }
            for(int y = 0; y < height; y++)
            {
                if(tiles[x,y] == null)
                {
                    grid[x,y] = (TileColors)Random.Range(0,5);
                    SpawnTile(x,y);
                    tiles[x,y].transform.position = new Vector2(x,y + emptyCount);
                    tiles[x,y].MoveTo(new Vector2(x,y) , dropDuration , 0f);
                }
            }
        }
    }


    void IncreasePoints(int amount)
    {
        gameManager.ApplyPoints(amount);  
    }


    void SpawnPropeller(int x , int y)
    {
        Vector2 pos = new Vector2(x,y);
        GameObject propellerObject = Instantiate(propellerPrefab , pos , Quaternion.identity);
        Tile propeller = propellerObject.GetComponent<Tile>();
        propeller.x = x;
        propeller.y = y;
        tiles[x,y] = propeller;
        grid[x,y] = TileColors.None;
    }

    void SpawnHorizontalRocket(int x , int y)
    {
        Vector2 pos = new Vector2(x,y);
        GameObject horizontalRocketObject = Instantiate(horizontalRocketPrefab , pos , Quaternion.identity);
        Tile horizontalRocket = horizontalRocketObject.GetComponent<Tile>();
        horizontalRocket.x = x;
        horizontalRocket.y = y;
        tiles[x,y] = horizontalRocket;
        grid[x,y] = TileColors.None;
    }

    void SpawnVerticalRocket(int x , int y)
    {
        Vector2 pos = new Vector2(x,y);
        GameObject verticalRocketObject = Instantiate(verticalRocketPrefab , pos , Quaternion.identity);
        Tile verticalRocket = verticalRocketObject.GetComponent<Tile>();
        verticalRocket.x = x;
        verticalRocket.y = y;
        tiles[x,y] = verticalRocket;
        grid[x,y] = TileColors.None;
    }

    public TileColors GetColorAt(int x , int y)
    {
        return grid[x,y];
    }

    IEnumerator ActivateBooster(List<Vector2Int> targets)
    {
        isAnimating = true;
        HashSet<Tile> tileObjects = new HashSet<Tile>();
        foreach(Vector2Int pos in targets)
        {
           if(tiles[pos.x , pos.y] != null) 
           {
                tileObjects.Add(tiles[pos.x , pos.y]);  
           }
        }
    
        DestroyMatches(tileObjects);
        IncreasePoints(tileObjects.Count);
        gameManager.UpdatePoints();
        DropTiles();
        yield return new WaitForSeconds(dropDuration);
        FillBoard();
        yield return new WaitForSeconds(dropDuration);

        yield return StartCoroutine(ResolveMatches());
        isAnimating = false;

    }

    IEnumerator ResolveMatches()
    //finds every match on the board and makes them pop untill there are no more matches.
    {
        matchedTiles = FindMatches();

        while(matchedTiles.Count > 0)
        {
            DestroyMatches(matchedTiles);
            foreach(Vector2Int pos in propellerPositions)
            {
                SpawnPropeller(pos.x , pos.y);
            }
            foreach(Vector2Int pos in horizontalRocketPositions)
            {
                SpawnHorizontalRocket(pos.x , pos.y);
            }
            foreach(Vector2Int pos in verticalRocketPositions)
            {
                SpawnVerticalRocket(pos.x , pos.y);
            }
            IncreasePoints(matchedTiles.Count);
            gameManager.UpdatePoints();

            DropTiles();
            yield return new WaitForSeconds(dropDuration);
            FillBoard();
            yield return new WaitForSeconds(dropDuration);
            
            matchedTiles = FindMatches();
            //In order to restart the loop this was added.
        } 
    }
}
