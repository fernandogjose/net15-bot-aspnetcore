using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using SimpleBotCore.Logic;
using MongoDB.Driver;
using MongoDB.Bson;

namespace SimpleBotCore.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private readonly MongoClient _client;

        private readonly IMongoDatabase _db;

        SimpleBotUser _bot = new SimpleBotUser();

        public MessagesController(SimpleBotUser bot)
        {
            this._bot = bot;

            // Abre a conexão e grava no banco
            _client = new MongoClient("mongodb://127.0.0.1:27017");
            _db = _client.GetDatabase("Bot");
        }

        [HttpGet]
        public string Get()
        {
            return "Hello World - Vendo a minha alteração";
        }

        [HttpGet("{message}")]
        public string Get(string message)
        {
            var collection = _db.GetCollection<BsonDocument>("Chat");
            FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter = builder.Eq("Mensagem", message);
            BsonDocument bsonDocumentResponse = collection.Find(filter).FirstOrDefault();

            return bsonDocumentResponse == null ? "Registro não encontrado" : bsonDocumentResponse.GetValue("Mensagem").ToString();
        }

        // POST api/messages
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Activity activity)
        {
            if (activity != null && activity.Type == ActivityTypes.Message)
            {
                await HandleActivityAsync(activity);
            }

            if (string.IsNullOrEmpty(activity.Text))
            {
                return Accepted();
            }

            // Grava a mensagem
            var collectionChat = _db.GetCollection<BsonDocument>("Chat");
            var bsonDocumentRequest = new BsonDocument(
                new Dictionary<string, string> {
                    { "IdUsuario", activity.From.Id },
                    { "Mensagem", activity.Text }
                }
            );

            collectionChat.InsertOne(bsonDocumentRequest);

            // Grava a quantidade de mensagens por usuário
            var countByUser = GetByUser(activity.From.Id);
            var collectionChatCount = _db.GetCollection<BsonDocument>("ChatCount");
            var count = 1;
            if (countByUser != null)
            {
                count = Convert.ToInt32(countByUser.GetValue("Count")) + 1;
                DeleteByUser(activity.From.Id);
            }           

            var bsonDocumentRequestChatCount = new BsonDocument(
                new Dictionary<string, string> {
                    { "IdUsuario", activity.From.Id },
                    { "Count", count.ToString() }
                }
            );
            collectionChatCount.InsertOne(bsonDocumentRequestChatCount);

            // HTTP 202
            return Accepted();
        }

        // Estabelece comunicacao entre o usuario e o SimpleBotUser
        async Task HandleActivityAsync(Activity activity)
        {
            string text = activity.Text;
            string userFromId = activity.From.Id;
            string userFromName = activity.From.Name;

            var message = new SimpleMessage(userFromId, userFromName, text);

            string response = _bot.Reply(message);

            await ReplyUserAsync(activity, response);
        }

        // Responde mensagens usando o Bot Framework Connector
        async Task ReplyUserAsync(Activity message, string text)
        {
            var connector = new ConnectorClient(new Uri(message.ServiceUrl));
            var reply = message.CreateReply(text);

            await connector.Conversations.ReplyToActivityAsync(reply);
        }

        public BsonDocument GetByUser(string userId)
        {
            var collection = _db.GetCollection<BsonDocument>("ChatCount");
            FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter = builder.Eq("IdUsuario", userId);
            BsonDocument bsonDocumentResponse = collection.Find(filter).FirstOrDefault();

            return bsonDocumentResponse;
        }

        public void DeleteByUser(string userId)
        {
            var collection = _db.GetCollection<BsonDocument>("ChatCount");
            FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
            FilterDefinition<BsonDocument> filter = builder.Eq("IdUsuario", userId);
            collection.DeleteOne(filter);
        }
    }
}
