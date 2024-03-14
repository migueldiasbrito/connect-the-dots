using Mdb.Ctd.Data;
using Mdb.Ctd.Dots.Data;
using System.Collections;
using UnityEngine;

namespace Mdb.Ctd.Dots.Presentation
{
    public class GridUiDisplay : MonoBehaviour
    {
        [SerializeField] private Transform[] _dotHolders;
        [SerializeField] private DotUiDisplay _dotPrefab;

        private IDotGridDataReader _dotGridDataReader;

        private DotUiDisplay[,] _dotUiDisplayGrid;

        private void Start()
        {
            _dotGridDataReader = DataReaders.Get<IDotGridDataReader>();

            StartCoroutine(InitializeGrid());
        }

        private IEnumerator InitializeGrid()
        {
            yield return new WaitForEndOfFrame();

            IDot[,] grid = _dotGridDataReader.Grid;

            _dotUiDisplayGrid = new DotUiDisplay[grid.GetLength(0), grid.GetLength(1)];

            for (int x = 0; x < grid.GetLength(0); ++x)
            {
                for (int y = 0; y < grid.GetLength(1); ++y)
                {
                    if (grid[x, y] == null) continue;

                    _dotUiDisplayGrid[x,y] = Instantiate(_dotPrefab, _dotHolders[x + y * grid.GetLength(0)]);
                    _dotUiDisplayGrid[x, y].Setup(grid[x, y]);
                }
            }
        }
    }
}
