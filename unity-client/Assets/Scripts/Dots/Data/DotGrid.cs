using System.Linq;

namespace Mdb.Ctd.Dots.Data
{
    public class DotGrid : IDotGrid
    {
        public IDot[][] Grid => Dots.Select(line => line.Select(dot => (IDot) dot).ToArray()).ToArray();

        public Dot[][] Dots;
    }
}
