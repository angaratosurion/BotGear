using BotGear.Managers;
using BotGear.Tools;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Modules
{
    [Export(typeof(ModuleBase))]
    public class PreEmptiveBanining : ModuleBase
    {
        PreBannedUserManager prebnmngr = new PreBannedUserManager();
        [Command("PreBan")]
        [Summary("PreBan @Userid")]
        [RequireUserPermission(GuildPermission.BanMembers)] ///Needed User Permissions ///
        [RequireBotPermission(GuildPermission.BanMembers)] ///Needed Bot Permissions ///
        public async Task PreBanAsync(string  user = null, [Remainder] string reason = null)
        {
            try
            {
                if (user == null) { await ReplyAsync("You must mention a user"); }
                /*if (string.IsNullOrWhiteSpace(reason))
                {
                    await ReplyAsync("You must provide a reason");
                }*/
                var gld = Context.Guild as SocketGuild;
                await prebnmngr.banUser(user, reason, gld);
                var embed = new EmbedBuilder(); ///starts embed///
                embed.WithColor(new Color(0x4900ff)); ///hexacode colours ///
                embed.Title = $"**{user}** was banned";///Who was banned///
                embed.Description = $"**Username: **{user}\n**Guild Name: **{Context.Guild.Name}\n**Banned by: **{Context.User.Mention}!\n**Reason: **{reason}"; ///Embed values///]
               
                await Context.Channel.SendMessageAsync("", false, embed.Build());///sends embed///              


            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("PreUnBan")]
        [Summary("PreUnBan @Userid")]
        [RequireUserPermission(GuildPermission.BanMembers)] ///Needed User Permissions ///
        [RequireBotPermission(GuildPermission.BanMembers)] ///Needed Bot Permissions ///
        public async Task PreUnBanAsync(string user = null, [Remainder] string reason = null)
        {
            try
            {
                if (user == null) { await ReplyAsync("You must mention a user"); }
                /*if (string.IsNullOrWhiteSpace(reason))
                {
                    await ReplyAsync("You must provide a reason");
                }*/
                var gld = Context.Guild as SocketGuild;
                await prebnmngr.UnBanUser(user,gld);
                /*
                var embed = new EmbedBuilder(); ///starts embed///
                embed.WithColor(new Color(0x4900ff)); ///hexacode colours ///
                embed.Title = $"**{user}** was banned";///Who was banned///
                embed.Description = $"**Username: **{user}\n**Guild Name: **{Context.Guild.Name}\n**Banned by: **{Context.User.Mention}!\n**Reason: **{reason}"; ///Embed values///]
                */
                //await Context.Channel.SendMessageAsync("", false, embed.Build());///sends embed///              


            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
    }
}
