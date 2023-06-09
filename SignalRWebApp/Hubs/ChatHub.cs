using Microsoft.AspNetCore.SignalR;
using SignalRWebApp.Models;
using SqlSugar;

namespace SignalRWebApp.Hubs
{
    /// <summary>
    /// 创建SignalR中心
    /// </summary>
    public class ChatHub : Hub
    {
        private ISqlSugarClient _db;
        public ChatHub(ISqlSugarClient db)
        {
            _db = db;
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="user">用户名</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public async Task SendMessage(string user, string message)
        {
            DateTime time = DateTime.Now;
            //记录消息
            UserMessage userMessage = new UserMessage()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = user,
                Content = message,
                CreateTime = time
            };
            _db.Insertable(userMessage).ExecuteCommand();
            
            /*await Clients.User(user).SendAsync("ReceiveMessage", message);*/
            await Clients.All.SendAsync("ReceiveMessage", user, message, time.ToString());
            /* await Clients.All.SendAsync("ReceiveMessage", user, message, time.ToString());*/
        }


        public override async Task OnConnectedAsync()
        {
            var userId = Context.User.Identity.Name; // Replace this with actual user ID retrieval logic
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);

            await base.OnConnectedAsync();
        }

    }
}
