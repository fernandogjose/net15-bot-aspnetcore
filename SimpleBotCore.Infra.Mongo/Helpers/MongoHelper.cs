using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SimpleBotCore.AppService.Dtos;
using System;

namespace SimpleBotCore.Infra.Mongo.Helpers
{
    public class MongoHelper
    {
        private readonly IOptions<ConnectionStringDto> _connectionString;
        public IMongoDatabase MongoDatabase;

        public MongoHelper(IOptions<ConnectionStringDto> connectionString)
        {
            _connectionString = connectionString;
            CreateMongoDatabase();
        }

        private void CreateMongoDatabase()
        {
            MongoClient client = new MongoClient(_connectionString.Value.MongoDB);
            MongoDatabase = client.GetDatabase("Bot");            
        }
    }
}
