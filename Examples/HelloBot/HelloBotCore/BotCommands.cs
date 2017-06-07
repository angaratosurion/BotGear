using Discord.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloBotCore
{
    [Export(typeof(ModuleBase))]
    public class BotCommands : ModuleBase
    {
        [Command("Hello")]
        [Summary("Just a Hello World Command")]
        public async Task Hello()
        {

            await ReplyAsync("Hello World!");
        }
    }
}
