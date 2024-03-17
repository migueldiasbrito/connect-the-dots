using Mdb.Ctd.Services;

namespace Mdb.Ctd.Dots.Services
{
    public interface IDotsService : IService
    {
        void ConnectSequence((int x, int y)[] sequence);
    }
}
