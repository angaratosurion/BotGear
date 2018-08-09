using BotGear.Data;
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
        ServerManager srvMngr = new ServerManager();
        BotGearContext db = new BotGearContext();
        [Command("PreBan")]
        [Summary("preemptivelly bans the user with the id.")]
        [RequireUserPermission(GuildPermission.BanMembers)] ///Needed User Permissions ///
        [RequireBotPermission(GuildPermission.BanMembers)] ///Needed Bot Permissions ///
        public async Task PreBanAsync(string  user = null,   string reason = null)
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
        [Summary("removes preemptive ban of the user with the id")]
        [RequireUserPermission(GuildPermission.BanMembers)] ///Needed User Permissions ///
        [RequireBotPermission(GuildPermission.BanMembers)] ///Needed Bot Permissions ///
        public async Task PreUnBanAsync(string user = null,  string reason = null)
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
        [Command("GetPreBan")]
        [Summary("gets all the preemptive users of this server")]
        [RequireUserPermission(GuildPermission.BanMembers)] ///Needed User Permissions ///
        [RequireBotPermission(GuildPermission.BanMembers)] ///Needed Bot Permissions ///
        public async Task GetPreBanOnServAsync()
        {
            try
            {
                
                /*if (string.IsNullOrWhiteSpace(reason))
                {
                    await ReplyAsync("You must provide a reason");
                }*/
                var gld = Context.Guild as SocketGuild;
                var users=  prebnmngr.GetPreBannedUsers(Convert.ToString(gld.Id));
                var embed = new EmbedBuilder(); ///starts embed///
                embed.WithColor(new Color(0x4900ff)); ///hexacode colours ///
                embed.Title = $"Premptively Banned Users";///Who was banned///
                foreach (var user in users)
                {
                    var reason = user.Reason;
                    embed.Description += $"**Username: **{user.UserId}\n**Guild Name: **{Context.Guild.Name}\n**Reason: **{reason}\n"; ///Embed values///]

                }
                await Context.Channel.SendMessageAsync("", false, embed.Build());///sends embed///              


            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }

        [Command("GetPreBanAll")]
        [Summary("gets all the preemptive users of this bot")]
        [RequireOwner]//Needed Bot Permissions ///
        public async Task GetPreBanOnAllServAsync()
        {
            try
            {

                /*if (string.IsNullOrWhiteSpace(reason))
                {
                    await ReplyAsync("You must provide a reason");
                }*/
                var gld = Context.Guild as SocketGuild;
                var servers = db.Servers.ToList();
                
                var embed = new EmbedBuilder(); ///starts embed///
                embed.WithColor(new Color(0x4900ff)); ///hexacode colours ///
                embed.Title = $"Premptively Banned Users";///Who was banned///
                foreach (var serv in servers)
                {
                    var users = prebnmngr.GetPreBannedUsers(serv.Id);
                    embed.Description += $"*****{serv.Name}*****\n";
                    
                    foreach (var user in users)
                    {
                        var reason = user.Reason;
                        if (user != null)
                        {
                            embed.Description += $"**Username: **{user.UserId}\n**Guild Name: **{serv.Name}\n**Reason: **{reason}\n"; ///Embed values///]
                        }
                       

                    }
                    embed.Description += "\n";


                }
                var channel = await Context.User.GetOrCreateDMChannelAsync();
                await channel.SendMessageAsync("", false, embed.Build());///sends embed///              


            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
    }
}
