using SqlSugar;

namespace SignalRWebApp.Models
{
    public class UserToFriend
    {
        [SugarColumn(IsPrimaryKey = true)]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FriendId  { get; set; }
    }
}
