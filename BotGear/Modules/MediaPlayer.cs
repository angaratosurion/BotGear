using BotGear.Tools;
using Discord;
using Discord.Audio;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Modules
{
    [Export(typeof(ModuleBase))]
    public class MediaPlayer : ModuleBase
    {
        IVoiceChannel channel;
        IAudioClient client;
        private static  Queue<Tuple<string, string, string, string,ulong>> _queue = new Queue<Tuple<string, string, string, string,ulong>();
        private static Dictionary<ulong, int> CurrentSong = new Dictionary<ulong, int>();
        private Process CreateStream(string url)
        {
            try
            {
                Process currentsong = new Process();

                currentsong.StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C youtube-dl.exe -o - {url} | ffmpeg -i pipe:0 -ac 2 -f s16le -ar 48000 pipe:1",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                currentsong.Start();
                return currentsong;
            }
            catch (Exception ex)
            {
                BotGear.Tools.CommonTools.ErrorReporting(ex);
               // Context.Channel.SendMessageAsync(ex.ToString());
                return null;
            }
        }
        [Command("play",RunMode =RunMode.Async)]
        [Summary("Plays Music")]
        public async Task Play(string url)
        {
            try
            {
                channel = Context.Guild.GetVoiceChannelsAsync(CacheMode.AllowDownload).Result.FirstOrDefault(x => x.Name.ToLower() == "Music".ToLower());

                if (channel == null)
                {
                    await Context.Channel.SendMessageAsync("Create a Voice Channel with the name Music");
                    return;

                }
                if (url != null)
                {
                    //(Context.User as IVoiceState).VoiceChannel;
                    client = await channel.ConnectAsync();


                    var output = CreateStream(url).StandardOutput.BaseStream;
                    var stream = client.CreatePCMStream(AudioApplication.Music, 64 * 1024);
                    output.CopyToAsync(stream);
                    stream.FlushAsync().ConfigureAwait(false);
                }
                else
                {
                    if (_queue.Count > 0)
                    {
                        var song = _queue.Peek();
                        await this.Play(song.Item1);

                    }
                }

            }
            catch (Exception ex)
            {
                BotGear.Tools.CommonTools.ErrorReporting(ex);
                //await Context.Channel.SendMessageAsync(ex.ToString());
            }
        }
        [Command("play", RunMode = RunMode.Async)]
        [Summary("Plays Music")]
        public async Task Play()
        {
            try
            {
                channel = Context.Guild.GetVoiceChannelsAsync(CacheMode.AllowDownload).Result.FirstOrDefault(x => x.Name.ToLower() == "Music".ToLower());

                if (channel == null)
                {
                    await Context.Channel.SendMessageAsync("Create a Voice Channel with the name Music");
                    return;

                }
               
                    if (_queue.Count > 0)
                    {
                    var song = _queue.First(x => x.Item5 == Context.Guild.Id);
                        await this.Play(song.Item1);

                    }
               

            }
            catch (Exception ex)
            {
                BotGear.Tools.CommonTools.ErrorReporting(ex);
                //await Context.Channel.SendMessageAsync(ex.ToString());
            }
        }
        [Command("stop", RunMode = RunMode.Async)]
        [Summary("Stops the  Music")]
        public async Task Stop()
        {
            try
            { 
                Process killffmpeg = new Process();


                killffmpeg.StartInfo = new ProcessStartInfo
                {
                    FileName = "taskkill",
                    Arguments = $"/f /im \"ffmpeg.exe\"",
                   UseShellExecute = false,
                  RedirectStandardOutput = true,
                    CreateNoWindow = true
                };
                killffmpeg.Start();




                Process killyoutubedll = new Process();
                killyoutubedll.StartInfo = new ProcessStartInfo
                {
                    FileName = "taskkill",
                    Arguments = $"/f /im \"youtube-dl.exe\"",
                   UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };
                killyoutubedll.Start();
                

                await this.Clear();
            }
            catch (Exception ex)
            {
                BotGear.Tools.CommonTools.ErrorReporting(ex);
                //await Context.Channel.SendMessageAsync(ex.ToString());
            }
        }
        [Command("Add", RunMode = RunMode.Async)]
        [Summary("add only a song to play list ")]
        public async Task Add(string url)
        {
            try
            {
                if (url != null)
                {
                    bool result = Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                                  && (uriResult.Scheme == "http" || uriResult.Scheme == "https");

                    //Answer
                    if (result)
                    {

                        Tuple<string, string> info = await DownloadHelper.GetInfo(url);
                        await Context.Channel.SendMessageAsync($"{Context.User.Mention} requested \"{info.Item1}\" ({info.Item2})! Downloading now...");
                        var vidInfo = new Tuple<string, string, string, string,ulong>(url, info.Item1, info.Item2, Context.User.Mention,Context.Guild.Id);

                        _queue.Enqueue(vidInfo);

                        //Download

                    }
                }
                

            }
            catch (Exception ex )
            {
                BotGear.Tools.CommonTools.ErrorReporting(ex);
                await Context.Channel.SendMessageAsync(
                                   $"Sorry {Context.User.Mention}, unfortunately I can't play that Song!");
            }
        }
        [Command("clear", RunMode = RunMode.Async)]
        [Summary("Clears the queque")]
        public async Task Clear()
        {
            try
            {
                // Pause = true;
                var songs=_queue.Where(x=>x.Item5==Context.Guild.Id).ToList();
                if (songs != null)
                {

                    //foreach(var q in songs)
                    //{

                    _queue.SkipWhile(x => x.Item5 == Context.Guild.Id);


                    //}
                    await Context.Channel.SendMessageAsync(
                               $"{Context.User.Mention}cleared the Playlist!");
                }
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("queue", RunMode = RunMode.Async)]
        [Summary("Shows  the queue")]
        public async Task Queue()
        {
            try
            {
                await SendQueue(Context.Channel);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }

        }

        [Command("skip", RunMode = RunMode.Async)]
        [Summary("Skips to the next song")]
        public async Task Skip()
        {
            try
            {
                await Context.Channel.SendMessageAsync($"{Context.User.Mention} skipped **{_queue.Peek().Item2}**!");
                if (channel!=null )
                {
                    if(_queue.Count==0)
                    {

                    }
                    var song = _queue.Peek();
                    await Context.Channel.SendMessageAsync($"Now playing: **{song.Item2}** ({song.Item3})");

                    await this.Play(song.Item1);

                }
                
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
            }
        }

        private async Task SendQueue(IMessageChannel tchannel)
        {
            EmbedBuilder builder = new EmbedBuilder()
            {
                Author = new EmbedAuthorBuilder { Name = "Song Queue" },
                Footer = new EmbedFooterBuilder() { Text = "(I don't actually sing)" },
                Color =  new Color(244, 67, 54) ///*Red*/ : new Color(00, 99, 33) /*Green*/
                //Color = Pause ? new Color(244, 67, 54) /*Red*/ : new Color(00, 99, 33) /*Green*/
            };
            //builder.ThumbnailUrl = "some cool url";
             var songs = _queue.Where(x => x.Item5 == Context.Guild.Id).ToList();
            if (songs!=null &&songs.Count == 0)
            {
                await tchannel.SendMessageAsync("Sorry, Song Queue is empty! Add some songs with the `!add [url]` command!");
            }
            else
            {
                foreach (Tuple<string, string, string, string,ulong> song in songs)
                {
                    builder.AddField($"{song.Item2} ({song.Item3})", $"by {song.Item4}");
                }

                await tchannel.SendMessageAsync("", embed: builder.Build());
            }
        }

        private async Task GetNextSong()
        {
            try
            {
                var songs = _queue.Where(x => x.Item5 == Context.Guild.Id).ToList();
               if ( songs!=null)
                {
                    
                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
            }
        }

    }
}
