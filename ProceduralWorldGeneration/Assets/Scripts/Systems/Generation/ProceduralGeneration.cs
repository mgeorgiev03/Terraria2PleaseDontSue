using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] int width, height;
    float smoothness;
    [SerializeField] int minSmoothness = 12;
    [SerializeField] int maxSmoothness = 50;
    [SerializeField] float seed;
    [HideInInspector] public static CustomTile[] allTiles;
    [SerializeField] Tilemap groundTilemap, caveTilemap;
    [SerializeField] [Range(0, 100)] int stonePercent_Overwold;


    [Header("Caves")]
    [SerializeField] [Range(0, 100)] int percent;
    [SerializeField] [Range(0, 100)] int stonePercent_Cave;
    [SerializeField]  int smoothCount;

    int[,] map, caveMap, overworldMap;

    private void Start()
    {
        ItemDB.SaveToJSON<CustomTile>("Tiles");

        allTiles = ItemDB.ReadListFromJSON<CustomTile>("Tiles").ToArray();

        GenerateSmootheness();
        Generation();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GenerateSmootheness()
    {
        smoothness = Random.Range(minSmoothness, maxSmoothness);
    }

    private void Generation()
    {
        int x = Random.Range(1, width - 1);
        seed = Random.Range(-10000, 10000);
        clearMap();

        map = GenerateArray(width, height, true);
        overworldMap = GenerateArray(width, height / 3, true);
        caveMap = GenerateArray(width, height - (height / 3), true);

        caveMap = CelluarAutomata.GenerateCellularAutomata(width, height - (height / 3), 1, seed, percent, true);
        caveMap = DirectionalTunnel.GenerateDirectionalTunnelForCaveLayer(x, caveMap, 2, 4, 5, 2, 4);
        caveMap = CelluarAutomata.SmoothMooreCellularAutomata(caveMap, 1, true, smoothCount);
        int patches = (width * height * stonePercent_Cave * 5) / (100 * 15 *  17); //15, 17 avarage size of patches
        //int patches = width * height * (stonePercent_Cave / 100);
        caveMap = GenerateStone(caveMap, GetCustomTile("stone").ID, patches);

        overworldMap = TerrainGeneration(overworldMap, height / 3);
        int counter = 0;
        for (int y = overworldMap.GetUpperBound(1); y == 0; y--)
            if (map[x, y] == 0) counter++;
        overworldMap = DirectionalTunnel.GenerateDirectionalTunnelForSurfaceLayer(x, counter, overworldMap, 2, 4, 5, 2, 4);
        overworldMap = CelluarAutomata.SmoothMooreCellularAutomata(overworldMap, GetCustomTile("dirt").ID, false, smoothCount);

        map = CombineMaps(overworldMap, caveMap);
        RenderMap(map, groundTilemap, caveTilemap);
        CreateBorder();
    }

    public int[,] GenerateArray(int width, int height, bool empty)
    {
        int[,] map = new int[width, height];
        for (int x = 0; x < width; x++) 
        {
            for (int y = 0; y < height; y++)
                map[x, y] = (empty) ? GetCustomTile("cave").ID : GetCustomTile("dirt").ID;
        }

        return map;
    }

    public int[,] TerrainGeneration(int[,] map, int height)
    {
        int perlinHeight;
        for (int x = 0; x < width; x++)
        {
            perlinHeight = Mathf.RoundToInt(Mathf.PerlinNoise(x / smoothness, seed) * height);
            perlinHeight += Random.Range(0, 1);

            for (int y = 0; y < perlinHeight; y++) 
                map[x, y] = GetCustomTile("dirt").ID;

            if (x % 20 == 0)
                GenerateSmootheness();
        }

        return map;
    }

    public int[,] CombineMaps(int[,] overworldMap, int[,] caveMap)
    {
        int[,] combinedMap = new int[overworldMap.GetUpperBound(0), overworldMap.GetUpperBound(1) + caveMap.GetUpperBound(1)];

        for (int x = 0; x < caveMap.GetUpperBound(0); x++)
        {
            for (int y = 0; y < caveMap.GetUpperBound(1); y++)
                combinedMap[x, y] = caveMap[x, y];
        }

        for (int x = 0; x < overworldMap.GetUpperBound(0); x++)
        {
            for (int y = caveMap.GetUpperBound(1); y < overworldMap.GetUpperBound(1) + caveMap.GetUpperBound(1); y++)
                combinedMap[x, y] = overworldMap[x, y - caveMap.GetUpperBound(1)];
        }

        return combinedMap;
    }

    public void RenderMap(int[,] map, Tilemap groundTileMap, Tilemap caveTilemap)
    {
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (map[x, y] == GetCustomTile("dirt").ID)
                    groundTileMap.SetTile(new Vector3Int(x, y, 0), GetCustomTile("dirt").tile);
                else if (map[x, y] == GetCustomTile("stone").ID)
                    groundTilemap.SetTile(new Vector3Int(x, y, 0), GetCustomTile("stone").tile);
                else if (map[x, y] == GetCustomTile("cave").ID && y < caveMap.GetUpperBound(1))
                    caveTilemap.SetTile(new Vector3Int(x, y, 0), GetCustomTile("cave").tile);
            }
        }

    }

    private int[,] GenerateStone(int[,] map, int stoneID, int numberOfPathches)
    {
        for (int i = 0; i < numberOfPathches; i++)
        {
            int[,] stoneMap = new int[Random.Range(15, 50), Random.Range(10, 40)];
            stoneMap = CelluarAutomata.GenerateStoneAutomata(stoneMap.GetLength(0), stoneMap.GetLength(1), seed, false);
            stoneMap = CelluarAutomata.SmoothMooreCellularAutomata(stoneMap, stoneID, false, 2);

            int xStart = Random.Range(0, map.GetLength(0) - stoneMap.GetLength(0));
            int yStart = Random.Range(0, map.GetLength(1) - stoneMap.GetLength(1));

            for (int x = xStart; x < xStart + stoneMap.GetLength(0); x++)
            {
                for (int y = yStart; y < yStart + stoneMap.GetLength(1); y++)
                {
                    if (stoneMap[x - xStart, y - yStart] == stoneID && map[x, y] != GetCustomTile("cave").ID)
                        map[x, y] = stoneID;
                }
            }
        }

        return map;
    }

    private void CreateBorder()
    {
        //gen coordinates: 0,0
    }

    public void clearMap()
    {
        groundTilemap.ClearAllTiles();
        caveTilemap.ClearAllTiles();
    }

    public static CustomTile GetCustomTile(string name)
    {
        return allTiles.Where(t => t.name == name).First();
    }

    public static CustomTile GetCustomTile(int id)
    {
        return allTiles.Where(t => t.ID == id).First();
    }

    public static CustomTile GetCustomTile(Tile tile)
    {
        return allTiles.Where(t => t.tile == tile).First();
    }
}