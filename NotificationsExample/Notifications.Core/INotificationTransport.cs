using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notifications.Core
{
    public interface INotificationTransport
    {
        Task Notify(IEnumerable<NotificationMessage> messages);
    }
}
