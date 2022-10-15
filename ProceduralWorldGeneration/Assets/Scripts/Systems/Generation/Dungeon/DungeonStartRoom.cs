using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonStartRoom : MonoBehaviour
{
    [SerializeField] public Tile TempleTile;
    public int width, height;
    public int[,] map;

    public void GenerateDungeon()
    {
        map = GenerateArray(width, height, true);
        Dungeon.GenerateMainDungeon();
    }

    private int[,] GenerateArray(int width, int height, bool empty)
    {
        int[,] map = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
                map[x, y] = (empty) ? 0 : 1;
        }

        return map;
    }

    private int[,] GenerateStarterRoom(int[,] map)
    {


        return map;
    }
}
