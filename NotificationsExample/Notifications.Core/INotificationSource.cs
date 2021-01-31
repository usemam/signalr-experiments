using System.Collections.Generic;

namespace Notifications.Core
{
    public interface INotificationSource
    {
        IEnumerable<NotificationMessage> GetNotifications();
    }
}