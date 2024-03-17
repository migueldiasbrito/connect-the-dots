namespace Mdb.Ctd.Dots.Data
{
    public class DotGrid : IDotGridDataReader
    {
        public IDot[,] Grid => Dots;

        public Dot[,] Dots;

        public bool CanConnect(int x1, int y1, int x2, int y2)
        {
            if (x1 == x2 && y1 == y2) return false;

            if (x1 < 0 || x1 >= Dots.GetLength(0) || y1 < 0 || y1 >= Dots.GetLength(1) ||
                x2 < 0 || x2 >= Dots.GetLength(0) || y2 < 0 || y2 >= Dots.GetLength(1))
                return false;

            Dot dot1 = Dots[x1, y1];
            Dot dot2 = Dots[x2, y2];

            if (dot1 == null || dot2 == null) return false;

            if (dot1.Value != dot2.Value) return false;

            if (x1 == x2 && (y1 == y2 + 1 || y1 == y2 - 1)) return true;

            if (y1 == y2 && (x1 == x2 + 1 || x1 == x2 - 1)) return true;

            if ((x1 == x2 + 1 || x1 == x2 - 1) && (y1 == y2 + 1 || y1 == y2 - 1)) return true;

            return false;
        }
    }
}
