using MongoDB.Bson;
using MongoDB.Driver;
using SimpleBotCore.Infra.Mongo.Helpers;
using System.Collections.Generic;
using SimpleBotCore.Domain.Models.Mongo;
using SimpleBotCore.Domain.Interfaces.MongoRepository;

namespace SimpleBotCore.Infra.Mongo.Repositories
{
    public class ChatCountRepository : IChatCountRepository
    {
        private readonly MongoHelper _mongoHelper;

        public ChatCountRepository(MongoHelper mongoHelper)
        {
            _mongoHelper = mongoHelper;
        }

        public BsonDocument GetByUser(string userId)
        {
            var collection = _mongoHelper.MongoDatabase.GetCollection<BsonDocument>("ChatCount");
            FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter = builder.Eq("UserId", userId);
            BsonDocument bsonDocumentResponse = collection.Find(filter).FirstOrDefault();

            return bsonDocumentResponse;
        }

        public void DeleteByUser(string userId)
        {
            var collection = _mongoHelper.MongoDatabase.GetCollection<BsonDocument>("ChatCount");
            FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter = builder.Eq("UserId", userId);
            collection.DeleteOne(filter);
        }

        public void Add(ChatCountModel chatCountModel)
        {
            var bsonDocumentRequestChatCount = new BsonDocument(
                new Dictionary<string, string> {
                    { "IdUsuario", chatCountModel.UserId },
                    { "Count", chatCountModel.Count }
                }
            );

            var collectionChatCount = _mongoHelper.MongoDatabase.GetCollection<BsonDocument>("ChatCount");
            collectionChatCount.InsertOne(bsonDocumentRequestChatCount);
        }
    }
}
