using Mdb.Ctd.Data;
using Mdb.Ctd.Dots.Config;
using Mdb.Ctd.Dots.Data;
using Mdb.Ctd.Dots.Services;
using Mdb.Ctd.Notifications;
using Mdb.Ctd.Services;
using UnityEngine;

namespace Mdb.Ctd.Boot
{
    public class Boot : MonoBehaviour
    {
        [SerializeField] private DotsConfig _dotsConfig;

        private void Awake()
        {
            NotificationService notificationService = new();
            ServiceLocator.Bind<INotificationService>(notificationService);

            DotGrid dotGrid = new();
            DataReaders.Bind<IDotGridDataReader>(dotGrid);

            DotsService dotsService = new(dotGrid, _dotsConfig, notificationService);
            ServiceLocator.Bind<IDotsService>(dotsService);
        }
    }
}
