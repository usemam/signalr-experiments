using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;

using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.Owin.Testing;

using Owin;

namespace FunctionalTests.Infrastructure
{
    public class MemoryHost : DefaultHttpClient, IDisposable
    {
        private static readonly string InstanceNameKey = $"{typeof(MemoryHost).FullName}.{nameof(InstanceName)}";
        
        private static int _instanceId;

        private TestServer _host;
        
        public string InstanceName { get; set; }

        public MemoryHost()
        {
            var id = Interlocked.Increment(ref _instanceId);
            InstanceName = Process.GetCurrentProcess().ProcessName + id;
        }

        public void Configure(Action<IAppBuilder> startup)
        {
            _host = TestServer.Create(app =>
            {
                app.Properties["host.AppName"] = InstanceName;

                // Add the memory host instance id to the Owin Environment so others can use it if necessary
                app.Use((context, next) =>
                {
                    context.Environment.Add(InstanceNameKey, InstanceName);
                    return next();
                });

                startup(app);
            });

            Initialize(null);
        }

        public void Dispose()
        {
            _host?.Dispose();
        }

        protected override HttpMessageHandler CreateHandler()
        {
            return new MemoryHostHttpHandler(_host.Handler);
        }
    }
}