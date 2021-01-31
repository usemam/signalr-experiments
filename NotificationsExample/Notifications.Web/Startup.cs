using System.Threading;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Notifications.Core;

namespace Notifications.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<INotificationSource, RandomNotificationSource>();
            services.AddSingleton<INotificationTransport, SignalrNotificationTransport>();
            services.AddSingleton<NotificationConnector>();

            services.AddSignalR();
        }

        public void Configure(
            IApplicationBuilder app, IWebHostEnvironment env, NotificationConnector connector)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("/notificationHub");
            });

            connector.Start(default(CancellationToken));
        }
    }
}
