using SimpleBotCore.Domain.Models.Mongo;

namespace SimpleBotCore.Domain.Interfaces.MongoRepository
{
    public interface IChatRepository
    {
        string GetMessage(string message);

        void AddMessage(ChatModel chatModel);
    }
}
