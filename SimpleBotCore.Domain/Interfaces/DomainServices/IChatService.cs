using SimpleBotCore.Domain.Models.Mongo;

namespace SimpleBotCore.Domain.Interfaces.DomainServices
{
    public interface IChatService
    {
        string GetMessage(string message);

        void AddMessage(ChatModel chatModel);
    }
}
