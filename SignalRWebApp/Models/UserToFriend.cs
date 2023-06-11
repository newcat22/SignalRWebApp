using SqlSugar;

namespace SignalRWebApp.Models
{
    public class UserToFriend
    {        
        [SqlSugar.SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FriendId  { get; set; }
    }
}
