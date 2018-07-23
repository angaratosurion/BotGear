using Discord.Commands;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace replyplug
{

    [Export(typeof(ModuleBase))]
    public class CommandsInitilizer2 : ModuleBase
    {
        [Command("echo")]
        public async Task Echo(string text)
        {
            await ReplyAsync(string.Format("You Said :  {0}", text));
        }
        [Command("test")]
        public async Task test ()
        {
            await ReplyAsync("this is  a test");
        }

      
        
    }
}
