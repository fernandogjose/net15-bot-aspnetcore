namespace SimpleBotCore.Domain.Models.Mongo
{
    public class ChatModel
    {
        public string UserId { get; set; }

        public string Message { get; set; }

        public ChatModel(string userId, string message)
        {
            UserId = userId;
            Message = message;
        }
    }
}
