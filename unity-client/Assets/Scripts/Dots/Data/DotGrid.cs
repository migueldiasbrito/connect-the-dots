using System.Collections.Generic;
using System.Linq;

namespace Mdb.Ctd.Dots.Data
{
    public class DotGrid : IDotGridDataReader
    {
        public IDot[,] Grid => Dots;

        public Dot[,] Dots;

        public bool CanConnect(int x1, int y1, int x2, int y2)
        {
            if (x1 == x2 && y1 == y2) return false;

            if (!IsPositionWithinGrid(x1, y1) || !IsPositionWithinGrid(x2, y2)) return false;

            Dot dot1 = Dots[x1, y1];
            Dot dot2 = Dots[x2, y2];

            if (dot1 == null || dot2 == null) return false;

            if (dot1.Value != dot2.Value) return false;

            if (x1 == x2 && (y1 == y2 + 1 || y1 == y2 - 1)) return true;

            if (y1 == y2 && (x1 == x2 + 1 || x1 == x2 - 1)) return true;

            if ((x1 == x2 + 1 || x1 == x2 - 1) && (y1 == y2 + 1 || y1 == y2 - 1)) return true;

            return false;
        }

        public int GetSequenceValue((int x, int y)[] sequence)
        {
            if (sequence.Length == 0) return -1;

            if (!sequence.All(position => IsPositionWithinGrid(position.x, position.y))) return -1;

            Dot first = Dots[sequence[0].x, sequence[0].y];

            if (sequence.Length == 1)
                return first != null ? Grid[sequence[0].x, sequence[0].y].Value : -1;

            List<(int x, int y)> visited = new () { sequence[0] };
            for (int i = 1; i < sequence.Length; ++i)
            {
                if (visited.Any(position => position.x == sequence[i].x && position.y == sequence[i].y)) return -1;

                if (!CanConnect(sequence[i - 1].x, sequence[i - 1].y, sequence[i].x, sequence[i].y)) return -1;

                visited.Add(sequence[i]);
            }

            return first.Value * 2 * (sequence.Length / 2);
        }

        private bool IsPositionWithinGrid(int x, int y)
        {
            return x >= 0 || x < Dots.GetLength(0) || y >= 0 || y < Dots.GetLength(1);
        }
    }
}
