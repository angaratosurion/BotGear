using Discord.Commands;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace DiscordGamesPlugin
{
    [Export(typeof(ModuleBase))]
    public class EightBallGame : ModuleBase
    {
       
        [Command("8ball")]
        [Summary("Gives a prediction")]
        public async Task EightBall([Remainder] string input)
        {
            string[] predictionsTexts = new string[]
              {
                "It is very unlikely.",
                "I don't think so...",
                "Yes !",
                "I don't know",
                "No.","Hi ", "Hey ", "Hello ", "Greetings ", "Hey there, ", "G'day",
              };
            Random rand = new Random();
            int randomIndex = rand.Next(predictionsTexts.Length);
            string text = predictionsTexts[randomIndex];
            await ReplyAsync(Context.User.Mention + ", " + text);
        }
     
      
    }
}
