using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Painter
{
    //Level의 map[,] 을 생성하기 위한 도구 모음집
    public static void Point(int[,] map, Vector2Int cell, int value)
    {
        map[cell.x, cell.y] = value;
    }

    public static void BoxFull(int[,] map, Vector2Int bottomLeft, Vector2Int topRight, int value)
    {
        for (int i = bottomLeft.y; i <= topRight.y; i++)
        {
            for (int j = bottomLeft.x; j <= topRight.x; j++)
            {
                map[j, i] = value;
            }
        }
    }

    public static void BoxFull(int[,] map, int value)
    {
        for (int i = 0; i < map.GetLength(1); i++)
        {
            for (int j = 0; j < map.GetLength(0); j++)
            {
                map[j, i] = value;
            }
        }
    }

    public static void BoxFill(int[,] map, Vector2Int bottomLeft, Vector2Int topRight, int line, int fill)
    {
        BoxFull(map, bottomLeft, topRight, line);
        Vector2Int fillBottomLeft = bottomLeft + new Vector2Int(1, 1);
        Vector2Int fillTopRight = topRight - new Vector2Int(1, 1);
        BoxFull(map, fillBottomLeft, fillTopRight, fill);
    }

    public static void BoxFill(int[,] map, int line, int fill)
    {
        BoxFull(map, line);
        Vector2Int fillBottomLeft = new Vector2Int(1, 1);
        Vector2Int fillTopRight = new Vector2Int(map.GetLength(0) -2, map.GetLength(1) -2);
        BoxFull(map, fillBottomLeft, fillTopRight, fill);
    }

    public static void Paste(int[,] map, Vector2Int bottomLeft, int[,] copy)
    {
        Vector2Int copySize = new Vector2Int(copy.GetLength(0), copy.GetLength(1));
        for (int i = 0; i < copySize.y; i++)
        {
            for (int j = 0; j < copySize.x; j++)
            {
                map[j+ bottomLeft.x, i + bottomLeft.y] = copy[j,i];
            }
        }
    }

    // Start is called before the first frame update

}


// (x - 2) 를 n*P 로 나눈 나머지가 p-1의 배수여야 함
// ((x - 2)%nP)%(p-1)=0

// if( ( (x-2) - (P-1)*n ) %  )
// 3
// 3 -> 1
// 4 -> 
//
//
//
//