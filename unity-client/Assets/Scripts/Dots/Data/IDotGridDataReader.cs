using Mdb.Ctd.Data;

namespace Mdb.Ctd.Dots.Data
{
    public interface IDotGridDataReader : IDataReader
    {
        IDot[,] Grid { get; }
    }
}
