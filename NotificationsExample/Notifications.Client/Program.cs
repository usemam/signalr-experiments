using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR.Client;

namespace Notifications.Client
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            string notificationHubUrl = "http://localhost:5000/notificationHub";
            var connection = new HubConnectionBuilder()
                .WithUrl(notificationHubUrl)
                .WithAutomaticReconnect()
                .Build();
            connection.On<string>("Notify", message =>
            {
                Console.WriteLine(message);
            });

            await connection.StartAsync();

            Console.WriteLine("Press ENTER to exit...");
            Console.ReadLine();
        }
    }
}
