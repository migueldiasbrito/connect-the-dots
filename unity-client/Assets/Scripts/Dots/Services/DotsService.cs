using Mdb.Ctd.Dots.Config;
using Mdb.Ctd.Dots.Data;

namespace Mdb.Ctd.Dots.Services
{
    public class DotsService : IDotsService
    {
        private DotGrid _model;

        public DotsService(IDotsConfig config)
        {
            _model = new DotGrid
            {
                Dots = new Dot[config.Width][]
            };

            // TODO (MB) can this be improved?
            for (int i = 0; i < _model.Dots.Length; ++i)
            {
                _model.Dots[i] = new Dot[config.Height];
            }
        }
    }
}
