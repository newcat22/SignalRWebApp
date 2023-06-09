using SignalRWebApp.Models;
using SqlSugar;
using System.Reflection;
using System.Xml.Linq;

namespace SignalRWebApp.Service
{
    public class UserService : IUserService
    {
        private readonly ISqlSugarClient _db;
        public UserService(ISqlSugarClient db)
        {
            _db = db;
        }
        public bool CodeFirst()
        {
            try
            {
                //创建数据库
                _db.DbMaintenance.CreateDatabase();
                string nspace = "SignalRWebApp.Models";
                Type[] ass = Assembly.LoadFrom(AppContext.BaseDirectory + "SignalRWebApp.dll")
                    .GetTypes().Where(p => p.Namespace == nspace).ToArray();
                _db.CodeFirst.SetStringDefaultLength(200).InitTables(ass);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public UserInfo GetUser(string name, string password)
        {
            //查询用户是否存在
            UserInfo userInfo = _db.Queryable<UserInfo>().First(x => x.Name == name);
            if (userInfo == null)
            {
                UserInfo newInfo = new UserInfo()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = name,
                    Password = password
                };
                return _db.Insertable(newInfo).ExecuteReturnEntity();
            }
            else if (userInfo.Password != password)
            {
                return new UserInfo();
            }
            return userInfo;
        }

        public List<UserMessage> GetMessages(int pageIndex, int pageSize)
        {
            return _db.Queryable<UserMessage>().ToOffsetPage(pageIndex, pageSize);
        }

        public List<UserInfo> getUserInfos()
        {
            List<UserInfo> list = _db.Queryable<UserInfo>().ToList();
            return list;            
        }


        public List<UserInfo> getFriendInfos(string userId)
        {
            List<UserToFriend> userToFriends = _db.Queryable<UserToFriend>()
                                      .Where(u => u.UserId == userId)
                                      .ToList();
            List<UserInfo> userInfos = new List<UserInfo>();
            userToFriends.ForEach(u =>
            {                
                UserInfo userInfo = _db.Queryable<UserInfo>().First(x => x.Id == u.FriendId);
                userInfos.Add(userInfo);
            });
            return userInfos;
        }

        public int addFriend(String userId, String friendId)
        {
            
            UserToFriend newUserToFriend = new UserToFriend
            {
                UserId = userId,
                FriendId = friendId
            };
            bool isInserted = _db.Insertable(newUserToFriend).ExecuteCommandIdentityIntoEntity();
            return isInserted?1:0;
        }

        public List<UserInfo> getUserInfosByName(string userName)
        {
            List<UserInfo> userToFriends = _db.Queryable<UserInfo>()
                          .Where(u => u.Name == userName)
                          .ToList();
            return userToFriends;
        }
    }
}
