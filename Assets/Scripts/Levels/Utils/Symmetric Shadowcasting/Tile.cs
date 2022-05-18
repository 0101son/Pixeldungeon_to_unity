using UnityEngine;

namespace SymmetricShadowcasting
{
    public class QTile
    {
        public readonly int depth;
        public readonly int col;

        public QTile(int depth, int col)
        {
            if (depth < 0)
            {
                Debug.Log("Minus Depth Exception - spawn");
                UnityEditor.EditorApplication.isPlaying = false;
            }
            this.depth = depth;
            this.col = col;
        }
    }

}
