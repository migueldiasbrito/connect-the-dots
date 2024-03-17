using Mdb.Ctd.Services;
using System;

namespace Mdb.Ctd.Notifications
{
    public interface INotificationService : IService
    {
        void Subscribe<T>(Action<T> callback) where T : INotification;
        void Unsubscribe<T>(Action<T> callback) where T : INotification;
        void Publish<T>(T notification) where T : INotification;
    }
}
