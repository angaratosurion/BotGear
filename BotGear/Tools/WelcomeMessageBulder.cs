using BotGear.Tools;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Tools
{
    public class WelcomeMessageBulder
    {


        public async Task<string> CreateMessage(string template, SocketGuildUser User, SocketTextChannel rulechannel)
        {
            try
            {
                string ap="";
                if( String.IsNullOrWhiteSpace(template)==false && User!=null &&rulechannel!=null)
                {
                    if ( template.Contains("%user"))
                    {
                        ap= template.Replace("%user",User.Mention);
                    }

                    if (template.Contains("%channel"))
                    {
                        ap = template.Replace("%channel", rulechannel.Mention);
                    }
                    else if(template.Contains("%user") && template.Contains("%channel"))
                    {
                        ap = template.Replace("%user", User.Mention);
                        ap = ap.Replace("%channel", rulechannel.Mention);
                    }
                }

                return ap;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
    }
}
