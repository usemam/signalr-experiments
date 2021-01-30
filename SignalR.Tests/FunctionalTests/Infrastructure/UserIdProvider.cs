using Microsoft.AspNet.SignalR;

namespace FunctionalTests.Infrastructure
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request) => request.QueryString["userId"];
    }
}