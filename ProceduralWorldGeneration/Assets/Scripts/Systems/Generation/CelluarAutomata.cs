using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelluarAutomata : MonoBehaviour
{
    public static int[,] GenerateCellularAutomata(int width, int height, int numberToGiveToCell, float seed, int fillPercent, bool edgesAreWalls)
    {
        System.Random rand = new System.Random(seed.GetHashCode());

        int[,] map = new int[width, height];

        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (edgesAreWalls && (x == 0 || x == map.GetUpperBound(0) - 1 || y == 0 || y == map.GetUpperBound(1) - 1))
                    map[x, y] = numberToGiveToCell;
                else
                    map[x, y] = (rand.Next(0, 100) < fillPercent) ? numberToGiveToCell : 0;
            }
        }

        return map;
    }

    static int GetMooreSurroundingTiles(int[,] map, int x, int y, bool edgesAreWalls)
    {
        int tileCount = 0;

        for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++)
        {
            for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < map.GetUpperBound(0) && neighbourY >= 0 && neighbourY < map.GetUpperBound(1))
                {
                    if (neighbourX != x || neighbourY != y)
                        tileCount += map[neighbourX, neighbourY];
                }
            }
        }
        return tileCount;
    }

    public static int[,] SmoothMooreCellularAutomata(int[,] map, int numberToGiveToCell, bool edgesAreWalls, int smoothCount)
    {
        for (int i = 0; i < smoothCount; i++)
        {
            for (int x = 0; x < map.GetUpperBound(0); x++)
            {
                for (int y = 0; y < map.GetUpperBound(1); y++)
                {
                    int surroundingTiles = GetMooreSurroundingTiles(map, x, y, edgesAreWalls);

                    if (edgesAreWalls && (x == 0 || x == (map.GetUpperBound(0) - 1) || y == 0))
                        map[x, y] = numberToGiveToCell;
                    else if (surroundingTiles > 4)
                        map[x, y] = numberToGiveToCell;
                    else if (surroundingTiles < 4)
                        map[x, y] = 0;
                }
            }
        }

        return map;
    }

    public static int[,] GenerateStoneAutomata(int width, int height, float seed, bool edgesAreWalls)
    {
        System.Random rand = new System.Random(seed.GetHashCode());
        int fillPercent = 3;
        int[,] map = new int[width, height];

        int y = 0;

        for (y = map.GetUpperBound(1); y > 0; y--)
        {
            if (y % 3 == 0)
                fillPercent += 1;

            for (int x = 0; x < map.GetUpperBound(0); x++)
            {
                if (edgesAreWalls && (x == 0 || x == map.GetUpperBound(0) - 1 || y == 0 || y == map.GetUpperBound(1) - 1))
                    map[x, y] = 2;
                else
                    map[x, y] = (rand.Next(0, 100) < fillPercent) ? 2 : 0;
            }
        }

        return map;
    }
}
