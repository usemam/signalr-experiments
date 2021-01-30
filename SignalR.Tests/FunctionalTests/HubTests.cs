using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using FunctionalTests.Infrastructure;

using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;

using Owin;

using Xunit;

namespace FunctionalTests
{
    public class HubTests
    {
        [Fact]
        public async Task SendToUser()
        {
            using (var host = new MemoryHost())
            {
                host.Configure(app =>
                {
                    var resolver = new DefaultDependencyResolver();
                    resolver.Register(typeof(IUserIdProvider), () => new UserIdProvider());
                    var config = new HubConfiguration { Resolver = resolver };
                    app.MapSignalR(config);
                });
                
                var connection1 = CreateHubConnection("user1");
                var connection2 = CreateHubConnection("user2");

                var wh1 = new TaskCompletionSource<object>();
                var wh2 = new TaskCompletionSource<object>();

                var hub1 = connection1.CreateHubProxy("EchoHub");
                var hub2 = connection2.CreateHubProxy("EchoHub");
                hub1.On("echo", () => wh1.TrySetResult(null));
                hub2.On("echo", () => wh2.TrySetResult(null));

                using (connection1)
                {
                    using (connection2)
                    {
                        await connection1.Start(host);
                        await connection2.Start(host);

                        await hub2.Invoke("SendToUser", "user1", "message");

                        await wh1.Task.OrTimeout();
                        Assert.False(wh2.Task.IsCompleted);
                    }
                }
            }
        }

        private HubConnection CreateHubConnection(string userId)
        {
            return new HubConnection(
                "http://memoryhost",
                new Dictionary<string, string> { {"userId", userId} });
        }
    }
}