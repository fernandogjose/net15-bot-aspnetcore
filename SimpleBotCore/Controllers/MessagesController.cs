using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using SimpleBotCore.Logic;
using SimpleBotCore.AppService.AppServices;
using SimpleBotCore.AppService.Dtos;

namespace SimpleBotCore.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        SimpleBotUser _bot = new SimpleBotUser();
        private readonly ChatAppService _chatAppService;

        public MessagesController(SimpleBotUser bot, ChatAppService chatAppService)
        {
            _bot = bot;
            _chatAppService = chatAppService;
        }

        [HttpGet]
        public string Get()
        {
            return "Hello World - Vendo a minha alteração";
        }

        [HttpGet("{message}")]
        public IActionResult Get(string message)
        {
            string response = _chatAppService.GetMessage(message);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Activity activity)
        {
            if (activity != null && activity.Type == ActivityTypes.Message)
            {
                await HandleActivityAsync(activity);
            }

            if (string.IsNullOrEmpty(activity.Text))
            {
                return BadRequest();
            }

            ChatDto chatDto = new ChatDto
            {
                UserId = activity.From.Id,
                Message = activity.Text
            };
            _chatAppService.AddMessage(chatDto);

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
    }
}
