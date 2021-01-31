using System;
using System.Collections.Generic;

using Notifications.Core;

namespace Notifications.Web
{
    public class RandomNotificationSource : INotificationSource
    {
        private readonly Random random = new Random();

        public IEnumerable<NotificationMessage> GetNotifications()
        {
            int messageCount = random.Next(10);
            var messages = new List<NotificationMessage>();
            for (int i = 0; i < messageCount; i++)
            {
                messages.Add(new NotificationMessage { Content = random.Next(10).ToString() });
            }

            return messages;
        }
    }
}