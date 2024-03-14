namespace Mdb.Ctd.Dots.Data
{
    public class DotGrid : IDotGridDataReader
    {
        public IDot[,] Grid => Dots;

        public Dot[,] Dots;
    }
}
