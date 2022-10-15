using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalTunnel : MonoBehaviour
{
    public static int[,] GenerateDirectionalTunnelForSurfaceLayer(int startPointX, int yOffset, int[,] map, int minPathWidth, int maxPathWidth, int maxPathChange, int roughness, int curvyness)
    {
        int tunnelWidth = 1;
        System.Random rand = new System.Random(Time.time.GetHashCode());

        for (int i = -tunnelWidth; i <= tunnelWidth; i++)
        {
            map[startPointX + i, 0] = 0;
        }

        for (int y = 1; y < map.GetUpperBound(1) - yOffset; y++)
        {
            if (rand.Next(0, 100) > roughness)
            {
                int widthChange = Random.Range(-maxPathWidth, maxPathWidth);
                tunnelWidth += widthChange;

                if (tunnelWidth < minPathWidth)
                    tunnelWidth = minPathWidth;

                if (tunnelWidth > maxPathWidth)
                    tunnelWidth = maxPathWidth;
            }

            if (rand.Next(0, 100) > curvyness)
            {
                float xChange = Mathf.PerlinNoise(maxPathChange, 0);
                maxPathChange += Random.Range(0, 1);
                float neg = Random.Range(-maxPathChange, maxPathChange);


                if (neg < 0)
                    startPointX -= Mathf.RoundToInt(xChange * 4);
                else
                    startPointX += Mathf.RoundToInt(xChange * 4);


                if (startPointX < maxPathWidth)
                    startPointX = maxPathWidth;

                if (startPointX > (map.GetUpperBound(0) - maxPathWidth))
                    startPointX = map.GetUpperBound(0) - maxPathWidth;
            }

            for (int i = -tunnelWidth; i <= tunnelWidth; i++)
            {
                map[startPointX + i, y] = 0;
            }
        }

        return map;
    }


    public static int[,] GenerateDirectionalTunnelForCaveLayer(int startPointX, int[,] map, int minPathWidth, int maxPathWidth, int maxPathChange, int roughness, int curvyness)
    {
        int tunnelWidth = 1;
        System.Random rand = new System.Random(Time.time.GetHashCode());

        for (int i = -tunnelWidth; i <= tunnelWidth; i++)
        {
            map[startPointX + i, map.GetUpperBound(1)] = 0;
        }

        for (int y = map.GetUpperBound(1) - 1; y > map.GetUpperBound(1) - 45; y--)
        {
            if (rand.Next(0, 100) > roughness)
            {
                float widthChange = Mathf.PerlinNoise(maxPathWidth, 0) * 10;
                maxPathWidth += Random.Range(0, 1);
                float neg = Random.Range(-maxPathChange, maxPathChange);


                if (neg < 0)
                    tunnelWidth -= Mathf.RoundToInt(widthChange * 2);
                else
                    tunnelWidth += Mathf.RoundToInt(widthChange * 2);


                if (tunnelWidth < minPathWidth)
                    tunnelWidth = minPathWidth;

                if (tunnelWidth > maxPathWidth)
                    tunnelWidth = maxPathWidth;
            }

            if (rand.Next(0, 100) > curvyness)
            {
                float xChange = Mathf.PerlinNoise(maxPathChange, 0);
                maxPathChange += Random.Range(0, 1);
                float neg = Random.Range(-maxPathChange, maxPathChange);


                if (neg < 0)
                    startPointX -= Mathf.RoundToInt(xChange * 4);
                else
                    startPointX += Mathf.RoundToInt(xChange * 4);


                if (startPointX < maxPathWidth)
                    startPointX = maxPathWidth;

                if (startPointX > (map.GetUpperBound(0) - maxPathWidth))
                    startPointX = map.GetUpperBound(0) - maxPathWidth;
            }

            for (int i = -tunnelWidth; i <= tunnelWidth; i++)
            {
                map[startPointX + i, y] = 0;
            }
        }

        return map;
    }
}
