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
using BotGear.Tools;

namespace BotGear.BotConfiguration
{
    public class BotConfigurationCore
    {
        private DiscordSocketClient _client;
        private IConfiguration _config;



        public async Task<DiscordSocketClient> ConfigureBot(DiscordSocketClient tclient, CommandHandler commhndl)
        {
            try
            {
                _config = BuildConfig();
                if (commhndl != null && tclient!=null)
                {
                    _client = tclient;
                    var services = ConfigureServices(commhndl);
                    // services.GetRequiredService<LogService>();
                    await services.GetRequiredService<CommandHandler>().Install(services);
                }
                return _client;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }

        }

        public async Task<DiscordSocketClient> ConfigureBotOnBotGearRunner(DiscordSocketClient tclient, CommandHandler commhndl,string botname)
        {
            try
            {
                _config = BuildConfig();
                if (commhndl != null && tclient != null && botname!=null)
                {
                    _client = tclient;
                    var services = ConfigureServices(commhndl);
                    // services.GetRequiredService<LogService>();
                    await services.GetRequiredService<CommandHandler>().Install(services,botname);
                }
                return _client;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }

        }
        public IServiceProvider ConfigureServices(CommandHandler commhndl)
        {
            try
            {
                if (commhndl != null)
                {

                    return new ServiceCollection()
                        // Base
                        .AddSingleton(_client)
                        .AddSingleton<CommandService>()
                        .AddSingleton<CommandHandler>(commhndl)
                        // Logging
                        // .AddLogging()
                        //  .AddSingleton<LogService>()
                        // Extra
                        .AddSingleton(_config)
                        // Add additional services here...
                        .BuildServiceProvider();
                }
                else
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

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;
            }
        }

        public IConfiguration BuildConfig()
        {
            try
            {
                string dir = Directory.GetCurrentDirectory();
                return new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
#if DEBUG
               .AddJsonFile("config_debug.json")
#else
                .AddJsonFile("config.json")
#endif
                .Build();
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
    }
}
