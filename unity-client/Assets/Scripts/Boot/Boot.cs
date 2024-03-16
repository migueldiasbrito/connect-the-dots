using Mdb.Ctd.Data;
using Mdb.Ctd.Dots.Config;
using Mdb.Ctd.Dots.Data;
using Mdb.Ctd.Dots.Services;
using Mdb.Ctd.Services;
using UnityEngine;

namespace Mdb.Ctd.Boot
{
    public class Boot : MonoBehaviour
    {
        [SerializeField] private DotsConfig _dotsConfig;

        private void Awake()
        {
            DotGrid dotGrid = new();
            DataReaders.Bind<IDotGridDataReader>(dotGrid);

            DotsService dotsService = new(dotGrid, _dotsConfig);
            ServiceLocator.Bind<IDotsService>(dotsService);
        }
    }
}
