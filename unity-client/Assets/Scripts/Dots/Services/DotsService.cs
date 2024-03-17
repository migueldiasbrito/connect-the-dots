using Mdb.Ctd.Dots.Config;
using Mdb.Ctd.Dots.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Mdb.Ctd.Dots.Services
{
    public class DotsService : IDotsService
    {
        private DotGrid _model;

        private int[] _possibleNewValues;

        public DotsService(DotGrid model, IDotsConfig config)
        {
            _model = model;
            _model.Dots = new Dot[config.Width, config.Height];

            List<int> possibleNewValues = new();
            int lastValue = 1;
            do
            {
                lastValue *= 2;
                possibleNewValues.Add(lastValue);
            } while (lastValue < config.MaxNewCellValue);
            _possibleNewValues = possibleNewValues.ToArray();

            FillEmptyGridSpaces();
        }

        public void ConnectSequence((int x, int y)[] sequence)
        {
            if (sequence.Length <= 1) return;

            int value = _model.GetSequenceValue(sequence);

            if (value == -1) return;

            _model.Dots[sequence[^1].x, sequence[^1].y].Value = value;
            // notify value change

            for (int i = 0; i < sequence.Length - 1; ++i)
            {
                _model.Dots[sequence[i].x, sequence[i].y] = null;
                // notify deleted dot
            }

            UpdateGridAfterSequence();
        }

        private void UpdateGridAfterSequence()
        {
            PullDownDotsOnRowsWithEmptyCells();
            FillEmptyGridSpaces();

            // notify state updated
        }

        private void PullDownDotsOnRowsWithEmptyCells()
        {
            for (int x = 0; x < _model.Dots.GetLength(0); ++x)
            {
                int emptyCells = 0;
                for (int y = 0; y < _model.Dots.GetLength(1); ++y)
                {
                    if (_model.Dots[x, y] == null)
                    {
                        ++emptyCells;
                    }
                    else if (emptyCells > 0)
                    {
                        _model.Dots[x, y - emptyCells] = _model.Dots[x, y];
                        _model.Dots[x, y] = null;
                    }
                }
            }
        }

        private void FillEmptyGridSpaces()
        {
            for (int x = 0; x < _model.Dots.GetLength(0); ++x)
            {
                for (int y = _model.Dots.GetLength(1) - 1; y >= 0; --y)
                {
                    if (_model.Dots[x, y] != null) break;

                    _model.Dots[x, y] = new Dot {
                        X = x,
                        Y = y,
                        Value = _possibleNewValues[Random.Range(0, _possibleNewValues.Length)]
                    };
                }
            }
        }
    }
}
