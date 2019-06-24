namespace SimpleBotCore.Domain.Models.SqlServer
{
    public class ChatCountModel
    {
        public string UserId { get; set; }

        public string Count { get; set; }

        public ChatCountModel(string userId, string count)
        {
            UserId = userId;
            Count = count;
        }
    }
}
