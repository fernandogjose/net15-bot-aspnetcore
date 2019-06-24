using MongoDB.Bson;
using SimpleBotCore.Domain.Interfaces.DomainServices;
using SimpleBotCore.Domain.Interfaces.MongoRepository;
using SimpleBotCore.Domain.Models.Mongo;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleBotCore.Domain.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IChatCountRepository _chatCountRepository;

        public ChatService(IChatRepository chatRepository, IChatCountRepository chatCountRepository)
        {
            _chatRepository = chatRepository;
            _chatCountRepository = chatCountRepository;
        }

        public string GetMessage(string message)
        {
            string response = _chatRepository.GetMessage(message);
            return response;
        }

        public void AddMessage(ChatModel chatModel)
        {
            // Grava a mensagem
            _chatRepository.AddMessage(chatModel);

            // Grava a quantidade de mensagens por usuário
            var countByUser = _chatCountRepository.GetByUser(chatModel.UserId);

            var count = 1;
            if (countByUser != null)
            {
                count = Convert.ToInt32(countByUser.GetValue("Count")) + 1;
                _chatCountRepository.DeleteByUser(chatModel.UserId);
            }

            ChatCountModel chatCountModel = new ChatCountModel(chatModel.UserId, count.ToString());
            _chatCountRepository.Add(chatCountModel);
        }
    }
}
