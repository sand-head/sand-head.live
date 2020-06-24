using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;
using SandHeadLiveApi.Infrastructure;
using System.Threading.Tasks;

namespace SandHeadLiveApi.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserIdentifierCache _userIdCache;

        public ChatHub() : base()
        {
            _userIdCache = new UserIdentifierCache();
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("messageReceived", "Server", "0000", "Welcome to chat!");
            await base.OnConnectedAsync();
        }

        public async Task NewMessage(string username, string message)
        {
            var clientIp = Context.GetHttpContext().Connection.RemoteIpAddress;
            var identifier = _userIdCache.GetOrCreate(clientIp);
            await Clients.All.SendAsync("messageReceived", username, identifier, message);
        }
    }
}
