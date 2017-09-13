using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Modules.CommandBuilder
{
    public class HelpCommand
    {
        private CommandService _service;

        public HelpCommand (CommandService service)           /* Create a constructor for the commandservice dependency */
        {
            _service = service;
        }
        public async Task<EmbedBuilder> Help(ICommandContext Context)
        {
            try
            {
                
                string prefix = "!";  /* put your chosen prefix here */
                var builder = new EmbedBuilder()
                {
                    Color = new Color(114, 137, 218),
                    Description = "These are the commands you can use"
                };
                if (Context != null)
                {

                    foreach (var module in _service.Modules) /* we are now going to loop through the modules taken from the service we initiated earlier ! */
                    {
                        string description = null;
                        foreach (var cmd in module.Commands) /* and now we loop through all the commands per module aswell, oh my! */
                        {
                            var result = await cmd.CheckPreconditionsAsync(Context); /* gotta check if they pass */
                            if (result.IsSuccess)
                                description += $"{prefix}{cmd.Aliases.First()}\n"; /* if they DO pass, we ADD that command's first alias (aka it's actual name) to the description tag of this embed */
                        }

                        if (!string.IsNullOrWhiteSpace(description)) /* if the module wasn't empty, we go and add a field where we drop all the data into! */
                        {
                            builder.AddField(x =>
                            {
                                x.Name = module.Name;
                                x.Value = description;
                                x.IsInline = false;
                            });
                        }
                    }
                }
                return builder;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
