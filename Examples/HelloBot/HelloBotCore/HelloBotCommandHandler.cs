using BotGear.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace HelloBotCore
{
    class HelloBotCommandHandler : CommandHandler
    {
        public HelloBotCommandHandler(IServiceProvider provider, Discord.WebSocket.DiscordSocketClient discord, CommandService tcommands) : base(provider, discord, tcommands)
        {
        }

        public override Task UserJoined(SocketGuildUser user)
        {
            try
            {
                base.UserJoined(user);
                var channel = user.Guild.DefaultChannel;
                if (channel != null && user.IsBot == false)
                {
                    //channel.SendMessageAsync(String.Format("{0} Type !help for help ", user.Mention));
                }
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
              
                return Task.CompletedTask;
            }
        }
        public override async Task GuildMemberUpdated(SocketGuildUser arg1, SocketGuildUser arg2)
        {
            try
            {
                await base.GuildMemberUpdated(arg1, arg2);


                if (arg1.Status != arg2.Status && arg1.IsBot == false && arg2.IsBot == false)
                {
                    if (arg1.Status != Discord.UserStatus.Online && arg2.Status == Discord.UserStatus.Online)
                    {
                        var channel = arg2.Guild.DefaultChannel;
                        if (channel != null)
                        {
                            await channel.SendMessageAsync(String.Format(" {0}  Type !help for help ", arg2.Mention));
                        }
                    }
                       
                         
                    
                }




                // return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                 
                //return Task.CompletedTask;
            }

        }
    }
}
