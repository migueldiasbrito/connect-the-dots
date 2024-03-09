using Mdb.Ctd.Data;

namespace Mdb.Ctd.Dots.Data
{
    public interface IDot : IDataReader
    {
        int Value { get; }
        int X { get; }
        int Y { get; }
    }
}
