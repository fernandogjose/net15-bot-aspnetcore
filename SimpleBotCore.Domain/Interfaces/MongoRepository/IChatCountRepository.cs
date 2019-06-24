using MongoDB.Bson;
using SimpleBotCore.Domain.Models.Mongo;

namespace SimpleBotCore.Domain.Interfaces.MongoRepository
{
    public interface IChatCountRepository
    {
        BsonDocument GetByUser(string userId);

        void DeleteByUser(string userId);

        void Add(ChatCountModel chatCountModel);
    }
}
