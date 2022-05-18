using System;

namespace SymmetricShadowcasting
{
    public class Row
    {
        public int depth;
        public Fraction startSlope, endSlope;

        public Row(int depth, Fraction startSlope, Fraction endSlope)
        {
            this.depth = depth;
            this.startSlope = startSlope;
            this.endSlope = endSlope;
        }
        public QTile[] Tiles()
        {
            int minCol = RoundTiesUp(this.depth * this.startSlope);
            int maxCol = RoundTiesDown(this.depth * this.endSlope);
            QTile[] Tiles = new QTile[maxCol - minCol + 2];
            for (int i = 0; i < maxCol - minCol + 2; i++)
            {
                Tiles[i] = new QTile(this.depth, minCol + i);
            }
            return Tiles;
        }

        public Row Next()
        {
            ushort one = 1;
            return new Row(this.depth + one, this.startSlope, this.endSlope);
        }

        private static int RoundTiesUp(Fraction n)
        {
            Fraction temp = n + new Fraction(1, 2);
            return (int)Math.Floor(temp.Float());
        }

        private static int RoundTiesDown(Fraction n)
        {
            Fraction temp = n - new Fraction(1, 2);
            return (int)Math.Floor(temp.Float());
        }
    }
}