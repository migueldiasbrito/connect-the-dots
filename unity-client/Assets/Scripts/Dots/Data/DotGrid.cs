namespace Mdb.Ctd.Dots.Data
{
    public class DotGrid : IDotGrid
    {
        public IDot[,] Grid => Dots;

        public Dot[,] Dots;
    }
}
