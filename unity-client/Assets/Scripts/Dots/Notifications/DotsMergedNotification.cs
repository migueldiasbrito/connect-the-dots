using Mdb.Ctd.Dots.Data;
using Mdb.Ctd.Notifications;
using System.Collections.Generic;

namespace Mdb.Ctd.Dots.Notifications
{
    public struct DotsMergedNotification : INotification
    {
        public IDot UnifiedDot { get; }
        public IReadOnlyList<IDot> RemovedDots { get; }

        public DotsMergedNotification(IDot unifiedDot, IReadOnlyList<IDot> removedDots)
        {
            UnifiedDot = unifiedDot;
            RemovedDots = removedDots;
        }
    }
}
