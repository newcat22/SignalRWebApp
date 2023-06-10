using Microsoft.AspNetCore.SignalR;
using SignalRWebApp.Models;
using SqlSugar;
using System.Collections.Concurrent;

namespace SignalRWebApp.Hubs
{
    /// <summary>
    /// 创建SignalR中心
    /// </summary>
    public class ChatOneFriend : Hub
    {
        private ISqlSugarClient _db;
        public ChatOneFriend(ISqlSugarClient db)
        {
            _db = db;
        }
        public static ConcurrentDictionary<string, string> UserConnectionMap = new ConcurrentDictionary<string, string>();

        public override async Task OnConnectedAsync()
        {
            string userId = Context.GetHttpContext().Request.Headers["Id"];
            //var cookies = Context.GetHttpContext().Request.Cookies;
            // Assume you have the user's ID accessible when they connect
            //string userId = Context.User.Identity.Name; // Replace this with actual user ID retrieval logic
            UserConnectionMap.TryAdd(userId, Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {

            string userId = Context.User.Identity.Name; // Replace this with actual user ID retrieval logic
            UserConnectionMap.TryRemove(userId, out _);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string receiverId, string senderId, string message)
        {
            if (UserConnectionMap.TryGetValue(receiverId, out string connectionId))
            {
                // Send the message to the receiver
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", senderId, message);
            }
        }
    }
}
