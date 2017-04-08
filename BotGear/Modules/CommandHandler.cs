using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Modules
{
    public class CommandHandler
    {
        private CommandService commands;
        private DiscordSocketClient client;
        private IDependencyMap map;

        public async Task Install(IDependencyMap _map)
        {
            //Create Command Service, Inject it into Dependency Map
            client = _map.Get<DiscordSocketClient>();
            client.MessageReceived += HandleCommand;
            commands = new CommandService();
            //_map.Add(commands);
            map = _map;

            //await commands.AddModulesAsync(Assembly.GetExecutingAssembly());
            var plugins = BotGearCore.GetAssemblies();
            if (plugins != null)
            {
                foreach (var a in plugins)
                {
                    await commands.AddModulesAsync(a);

                }
            }


            //Send user message to get handled
            // client.MessageReceived += HandleCommand;
        }
        public CommandService Commands
        {
            get { return commands; }
        }

        public async Task HandleCommand(SocketMessage parameterMessage)
        {
            //Don't handle the command if it is a system message
            var message = parameterMessage as SocketUserMessage;
            if (message == null) return;

            //Mark where the prefix ends and the command begins
            int argPos = 0;
            //Determine if the message has a valid prefix, adjust argPos
            //if (!(message.HasMentionPrefix(client.CurrentUser, ref argPos) || message.HasCharPrefix('!', ref argPos))) return;
            if (!(message.HasCharPrefix('!', ref argPos))) return;

            //Create a Command Context
            var context = new CommandContext(client, message);
            //Execute the command, store the result
            var result = await commands.ExecuteAsync(context, argPos, map);

            //If the command failed, notify the user
            if (!result.IsSuccess)
                await message.Channel.SendMessageAsync($"**Error:** {result.ErrorReason}");
        }

    }
}
