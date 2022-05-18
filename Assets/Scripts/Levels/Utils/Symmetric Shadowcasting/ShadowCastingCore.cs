using System;
using UnityEngine;

namespace SymmetricShadowcasting
{
    public static class ShadowCastingCore
    {
        public static void ComputeFov(Vector2Int origin, Func<Vector2Int, bool> IsBlocking, Action<Vector2Int> MarkVisible)
        {

            MarkVisible(origin);

            for (int i = 0; i < 4; i++)
            {
                Quadrant quadrant = new Quadrant(i, origin);

                void Reveal(in QTile tile)
                {
                    Vector2Int toReveal = quadrant.Transform(tile);
                    MarkVisible(toReveal);
                }

                bool IsWall(in QTile tile)
                {
                    if (tile == null)
                        return false;
                    Vector2Int toCheck = quadrant.Transform(tile);
                    return IsBlocking(toCheck);
                }

                bool IsFloor(in QTile tile)
                {
                    if (tile == null)
                        return false;
                    Vector2Int toCheck = quadrant.Transform(tile);
                    return !IsBlocking(toCheck);
                }

                void Scan(in Row row)
                {
                    QTile prevTile = null;
                    foreach (QTile tile in row.Tiles())
                    {
                        if(IsWall(tile) || IsSymmetric(row, tile))
                            Reveal(tile);
                        if(IsWall(prevTile) && IsFloor(tile))
                            row.startSlope = Slope(tile);
                        if(IsFloor(prevTile) && IsWall(tile))
                        { 
                            Row nextRow = row.Next();
                            nextRow.endSlope = Slope(tile);
                            Scan(nextRow);
                        }
                        prevTile = tile;
                    }

                    if(IsFloor(prevTile))
                        Scan(row.Next());
                }

                Row firstRow = new Row(1, new Fraction(-1), new Fraction(1));
                Scan(firstRow);
            }
        }

        private static Fraction Slope(in QTile tile)
        {
            return new Fraction(2 * tile.col - 1, 2 * tile.depth);
        }

        private static bool IsSymmetric(in Row row, in QTile tile)
        {
            return ((tile.col >= row.depth * row.startSlope) && (tile.col <= row.depth * row.endSlope));
        }

    }
}
