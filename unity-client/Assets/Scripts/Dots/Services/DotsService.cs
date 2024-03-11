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
