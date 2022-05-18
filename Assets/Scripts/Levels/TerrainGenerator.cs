using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class TerrainGenerator
{
    // Start is called before the first frame update
    static public int[,] GetSample()
    {
        //Debug.Log("Gotta");
        int[,] sample = new int[32, 32];
        Painter.BoxFill(sample, 1, 0);

        for(int i=1; i<30; i+=10)
        {
            for(int j=1; j<30; j+=10)
            {
                int[,] temp = new int[10, 10];
                Vector2Int bottomLeft = new Vector2Int(Random.Range(1, 5), Random.Range(1, 5));
                Vector2Int topRight = new Vector2Int(Random.Range(5, 9), Random.Range(5, 9));
                Painter.BoxFull(temp, bottomLeft, topRight, 1);
                bottomLeft = new Vector2Int(j, i);
                Painter.Paste(sample, bottomLeft, temp);
            }
        }

        return sample;
    }
}
