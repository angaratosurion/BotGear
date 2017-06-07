using BotGear.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using BotGear.Attributes;
using Discord.WebSocket;
using BotGear.Tools;
using BotGear.BotConfiguration;
using Discord.Net.Providers.WS4Net;
using System.ComponentModel.Composition;

namespace HelloBotCore
{
    [Export(typeof(IBot))]
    [BotGearRunnerClass()]
    public class HelloBot : IBot
    {
       const string token = "Your discordbot token here";
        
        private static DiscordSocketClient client;
        static HelloBotCommandHandler handler;
        BotConfigurationCore confcore;

        public Task Configure()
        {
            DiscordSocketConfig config = new DiscordSocketConfig();
            if (CommonTools.IsWindows10() != true)//check if you run it on windows 10 
            {
                config.WebSocketProvider = WS4NetProvider.Instance;
                client = new DiscordSocketClient(config);
            }
            else
            {
                client = new DiscordSocketClient();
            }
            confcore = new BotConfigurationCore();
            handler = new HelloBotCommandHandler(null, client, null);
            client = confcore.ConfigureBot(client, handler).Result;

            return Task.CompletedTask;

        }

        public Task Log(LogMessage arg)
        {
            return Task.CompletedTask;
        }

        public async Task Start()
        {
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            await Task.Delay(-1);
        }

        public Task Stop()
        {
            throw new NotImplementedException();
        }
    }
}
