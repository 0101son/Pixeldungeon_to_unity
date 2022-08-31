using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class TerrainGenerator
{
    static public int[,] GetSample()
    {
        System.Random r = new();

        Debug.Log("T: Getting Sample");
        int[,] sample = new int[32, 32];
        Painter.BoxFill(sample, 1, 0);
        Debug.Log("T: Border");

        for (int i=1; i<30; i+=10)
        {
            for(int j=1; j<30; j+=10)
            {
                int[,] temp = new int[10, 10];
                Vector2Int bottomLeft = new Vector2Int(r.Next(1, 5), r.Next(1, 5));
                Vector2Int topRight = new Vector2Int(r.Next(5, 9), r.Next(5, 9));
                Painter.BoxFull(temp, bottomLeft, topRight, 1);
                bottomLeft = new Vector2Int(j, i);
                Painter.Paste(sample, bottomLeft, temp);
            }
        }
        Debug.Log("T: Getting Sample fin");
        return sample;
    }
}
