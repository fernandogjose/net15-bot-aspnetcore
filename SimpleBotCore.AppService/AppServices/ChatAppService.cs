using SimpleBotCore.AppService.Dtos;
using SimpleBotCore.Domain.Interfaces.DomainServices;
using SimpleBotCore.Domain.Models.Mongo;

namespace SimpleBotCore.AppService.AppServices
{
    public class ChatAppService
    {
        private readonly IChatService _chatService;

        public ChatAppService(IChatService chatService)
        {
            _chatService = chatService;
        }

        public string GetMessage(string message)
        {
            string response = _chatService.GetMessage(message);
            return response;
        }

        public void AddMessage(ChatDto chatDto)
        {
            ChatModel chatModel = new ChatModel(chatDto.UserId, chatDto.Message);
            _chatService.AddMessage(chatModel);
        }
    }
}
