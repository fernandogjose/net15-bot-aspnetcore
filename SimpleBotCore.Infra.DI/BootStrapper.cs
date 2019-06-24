using Microsoft.Extensions.DependencyInjection;
using SimpleBotCore.AppService.AppServices;
using SimpleBotCore.Domain.Interfaces.DomainServices;
using SimpleBotCore.Domain.Interfaces.MongoRepository;
using SimpleBotCore.Domain.Services;
using SimpleBotCore.Infra.Mongo.Helpers;
using SimpleBotCore.Infra.Mongo.Repositories;

namespace SimpleBotCore.Infra.DI
{
    public class BootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // AppServices
            services.AddScoped<ChatAppService>();

            // Domain Services
            services.AddScoped<IChatService, ChatService>();

            // Mongo Repository
            services.AddSingleton<MongoHelper>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IChatCountRepository, ChatCountRepository>();

            // Sql Server Repository
            services.AddScoped<Domain.Interfaces.SqlServerRepository.IChatRepository, SqlServer.Repositories.ChatRepository>();
        }
    }
}
