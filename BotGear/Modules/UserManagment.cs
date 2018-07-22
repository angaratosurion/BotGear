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
    public  class UserManagment : ModuleBase
    {
        [Command("Ban")]
        [Summary("Ban @Username")]
        [RequireUserPermission(GuildPermission.BanMembers)] ///Needed User Permissions ///
        [RequireBotPermission(GuildPermission.BanMembers)] ///Needed Bot Permissions ///
        public async Task BanAsync(SocketGuildUser user = null, [Remainder] string reason = null)
        {
            try
            {
                if (user == null) { await ReplyAsync("You must mention a user"); }
                if (string.IsNullOrWhiteSpace(reason))
                {
                    await ReplyAsync("You must provide a reason");
                }
                var gld = Context.Guild as SocketGuild;
                var embed = new EmbedBuilder(); ///starts embed///
                embed.WithColor(new Color(0x4900ff)); ///hexacode colours ///
                embed.Title = $"**{user.Username}** was banned";///Who was banned///
                embed.Description = $"**Username: **{user.Username}\n**Guild Name: **{user.Guild.Name}\n**Banned by: **{Context.User.Mention}!\n**Reason: **{reason}"; ///Embed values///]
                await gld.AddBanAsync(user);///bans selected user///
                await Context.Channel.SendMessageAsync("", false, embed.Build());///sends embed///              


            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }


        //[Command("UnBan")]
        //[Summary("UnBan @Username")]
        //[RequireUserPermission(GuildPermission.BanMembers)] ///Needed User Permissions ///
        //[RequireBotPermission(GuildPermission.BanMembers)] ///Needed Bot Permissions ///
        //public async Task UnBanAsync(SocketGuildUser user = null)
        //{
        //    try
        //    {
        //        if (user == null) { await ReplyAsync("You must mention a user"); }

        //        var gld = Context.Guild as SocketGuild;
        //        var embed = new EmbedBuilder(); ///starts embed///
        //        embed.WithColor(new Color(0x4900ff)); ///hexacode colours ///
        //        embed.Title = $"**{user.Username}** was Unbanned";///Who was banned///
        //        embed.Description = $"**Username: **{user.Username}\n**Guild Name: **{user.Guild.Name}\n**UnBanned by: **{Context.User.Mention}!\n**"; ///Embed values///]
        //        await gld.RemoveBanAsync(user);///bans selected user///
        //        await Context.Channel.SendMessageAsync("", false, embed);///sends embed///



        //    }
        //    catch (Exception ex)
        //    {
        //        CommonTools.ErrorReporting(ex);
        //    }
        //}
        [Command("Kick")]
        [RequireBotPermission(GuildPermission.KickMembers)] ///Needed BotPerms///
        [RequireUserPermission(GuildPermission.KickMembers)] ///Needed User Perms///
        public async Task KickAsync(SocketGuildUser user, [Remainder] string reason)
        {
            try
            {
                if (user == null) { await ReplyAsync("You must mention a user"); }
                if (string.IsNullOrWhiteSpace(reason))
                {
                    await ReplyAsync("You must provide a reason");
                }
                var gld = Context.Guild as SocketGuild;
                var embed = new EmbedBuilder(); ///starts embed///
                embed.WithColor(new Color(0x4900ff)); ///hexacode colours ///
                embed.Title = $" {user.Username} has been kicked from {user.Guild.Name}"; ///who was kicked///
                embed.Description = $"**Username: **{user.Username}\n**Guild Name: **{user.Guild.Name}\n**Kicked by: **{Context.User.Mention}!\n**Reason: **{reason}";
                ///embed values///
                ///
                await user.KickAsync(); ///kicks selected user///
                await Context.Channel.SendMessageAsync("", false, embed.Build()); ///sends embed///
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }

    }
}
