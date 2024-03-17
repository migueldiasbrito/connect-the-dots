using Mdb.Ctd.Data;

namespace Mdb.Ctd.Dots.Data
{
    public interface IDotGridDataReader : IDataReader
    {
        IDot[,] Grid { get; }

        bool CanConnect(int x1, int y1, int x2, int y2);
    }
}
