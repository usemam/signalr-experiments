using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Notifications.Core
{
    public class NotificationConnector
    {
        private readonly INotificationSource source;
        private readonly INotificationTransport transport;

        public NotificationConnector(
            INotificationSource source, INotificationTransport transport)
        {
            this.source = source;
            this.transport = transport;
        }

        public Task Start(CancellationToken cancellationToken)
        {
            return Task.Run(() => RunLoop(cancellationToken));
        }

        private async Task RunLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                IEnumerable<NotificationMessage> messages = source.GetNotifications();
                if (messages.Any())
                {
                    await transport.Notify(messages);
                }

                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
            }
        }
    }
}