using Mdb.Ctd.Data;
using Mdb.Ctd.Dots.Data;
using System;
using UnityEngine;

namespace Mdb.Ctd.Dots.Presentation
{
    public class GridUiDisplay : MonoBehaviour
    {
        [SerializeField] private Transform[] _dotHolders;
        [SerializeField] private DotUiDisplay _dotPrefab;

        private IDotGrid _dotGrid;

        private void Start()
        {
            _dotGrid = DataReaders.Get<IDotGrid>();

            InitializeGrid();
        }

        private void InitializeGrid()
        {
            IDot[,] grid = _dotGrid.Grid;

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
