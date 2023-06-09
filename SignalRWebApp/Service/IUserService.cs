using SignalRWebApp.Models;

namespace SignalRWebApp.Service
{
    public interface IUserService
    {
        bool CodeFirst();


        UserInfo GetUser(string name, string password);

        /// <summary>
        /// 获取历史消息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        List<UserMessage> GetMessages(int pageIndex, int pageSize);

        /// <summary>
        /// 获取全部用户信息
        /// </summary>
        /// <returns></returns>
        List<UserInfo> getUserInfos();

        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<UserInfo> getFriendInfos(String userId);

        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="user1"></param>
        /// <param name="user2"></param>
        /// <returns></returns>
        int addFriend(String userId, String friendId);

        List<UserInfo> getUserInfosByName(String userName);

    }
}
