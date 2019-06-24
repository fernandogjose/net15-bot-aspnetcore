using MongoDB.Bson;
using MongoDB.Driver;
using SimpleBotCore.Infra.Mongo.Helpers;
using System.Collections.Generic;
using SimpleBotCore.Domain.Models.Mongo;
using SimpleBotCore.Domain.Interfaces.MongoRepository;

namespace SimpleBotCore.Infra.Mongo.Repositories
{
    public class ChatRepository: IChatRepository
    {
        private readonly MongoHelper _mongoHelper;

        public ChatRepository(MongoHelper mongoHelper)
        {
            _mongoHelper = mongoHelper;
        }

        public string GetMessage(string message)
        {
            var collection = _mongoHelper.MongoDatabase.GetCollection<BsonDocument>("Chat");
            FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter = builder.Eq("Message", message);
            BsonDocument bsonDocumentResponse = collection.Find(filter).FirstOrDefault();

            return bsonDocumentResponse == null ? "Registro não encontrado" : bsonDocumentResponse.GetValue("Message").ToString();
        }

        public void AddMessage(ChatModel chatModel)
        {
            var collectionChat = _mongoHelper.MongoDatabase.GetCollection<BsonDocument>("Chat");
            var bsonDocumentRequest = new BsonDocument(
                new Dictionary<string, string> {
                    { "UserId", chatModel.UserId },
                    { "Message", chatModel.Message }
                }
            );

            collectionChat.InsertOne(bsonDocumentRequest);
        }
    }
}
