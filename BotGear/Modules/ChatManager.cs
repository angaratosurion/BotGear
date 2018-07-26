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
    public  class ChatManager : ModuleBase
    {
        [Command("purge")]
        [Summary("Deletes all messages.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task ClearChannel()
        {
            try
            {
                // var messages =await  Context.Channel.GetMessagesAsync(100, CacheMode.AllowDownload, null).Flatten();
                var messages = await Context.Channel.GetMessagesAsync(Context.Message, Direction.Before, int.MaxValue, CacheMode.AllowDownload, null).Flatten();

               
                    Context.Channel.DeleteMessagesAsync(messages);

                    
               


               await ReplyAsync(String.Format("Channel got Cleaned!"));

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("purge")]
        [Summary("Deletes all messages.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        public async Task ClearChannel(string name)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(name)) return;
                var channels = await Context.Guild.GetTextChannelsAsync(CacheMode.AllowDownload);
                var channel   =  channels.FirstOrDefault(x => x.Name == name||x.Mention==name);
                if (channel == null) return;
                // var messages =await  Context.Channel.GetMessagesAsync(100, CacheMode.AllowDownload, null).Flatten();
                var messages = await channel.GetMessagesAsync(Context.Message, Direction.Before, int.MaxValue, CacheMode.AllowDownload, null).Flatten();

               // while (messages != null)
                //{
                    channel.DeleteMessagesAsync(messages);

                    ///messages = await  channel.GetMessagesAsync(Context.Message, Direction.Before, 10, CacheMode.AllowDownload, null).Flatten();

                //}
               await ReplyAsync(String.Format ("Channel {0} got Cleaned!",channel.Name));
              



            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("create_textchannel")]
        [Summary("Creates a new Text  Channel.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireBotPermission(ChannelPermission.ManageChannel)]
        public async Task CreateTextChannel(string name)//,string category ,string description)
        {
            try
            {
                // var messages =await  Context.Channel.GetMessagesAsync(100, CacheMode.AllowDownload, null).Flatten();
                if (String.IsNullOrEmpty(name) == false )//&& String.IsNullOrEmpty(category) == false && String.IsNullOrEmpty(description) == false)
                {
                    var guild = Context.Guild;
                    var newchannel =await guild.CreateTextChannelAsync(name, null);
                    await ReplyAsync(String.Format("Channel {0} have been Created!", newchannel.Mention));

                }




            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("delete_textchannel")]
        [Summary("Deletes the Text  Channel.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireBotPermission(ChannelPermission.ManageChannel)]
        public async Task DeleteTextChannel(string name)//,string category ,string description)
        {
            try
            {
                // var messages =await  Context.Channel.GetMessagesAsync(100, CacheMode.AllowDownload, null).Flatten();
                if (String.IsNullOrEmpty(name) == false)//&& String.IsNullOrEmpty(category) == false && String.IsNullOrEmpty(description) == false)
                {
                    var guild = Context.Guild;
                    var channels = await guild.GetTextChannelsAsync(CacheMode.AllowDownload);
                    if (channels != null &&  channels.FirstOrDefault(x => x.Name == name || x.Mention == name) != null)
                    {
                        var channel =  channels.FirstOrDefault(x => x.Name == name || x.Mention == name);
                        await channel.DeleteAsync(null);
                        await ReplyAsync(String.Format("Channel {0} have been Deleted!", channel.Mention));
                    }

                }




            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("create_voicechannel")]
        [Summary("Creates a new Voice  Channel.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireBotPermission(ChannelPermission.ManageChannel)]
        public async Task CreateVoiceChannel(string name)//,string category ,string description)
        {
            try
            {
                // var messages =await  Context.Channel.GetMessagesAsync(100, CacheMode.AllowDownload, null).Flatten();
                if (String.IsNullOrEmpty(name) == false)//&& String.IsNullOrEmpty(category) == false && String.IsNullOrEmpty(description) == false)
                {
                    var guild = Context.Guild;
                    var newchannel = await guild.CreateVoiceChannelAsync(name, null);
                    await ReplyAsync(String.Format("Channel {0} have been Created!", name));

                }




            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("delete_voicechannel")]
        [Summary("Deletes the voice Channel.")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.ManageChannels)]
        [RequireBotPermission(ChannelPermission.ManageChannel)]
        public async Task DeleteVoiceChannel(string name)//,string category ,string description)
        {
            try
            {
                // var messages =await  Context.Channel.GetMessagesAsync(100, CacheMode.AllowDownload, null).Flatten();
                if (String.IsNullOrEmpty(name) == false)//&& String.IsNullOrEmpty(category) == false && String.IsNullOrEmpty(description) == false)
                {
                    var guild = Context.Guild;
                    var channels = await guild.GetVoiceChannelsAsync(CacheMode.AllowDownload);
                    if (channels != null && channels.FirstOrDefault(x => x.Name == name ) != null)
                    {
                        var channel = channels.FirstOrDefault(x => x.Name == name );
                        await channel.DeleteAsync(null);
                        await ReplyAsync(String.Format("Channel {0} have been Deleted!", name));
                    }

                }




            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
    }
}
