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
        [Command("setrulechannel")]
        [Summary("Sets the rule Chnanel for the server")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetRulesChannel(string rulechannelname)
        {
            try
            {
                if ( String.IsNullOrWhiteSpace(rulechannelname)!=null)
                {
                    await ReplyAsync("Hello it's not implemented yet");
                }

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
    }
}
