using Mdb.Ctd.Data;
using Mdb.Ctd.Dots.Data;
using UnityEngine;

namespace Mdb.Ctd.Dots.Presentation
{
    public class GridUiDisplay : MonoBehaviour
    {
        [SerializeField] private Transform[] _dotHolders;
        [SerializeField] private DotUiDisplay _dotPrefab;

        private IDotGridDataReader _dotGridDataReader;

        private void Start()
        {
            _dotGridDataReader = DataReaders.Get<IDotGridDataReader>();

            InitializeGrid();
        }

        private void InitializeGrid()
        {
            IDot[,] grid = _dotGridDataReader.Grid;

            for (int x = 0; x < grid.GetLength(0); ++x)
            {
                for (int y = 0; y < grid.GetLength(1); ++y)
                {
                    if (grid[x, y] == null) continue;

                    Instantiate(_dotPrefab, _dotHolders[x + y * grid.GetLength(0)]);
                }
            }
        }
    }
}
