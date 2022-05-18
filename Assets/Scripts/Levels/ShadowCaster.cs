using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SymmetricShadowcasting;

public static class ShadowCaster
{
    private static bool[,] blockingGrid;
    private static bool[,] fieldOfViewGrid;
    private static bool[,] visitedGrid;
    private static Vector2Int size;
    private static Vector2Int origin;
    private static int range;
    public static void CastShadow(Vector2Int position, bool[,] fieldOfView, bool[,] visited, bool[,] blocking, int distance)
    {
        blockingGrid = blocking;
        visitedGrid = visited;
        range = distance;
        origin = position;
        fieldOfViewGrid = fieldOfView;
        size.x = fieldOfView.GetLength(0);
        size.y = fieldOfView.GetLength(1);
        for (int y = 0; y < size.y; y++)
            for (int x = 0; x < size.x; x++)
                fieldOfView[x, y] = false;

        ShadowCastingCore.ComputeFov(origin, IsBlocking, MarkVisible);
    }

    private static bool IsBlocking(Vector2Int Tile)
    {
        if (IsInsideRange(Tile))
            return blockingGrid[Tile.x, Tile.y];
        else
            return true;
    }

    private static void MarkVisible(Vector2Int Tile)
    {
        if (IsInsideRange(Tile))
        {
            fieldOfViewGrid[Tile.x, Tile.y] = true;
            visitedGrid[Tile.x, Tile.y] = true;
        }
            
    }

    private static bool IsInsideRange(Vector2Int tile)
    {
        return (tile.x < 32) && (tile.x >= 0)
            && (tile.y < 32) && (tile.y >= 0)
        && (range >= Mathf.Abs(tile.x - origin.x))
        && (range >= Mathf.Abs(tile.y - origin.y));
    }
}
