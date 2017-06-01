using BotGear.Data.Models;
using BotGear.Managers;
using BotGear.Tools;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Modules
{
    [Export(typeof(ModuleBase))]
    public class BotGearCommands : ModuleBase
    {
        ModuleConverter mv = new ModuleConverter(); 
        ServerManager srvmngr = new ServerManager();
        ServerConfigManager srvConfmngr = new ServerConfigManager();
        [Command("setrulechannel")]
        [Summary("Sets the rule Chnanel for the server")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetRulesChannel(string rulechannelname)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(rulechannelname) != true)
                {
                    String serverid = Convert.ToString(this.Context.Guild.Id);
                    if (await srvConfmngr.ServersConfigurationExists(serverid) == true && await srvmngr.ServerExists(serverid) == true)
                    {
                        var oldconf = await srvConfmngr.GetServersConfigurationById(serverid);
                        oldconf.rules_channel_name = rulechannelname;
                        await srvConfmngr.EditServerConfiguration(serverid, oldconf);
                        await ReplyAsync("Rule Channel had been Set");
                    }
                    else if (await srvConfmngr.ServersConfigurationExists(serverid) == true && await srvmngr.ServerExists(serverid) != true)
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
        [Summary("Sets the rule Chnanel for the server")]
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
                        await srvConfmngr.EditServerConfiguration(serverid, conf);
                        await ReplyAsync("Rules had been Set");

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
        [Summary("Sets the rule Chnanel for the server")]
        public async Task getRules( )
        {
            try
            {
                
                    String serverid = Convert.ToString(this.Context.Guild.Id);
                    if (await srvConfmngr.ServersConfigurationExists(serverid) == true)
                    {
                        var conf = await srvConfmngr.GetServersConfigurationById(serverid);


                        string trules = conf.rules;


                        await ReplyAsync("Rules : \n "+trules);

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
        [Summary("Sets the rule Chnanel for the server")]
        public async Task getserverConfig( )
        {
            try
            {
                
                    String serverid = Convert.ToString(this.Context.Guild.Id);
                    if (await srvConfmngr.ServersConfigurationExists(serverid) == true)
                    {
                        var conf = await srvConfmngr.GetServersConfigurationById(serverid);


                    string text = String.Format("Rules Channel :{0}\n Rules :\n {1}",conf.rules_channel_name,conf.rules);


                        await ReplyAsync("Server Configuration :: \n " +text);

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
    }
}
