namespace SimpleBotCore.Domain.Models.Mongo
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
