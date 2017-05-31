using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Discord.WebSocket;
using Discord.Commands;
using BotGear.Modules;
using System.IO;

namespace BotGear.BotConfiguration
{
   public  class BotConfigurationCore
    {
        private DiscordSocketClient _client;
        private IConfiguration _config;



        public async  Task ConfigureBot()
        {
            var services = ConfigureServices();
           // services.GetRequiredService<LogService>();
            await services.GetRequiredService<CommandHandler>().Install(services);
        }
        public IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                // Base
                .AddSingleton(_client)
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                // Logging
               // .AddLogging()
              //  .AddSingleton<LogService>()
                // Extra
                .AddSingleton(_config)
                // Add additional services here...
                .BuildServiceProvider();
        }

        public IConfiguration BuildConfig()
        {
            return new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
#if DEBUG
               .AddJsonFile("config_debug.json")
#else
                .AddJsonFile("config.json")
#endif
                .Build();
        }
    }
}
