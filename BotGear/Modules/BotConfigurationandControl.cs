using BotGear.Data.Models;
using BotGear.Managers;
using BotGear.Tools;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Modules
{
    [Export(typeof(ModuleBase))]
    public class BotConfigurationandControl : ModuleBase
    {
        ModuleConverter mv = new ModuleConverter();
        ServerManager srvmngr = new ServerManager();
        ServerConfigManager srvConfmngr = new ServerConfigManager();
   
        [Command("setrulechannel")]
        [Summary("Sets the rule Channel for the server")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetRulesChannel(string rulechannelname)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(rulechannelname) != true)
                {
                    String serverid = Convert.ToString(this.Context.Guild.Id);
                    if (await srvConfmngr.ServersConfigurationExists(serverid) == true
                        && await srvmngr.ServerExists(serverid) == true)
                    {
                        var oldconf = await srvConfmngr.GetServersConfigurationById(serverid);
                        oldconf.rules_channel_name = rulechannelname;
                        await srvConfmngr.EditServerConfiguration(serverid, oldconf);
                        await ReplyAsync("Rule Channel had been Set");
                    }
                    else if (await srvConfmngr.ServersConfigurationExists(serverid) == true 
                        && await srvmngr.ServerExists(serverid) != true)
                    {

                        var oldconf = await srvConfmngr.GetServersConfigurationById(serverid);
                        oldconf.rules_channel_name = rulechannelname;
                        await srvConfmngr.EditServerConfiguration(serverid, oldconf);

                        await srvmngr.addServer(this.Context.Guild);
                        await ReplyAsync("Rule Channel had been Set");
                    }
                    else if (await srvConfmngr.ServersConfigurationExists(serverid) != true && await srvmngr.ServerExists(serverid) == true)
                    {
                        BotGearServerConfiguration conf = new BotGearServerConfiguration();
                        conf.ServerId = serverid;
                        conf.rules_channel_name = rulechannelname;
                        await this.srvConfmngr.AddServerConfiguration(conf);
                        await ReplyAsync("Rule Channel had been Set");
                    }

                    else
                    {
                        await srvmngr.addServer(this.Context.Guild);
                        BotGearServerConfiguration conf = new BotGearServerConfiguration();
                        conf.ServerId = serverid;
                        conf.rules_channel_name = rulechannelname;
                        await this.srvConfmngr.AddServerConfiguration(conf);
                        await ReplyAsync("Rule Channel had been Set");
                    }
                }

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("setrules")]
        [Summary("Sets the rule Channel for the server")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task setRules(string rules)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(rules) != true)
                {
                    String serverid = Convert.ToString(this.Context.Guild.Id);
                    if (await srvConfmngr.ServersConfigurationExists(serverid) == true)
                    {
                        var conf = await srvConfmngr.GetServersConfigurationById(serverid);
                        conf.rules = rules;
                        conf.Notify_everyon_rulesChange = true;
                        await srvConfmngr.EditServerConfiguration(serverid, conf);
                        await ReplyAsync("Rules had been Set");
                        if (conf.Notify_everyon_rulesChange == true && String.IsNullOrWhiteSpace(conf.rules_channel_name) == false)
                        {
                            var channel = Context.Guild.GetChannelsAsync().Result.First(x => x.Name == conf.rules_channel_name);
                            if (channel != null)
                            {
                                await this.Context.Guild.GetTextChannelAsync(channel.Id).Result.SendMessageAsync(rules
                                    + " " + Context.Guild.Roles.First(x => x.Name == "everyone").Mention);
                            }

                        }

                    }
                    else
                    {
                        await ReplyAsync("use the command set rule channel to create the server config");

                    }


                }
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("rules")]
        [Summary("Shows the rules")]
        public async Task getRules()
        {
            try
            {

                String serverid = Convert.ToString(this.Context.Guild.Id);
                if (await srvConfmngr.ServersConfigurationExists(serverid) == true)
                {
                    var conf = await srvConfmngr.GetServersConfigurationById(serverid);

                    var channel =await  Context.User.GetOrCreateDMChannelAsync();
                        //Context.Guild.GetTextChannelsAsync().Result.First(x => x.Name == conf.rules_channel_name);
                    string trules = conf.rules;

                    if (channel != null)
                    {
                        var messages = await channel.GetMessagesAsync(100, CacheMode.AllowDownload, null).Flatten();

                        await channel.SendMessageAsync(String.Format("Rules for the {0} Server : \n {1} " ,Context.Guild.Name, trules));
                    }                
                    else
                    {
                        await ReplyAsync("Rules : \n " + trules);
                    }
                    

                }
                else
                {
                    await ReplyAsync("use the command set rule channel to create the server config");

                }


            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("updateruleschat")]
        [Summary("Posts the current rules to the channel with the rules")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task UpdateRulesChat()
        {
            try
            {

                String serverid = Convert.ToString(this.Context.Guild.Id);
                if (await srvConfmngr.ServersConfigurationExists(serverid) == true)
                {
                    var conf = await srvConfmngr.GetServersConfigurationById(serverid);

                    var channel = Context.Guild.GetTextChannelsAsync().Result.First(x => x.Name == conf.rules_channel_name);
                    string trules = conf.rules;

                    if (channel != null)
                    {
                        //var messages = await channel.GetMessagesAsync(100, CacheMode.AllowDownload, null).Flatten();

                        //while (messages != null)
                        //{
                        //    channel.DeleteMessagesAsync(messages);

                        //    messages = await channel.GetMessagesAsync(100, CacheMode.AllowDownload, null).Flatten();

                        //}
                        await channel.SendMessageAsync("Rules : \n " + trules);
                    }
                    else
                    {
                        await ReplyAsync("Rules : \n " + trules);
                    }


                }
                else
                {
                    await ReplyAsync("use the command set rule channel to create the server config");

                }


            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("getServerConf")]
        [Summary("Sets the rule Channel for the server")]
        public async Task getserverConfig()
        {
            try
            {

                String serverid = Convert.ToString(this.Context.Guild.Id);
                if (await srvConfmngr.ServersConfigurationExists(serverid) == true)
                {
                    var conf = await srvConfmngr.GetServersConfigurationById(serverid);
                    string mentions = conf.allow_channels_mentions;

                    string text = String.Format("Rules Channel :{0}\n Rules :\n {1}\n\n Notify on Rules Cahnge {2}\n" +
                        "Welcome and Goodbyes Channel :{4}\n Welcome Message:{5}\n"+ " Allowed channels for the bot to type by name : {3}\n by Mentions :"

                        , conf.rules_channel_name, conf.rules, conf.Notify_everyon_rulesChange,conf.allow_channels_name,conf.welcome_channel_name,conf.welcome_message)
                        +mentions;


                    await ReplyAsync("Server Configuration :: \n " + text);

                }
                else
                {
                    await ReplyAsync("use the command set rule channel to create the server config");

                }



            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("setnotifyOnruleson")]
        [Summary("Enable /Disable the notification on Rules change (type only true or false)")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task setNotificationOnRuleChangeOn()
        {
            try
            {

                String serverid = Convert.ToString(this.Context.Guild.Id);
                if (await srvConfmngr.ServersConfigurationExists(serverid) == true)
                {
                    var conf = await srvConfmngr.GetServersConfigurationById(serverid);
                    conf.Notify_everyon_rulesChange = true;

                    await srvConfmngr.EditServerConfiguration(serverid, conf);
                    await ReplyAsync("setting had been Set");

                }
                else
                {
                    await ReplyAsync("use the command set rule channel to create the server config");

                }



            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("setnotifyOnrulesoff")]
        [Summary("Enable /Disable the notification on Rules change (type only true or false)")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task setNotificationOnRuleChangeOff()
        {
            try
            {

                String serverid = Convert.ToString(this.Context.Guild.Id);
                if (await srvConfmngr.ServersConfigurationExists(serverid) == true)
                {
                    var conf = await srvConfmngr.GetServersConfigurationById(serverid);
                    conf.Notify_everyon_rulesChange = false;

                    await srvConfmngr.EditServerConfiguration(serverid, conf);
                    await ReplyAsync("setting had been Set");

                }
                else
                {
                    await ReplyAsync("use the command set rule channel to create the server config");

                }



            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("setallowed_channels")]
        [Summary("Sets the  Channels  for the bot to reply")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task setAllowedChannels(string channels)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(channels) != true)
                {
                    String serverid = Convert.ToString(this.Context.Guild.Id);
                    if (await srvConfmngr.ServersConfigurationExists(serverid) == true)
                    {
                        var conf = await srvConfmngr.GetServersConfigurationById(serverid);
                        conf.allow_channels_name = channels;

                        await srvConfmngr.EditServerConfiguration(serverid, conf);
                        await ReplyAsync("Channels had been Set");


                    }
                   
                        else if (await srvConfmngr.ServersConfigurationExists(serverid) == true
                        && await srvmngr.ServerExists(serverid) != true)
                        {

                            var oldconf = await srvConfmngr.GetServersConfigurationById(serverid);
                            oldconf.allow_channels_name = channels;
                            await srvConfmngr.EditServerConfiguration(serverid, oldconf);

                            await srvmngr.addServer(this.Context.Guild);
                            await ReplyAsync("Allowed  Channels had been Set");
                        }
                        else if (await srvConfmngr.ServersConfigurationExists(serverid) != true && await srvmngr.ServerExists(serverid) == true)
                        {
                            BotGearServerConfiguration conf = new BotGearServerConfiguration();
                            conf.ServerId = serverid;
                            conf.allow_channels_name = channels;
                            await this.srvConfmngr.AddServerConfiguration(conf);
                            await ReplyAsync("Allowed  Channels  had been Set");
                        }

                        else
                        {
                            await srvmngr.addServer(this.Context.Guild);
                            BotGearServerConfiguration conf = new BotGearServerConfiguration();
                            conf.ServerId = serverid;
                            conf.allow_channels_name = channels;
                            await this.srvConfmngr.AddServerConfiguration(conf);
                            await ReplyAsync("Allowed  ChannelsChannel had been Set");
                        }

                    
                }
            
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("setallowed_channelsbymentions")]
        [Summary("Sets the  Channels  for the bot to reply")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task setAllowedChannelsByMentions(string channels)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(channels) != true)
                {
                    String serverid = Convert.ToString(this.Context.Guild.Id);
                    if (await srvConfmngr.ServersConfigurationExists(serverid) == true)
                    {
                        var conf = await srvConfmngr.GetServersConfigurationById(serverid);
                        conf.allow_channels_mentions = channels;

                        await srvConfmngr.EditServerConfiguration(serverid, conf);
                        await ReplyAsync("Channels had been Set");


                    }

                    else if (await srvConfmngr.ServersConfigurationExists(serverid) == true
                    && await srvmngr.ServerExists(serverid) != true)
                    {

                        var oldconf = await srvConfmngr.GetServersConfigurationById(serverid);
                        oldconf.allow_channels_mentions = channels;
                        await srvConfmngr.EditServerConfiguration(serverid, oldconf);

                        await srvmngr.addServer(this.Context.Guild);
                        await ReplyAsync("Allowed  Channels had been Set");
                    }
                    else if (await srvConfmngr.ServersConfigurationExists(serverid) != true && await srvmngr.ServerExists(serverid) == true)
                    {
                        BotGearServerConfiguration conf = new BotGearServerConfiguration();
                        conf.ServerId = serverid;
                        conf.allow_channels_mentions = channels;
                        await this.srvConfmngr.AddServerConfiguration(conf);
                        await ReplyAsync("Allowed  Channels  had been Set");
                    }

                    else
                    {
                        await srvmngr.addServer(this.Context.Guild);
                        BotGearServerConfiguration conf = new BotGearServerConfiguration();
                        conf.ServerId = serverid;
                        conf.allow_channels_mentions = channels;
                        await this.srvConfmngr.AddServerConfiguration(conf);
                        await ReplyAsync("Allowed  ChannelsChannel had been Set");
                    }


                }

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("allowedcahnenels")]
        [Summary("Sets  the  Channels  for the bot to reply")]
        public async Task getAllowedChannels()
        {
            try
            {

                String serverid = Convert.ToString(this.Context.Guild.Id);
                if (await srvConfmngr.ServersConfigurationExists(serverid) == true)
                {
                    var conf = await srvConfmngr.GetServersConfigurationById(serverid);


                    string trules = conf.allow_channels_name;
                    string mentions = conf.allow_channels_mentions;


                    await ReplyAsync(String.Format("Allowed Channels By Name: \n {0} \n By Mentions :" , trules)+mentions);

                }
                


            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("welcome_and_goodbye_channel")]
        [Summary("Gets  the  Channels  for the bot to reply")]
        public async Task getWelcome_and_GoodByes_Channel()
        {
            try
            {

                String serverid = Convert.ToString(this.Context.Guild.Id);
                if (await srvConfmngr.ServersConfigurationExists(serverid) == true)
                {
                    var conf = await srvConfmngr.GetServersConfigurationById(serverid);


                    string trules = conf.welcome_channel_name;


                    await ReplyAsync("Welcome and Goodbye Channel : \n " + trules);

                }



            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }

        [Command("setwelcome_and_goodbye_channel")]
        [Summary("Sets the  Channel for welcome and goodbye")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task setWelcome_and_GoodByes_Channel(string channels)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(channels) != true)
                {
                    String serverid = Convert.ToString(this.Context.Guild.Id);
                    if (await srvConfmngr.ServersConfigurationExists(serverid) == true)
                    {
                        var conf = await srvConfmngr.GetServersConfigurationById(serverid);
                        conf.welcome_channel_name = channels;

                        await srvConfmngr.EditServerConfiguration(serverid, conf);
                        await ReplyAsync("Settings had been Set");


                    }

                    else if (await srvConfmngr.ServersConfigurationExists(serverid) == true
                    && await srvmngr.ServerExists(serverid) != true)
                    {

                        var oldconf = await srvConfmngr.GetServersConfigurationById(serverid);
                        oldconf.welcome_channel_name = channels;
                        await srvConfmngr.EditServerConfiguration(serverid, oldconf);

                        await srvmngr.addServer(this.Context.Guild);
                        await ReplyAsync("Settings had been Set");
                    }
                    else if (await srvConfmngr.ServersConfigurationExists(serverid) != true && await srvmngr.ServerExists(serverid) == true)
                    {
                        BotGearServerConfiguration conf = new BotGearServerConfiguration();
                        conf.ServerId = serverid;
                        conf.welcome_channel_name = channels;
                        await this.srvConfmngr.AddServerConfiguration(conf);
                        await ReplyAsync("Settings had been Set");
                    }

                    else
                    {
                        await srvmngr.addServer(this.Context.Guild);
                        BotGearServerConfiguration conf = new BotGearServerConfiguration();
                        conf.ServerId = serverid;
                        conf.welcome_channel_name = channels;
                        await this.srvConfmngr.AddServerConfiguration(conf);
                        await ReplyAsync("Settings had been Set");
                    }


                }

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }

        [Command("setwelcome_message")]
        [Summary("Sets  Template of  the welcome message")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task setWelcome_Message(string Message)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(Message) != true)
                {
                    String serverid = Convert.ToString(this.Context.Guild.Id);
                    if (await srvConfmngr.ServersConfigurationExists(serverid) == true)
                    {
                        var conf = await srvConfmngr.GetServersConfigurationById(serverid);
                        conf.welcome_message = Message;

                        await srvConfmngr.EditServerConfiguration(serverid, conf);
                        await ReplyAsync("Settings had been Set");


                    }

                    else if (await srvConfmngr.ServersConfigurationExists(serverid) == true
                    && await srvmngr.ServerExists(serverid) != true)
                    {

                        var oldconf = await srvConfmngr.GetServersConfigurationById(serverid);
                        oldconf.welcome_message = Message;
                        await srvConfmngr.EditServerConfiguration(serverid, oldconf);

                        await srvmngr.addServer(this.Context.Guild);
                        await ReplyAsync("Settings had been Set");
                    }
                    else if (await srvConfmngr.ServersConfigurationExists(serverid) != true && await srvmngr.ServerExists(serverid) == true)
                    {
                        BotGearServerConfiguration conf = new BotGearServerConfiguration();
                        conf.ServerId = serverid;
                        conf.welcome_message = Message;
                        await this.srvConfmngr.AddServerConfiguration(conf);
                        await ReplyAsync("Settings had been Set");
                    }

                    else
                    {
                        await srvmngr.addServer(this.Context.Guild);
                        BotGearServerConfiguration conf = new BotGearServerConfiguration();
                        conf.ServerId = serverid;
                        conf.welcome_message = Message;
                        await this.srvConfmngr.AddServerConfiguration(conf);
                        await ReplyAsync("Settings had been Set");
                    }


                }

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }

        [Command("getwelcome_message")]
        [Summary("Gets  the  Template the wlecome message of the bot will have.")]
        public async Task getWelcome_Message()
        {
            try
            {

                String serverid = Convert.ToString(this.Context.Guild.Id);
                if (await srvConfmngr.ServersConfigurationExists(serverid) == true)
                {
                    var conf = await srvConfmngr.GetServersConfigurationById(serverid);


                    string trules = conf.welcome_message;


                    await ReplyAsync("Welcome Message : \n " + trules);

                }



            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
       
        [Command("restartbot")]
        [RequireOwner]
        public async Task restartmethodxd()
        {
            try
            {


                await Task.Delay(10000);
                System.Diagnostics.Process.Start("launch.cmd");
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }

        }
        [Command("killbot")]
        [RequireOwner]
        public async Task killmethodxd()
        {
            try
            {


               
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }

        }
       
    }
}
