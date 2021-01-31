using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;

using Newtonsoft.Json;

using Notifications.Core;

namespace Notifications.Web
{
    public class SignalrNotificationTransport : INotificationTransport
    {
        private readonly IHubContext<NotificationHub> hubContext;

        public SignalrNotificationTransport(IHubContext<NotificationHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public Task Notify(IEnumerable<NotificationMessage> messages)
        {
            string payload = JsonConvert.SerializeObject(messages);
            return hubContext.Clients.All.SendAsync("Notify", payload);
        }
    }
}