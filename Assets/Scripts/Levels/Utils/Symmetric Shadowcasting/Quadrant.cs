using UnityEngine;

namespace SymmetricShadowcasting
{
    public class Quadrant
    {
        private const int North = 0, East = 1, South = 2, West = 3;

        private readonly int cardinal;
        private Vector2Int origin;

        public Quadrant(int cardinal, Vector2Int origin)
        {
            if ((cardinal < 0) || (cardinal > 3))
            {
                Debug.Log("Cardinal Out Of Range Exception - spawn");
                UnityEditor.EditorApplication.isPlaying = false;
            }
            this.cardinal = cardinal;
            this.origin = origin;
        }

        public Vector2Int Transform(QTile tile)
        {
            return cardinal switch
            {
                North => new Vector2Int(origin.x + tile.col, origin.y - tile.depth),
                East => new Vector2Int(origin.x + tile.col, origin.y + tile.depth),
                South => new Vector2Int(origin.x + tile.depth, origin.y - tile.col),
                West => new Vector2Int(origin.x - tile.depth, origin.y - tile.col),
                _ => new Vector2Int()
            };
        }
    }

}