using Dapper;
using SimpleBotCore.AppService.Dtos;
using SimpleBotCore.Domain.Interfaces.SqlServerRepository;
using SimpleBotCore.Domain.Models.SqlServer;
using System.Data;
using System.Data.SqlClient;

namespace SimpleBotCore.Infra.SqlServer.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ConnectionStringDto _connectionStringDto;

        public ChatRepository(ConnectionStringDto connectionStringDto)
        {
            _connectionStringDto = connectionStringDto;
        }

        public string GetMessage(string message)
        {
            ChatModel response;

            using (SqlConnection conn = new SqlConnection(_connectionStringDto.SqlServer))
            {
                // SELECT Message FROM Chat WHERE Message = @Message
                CommandDefinition commandDefinition = new CommandDefinition("sprGetMessage", new { Message = message }, null, null, CommandType.StoredProcedure);
                response = conn.QueryFirst<ChatModel>(commandDefinition);
            }

            return response.Message;
        }

        public void AddMessage(ChatModel chatModel)
        {
            using (SqlConnection conn = new SqlConnection(_connectionStringDto.SqlServer))
            {
                // INSERT INTO Chat (UserId, Message) VALUES (@UserId, @Message)
                conn.Execute("sprAddMessage", new
                {
                    UserId = chatModel.UserId,
                    Message = chatModel.Message
                }, null, null, CommandType.StoredProcedure);
            }
        }
    }
}
