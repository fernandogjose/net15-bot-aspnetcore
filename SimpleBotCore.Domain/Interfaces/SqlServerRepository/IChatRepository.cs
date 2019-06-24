using SimpleBotCore.Domain.Models.SqlServer;

namespace SimpleBotCore.Domain.Interfaces.SqlServerRepository
{
    public interface IChatRepository
    {
        string GetMessage(string message);

        void AddMessage(ChatModel chatModel);
    }
}
