using UnityEngine;

namespace Mdb.Ctd.Dots.Config
{
    public interface IDotsConfig
    {
        int Width { get; }
        int Height { get; }
        int MaxNewCellValue { get; }
    }
}
