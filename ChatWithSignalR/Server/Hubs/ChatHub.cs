using Microsoft.AspNetCore.SignalR;
using ChatWithSignalR.Client.Pages;

namespace ChatWithSignalR.Server.Hubs
{
    public class ChatHub : Hub
    {
        private static Dictionary<string, string> Users = new Dictionary<string, string>();
        public override async Task OnConnectedAsync()
        {
            string username = Context.GetHttpContext().Request.Query["username"];
            Users.Add(Context.ConnectionId, username);
            await SendNewMessege(string.Empty, $"{username} joined!");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string username = Users.FirstOrDefault(n => n.Key == Context.ConnectionId).Value;
            await SendNewMessege(string.Empty, $"{username} left chat!");
        }

        //send msg for all users in hub
        public async Task SendNewMessege(string user, string message)
        {
            await Clients.All.SendAsync("getMessage", user, message);
        }
    }
}
