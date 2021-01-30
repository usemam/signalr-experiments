using System.Threading.Tasks;

using Microsoft.AspNet.SignalR;

namespace FunctionalTests.Hubs
{
    public class EchoHub : Hub
    {
        public Task SendToUser(string userId, string message)
        {
            return Clients.User(userId).echo(message);
        }
    }
}