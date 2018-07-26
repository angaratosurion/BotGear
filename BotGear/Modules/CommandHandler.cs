using BotGear.Data.Models;
using BotGear.Managers;
using BotGear.Tools;
using Discord;
using Discord.Commands;
using Discord.Net;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Modules
{
    public abstract class CommandHandler
    {
        private CommandService commands;
        private DiscordSocketClient client;
        private IServiceProvider _provider;
        ServerConfigManager confmngr = new ServerConfigManager();
        ModuleConverter mdconv = new ModuleConverter();
        /// private IDependencyMap map;
       ServerManager srvmngr = new ServerManager();
         

        public CommandHandler(IServiceProvider provider, DiscordSocketClient discord, CommandService tcommands)
        {
            client= discord;
            commands = tcommands;

           // client.MessageReceived += MessageReceived;
        }
        public async Task Install(IServiceProvider provider)
        {
            //Create Command Service, Inject it into Dependency Map
            //client = _map.Get<DiscordSocketClient>();
            _provider = provider;
            
            client.MessageReceived += HandleCommand;
           client.UserJoined += UserJoined;
            client.UserLeft += UseLeft;
            client.UserUpdated += UserUpdated;
            client.GuildMemberUpdated += GuildMemberUpdated;

            if (commands == null)
            {
                commands = new CommandService();
            }
            
            //_map.Add(commands);
          //  map = _map;

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
       

        public virtual Task GuildMemberUpdated(SocketGuildUser arg1, SocketGuildUser arg2)
        {
            try
            {

                //if (arg1 != null && arg2 !=null)
                //{
                //    if (arg1.Status != arg2.Status && arg1.IsBot==false && arg2.IsBot==false)
                //    {
                //        UserManager usermngr = new UserManager();
                //        if (arg1.Status != Discord.UserStatus.Online && arg2.Status == Discord.UserStatus.Online)
                //        {
                //            var channel = arg2.Guild.DefaultChannel;
                //            if (channel != null)
                //            {
                //                channel.SendMessageAsync(String.Format("Welcome {0}, type !rules to read the rules ", arg2.Mention));
                //            }
                //        }
                //        else if (arg2.Status == Discord.UserStatus.Offline)
                //        {
                //            var channel = arg2.Guild.DefaultChannel;
                //            if (channel != null)
                //            {
                //                channel.SendMessageAsync(String.Format("Bye {0} ", arg2.Mention));
                //            }
                //        }
                //    }
                //}

                 
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return Task.CompletedTask;
            }
        }

        public virtual Task UserUpdated(SocketUser arg1, SocketUser arg2)
        {
            try
            {
                
                if (arg1 != null && arg1.IsBot == false && arg2.IsBot == false)
                {
                    UserManager usermngr = new UserManager();
                    usermngr.EditUser(arg1, arg2);
                }
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return Task.CompletedTask;
            }
        }

        public virtual Task UseLeft(SocketGuildUser user)
        {
            try
            {
                
                var conf =   confmngr.GetServersConfigurationById(this.mdconv.IGuildToBotGearServer(user.Guild).Id).Result;
                if (conf != null && conf.welcome_channel_name!=null )
                {
                    var channel = user.Guild.TextChannels.First(x => x.Name == conf.welcome_channel_name);
                    if (channel != null && user.IsBot == false)
                    {
                        channel.SendMessageAsync(String.Format("Bye {0} ", user.Mention));
                    }
                }
                else
                {

                    /*var channel = user.Guild.DefaultChannel;

                    if (channel != null && user.IsBot == false)
                    {
                        channel.SendMessageAsync(String.Format("Bye {0} ", user.Mention));
                    }*/
                }
                
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return Task.CompletedTask;
            }
        }

        public virtual Task UserJoined(SocketGuildUser  user)
        {
            try
            {
                var conf = confmngr.GetServersConfigurationById(this.mdconv.IGuildToBotGearServer(user.Guild).Id).Result;
                WelcomeMessageBulder welcomeMeesageBulder = new WelcomeMessageBulder();
                if (conf != null && conf.welcome_channel_name != null)
                {
                    var channel = user.Guild.TextChannels.First(x => x.Name == conf.welcome_channel_name);
                    if (channel != null && user.IsBot == false)
                    {
                        var rulechannel = user.Guild.TextChannels.First(x => x.Name == conf.rules_channel_name);
                        var welcome_message = welcomeMeesageBulder.CreateMessage(conf.welcome_message, user, rulechannel).Result;
                        channel.SendMessageAsync(welcome_message);
                    }
                }
                else
                {

                    /*var channel = user.Guild.DefaultChannel;
                    if (channel != null && user.IsBot == false)
                    {
                        channel.SendMessageAsync(String.Format("Welcome {0} type !rules to read the rules ", user.Mention));
                    }*/
                }
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return Task.CompletedTask;
            }
        }

        public CommandService Commands
        {
            get { return commands; }
        }

        public async Task HandleCommand(SocketMessage parameterMessage)
        {
            try
            { 
            //Don't handle the command if it is a system message
            var message = parameterMessage as SocketUserMessage;
            if (message == null) return;
                
                    //Mark where the prefix ends and the command begins
                    int argPos = 0;
                    //Determine if the message has a valid prefix, adjust argPos
                    //if (!(message.HasMentionPrefix(client.CurrentUser, ref argPos) || message.HasCharPrefix('!', ref argPos))) return;
                    if (!(message.HasStringPrefix("!?", ref argPos))) return;
                if (message.HasStringPrefix("!?", ref argPos) && message.Content == "!?") return;
                   






                //Create a Command Context
                    var context = new CommandContext(client, message);
                
                var channeltype = context.Channel.GetType();
                 if (channeltype == typeof(SocketDMChannel) && context.Guild == null)
                {
                    
                   await this.ExecuteCommand(message, context, argPos);
                   return ;
                }

                //Execute the command, store the result
                var conf = await confmngr.GetServersConfigurationById(this.mdconv.IGuildToBotGearServer(context.Guild).Id);
                if (conf != null && conf.allow_channels_name != null && channeltype!=typeof(SocketDMChannel)
                    &&context.Guild!=null)
                {
                    string[] allowedcahnels = conf.allow_channels_name.Split(',');
                    if (allowedcahnels != null && allowedcahnels.Contains(context.Channel.Name))
                    {
                        await this.ExecuteCommand(message, context, argPos);
                        return;
                    }
                }

                else if(message.Content.Contains("setallowed_channels")==true )
                {
                    //If the command failed, notify the user
                   await this.ExecuteCommand(message, context, argPos);
                    return;
                }
              
                else
                {
                    /*var dmchannel = await context.Guild.GetUserAsync(context.Guild.OwnerId).Result.GetOrCreateDMChannelAsync();
                    if(dmchannel!=null )
                    {
                        dmchannel.SendMessageAsync("Configure the list of channels the bot is allowed to type Seperated by ','\n" +
                            "with the setallowedcahnenels command ");
                        
                    }*/

                    /*var channel =context.Guild.GetDefaultChannelAsync().Result.Name;
                     String serverid = Convert.ToString(context.Guild.Id);
                     if (await this.confmngr.ServersConfigurationExists(serverid) != true 
                         && await srvmngr.ServerExists(serverid) == true)
                     {
                         BotGearServerConfiguration tconf = new BotGearServerConfiguration();
                         tconf.ServerId = serverid;
                         tconf.allow_channels_name = channel;
                         await this.confmngr.AddServerConfiguration(tconf);
                         await context.Guild.GetDefaultChannelAsync().Result.SendMessageAsync("Allowed  Channels  had been Set");
                     }

                     else
                     {
                         await srvmngr.addServer(context.Guild);
                         BotGearServerConfiguration tconf = new BotGearServerConfiguration();
                         tconf.ServerId = serverid;
                         tconf.allow_channels_name = channel;
                         await this.confmngr.AddServerConfiguration(tconf);
                         await context.Guild.GetDefaultChannelAsync().Result.SendMessageAsync("Allowed  Channels  had been Set");
                     }*/

                   await this.ExecuteCommand(message, context, argPos);
                    return;
                }
                
            }
            catch(HttpException)
            {

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                var message = parameterMessage as SocketUserMessage;
                if ( message ==null)
                {
                    return ;
                }
                var context = new CommandContext(client, message);
                if (context == null )
                {
                    return;
                }
                 var appinf=await context.Client.GetApplicationInfoAsync();
                if ( appinf == null)
                {
                    return;
                }
               var dmchannel =await appinf.Owner.GetOrCreateDMChannelAsync();
                if ( dmchannel !=null)
                {
                   await  dmchannel.SendMessageAsync(String.Format("an Exception was thrown at :{0} {1}",DateTime.Now.ToLongDateString() ,
                       DateTime.Now.ToLongTimeString()));
                }
                


            }
        }

        private async Task ExecuteCommand(SocketMessage parameterMessage, CommandContext context, int argPos)
        {
            try
            {
                var message = parameterMessage as SocketUserMessage;
                if (message == null || context ==null) return;
                var channel = context.Channel;
              await  channel.TriggerTypingAsync(null);
                var result = await commands.ExecuteAsync(context, argPos, _provider);

                if (!result.IsSuccess)
                {

                    // var emote = context.Guild.Emotes.First(x => x.Name == "x");
                    byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes("\\:x:");
                    var emote = new Emoji("❌");

                    if (emote != null)
                    {
                        await message.AddReactionAsync(emote, null);
                    }

                    //await message.Channel.SendMessageAsync($"**Error:** {result.ErrorReason}");
                }

            }
            catch (HttpException)
            {

            }
            catch (Exception ex)
            { 
                CommonTools.ErrorReporting(ex);
                var message = parameterMessage as SocketUserMessage;
                if (message == null)
                {
                    return;
                }
               // var context = new CommandContext(client, message);
                if (context == null)
                {
                    return;
                }
                var appinf = await context.Client.GetApplicationInfoAsync();
                if (appinf == null)
                {
                    return;
                }
                var dmchannel = await appinf.Owner.GetOrCreateDMChannelAsync();
                if (dmchannel != null)
                {
                    await dmchannel.SendMessageAsync(String.Format("an Exception was thrown at :{0} {1}", DateTime.Now.ToLongDateString(),
                        DateTime.Now.ToLongTimeString()));
                }



            }
        }
    }
}
