using Mdb.Ctd.Data;

namespace Mdb.Ctd.Dots.Data
{
    public interface IDotGrid : IDataReader
    {
        IDot[][] Grid { get; }
    }
}
