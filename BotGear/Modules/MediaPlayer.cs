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
using System.Threading;
using System.Threading.Tasks;

namespace BotGear.Modules
{
    [Export(typeof(ModuleBase))]
    public class MediaPlayer : ModuleBase
    {
        IVoiceChannel channel;
        IAudioClient client;
        private static  List<Tuple<string, string, string, string,ulong,int>> _queue = new List<Tuple<string, string, string, string,ulong,int>>();
        private static Dictionary<ulong, string> CurrentSong = new Dictionary<ulong, string>();
        private static Dictionary<ulong, Process> FfmpegInstancces = new Dictionary<ulong, Process>();
        private static Dictionary<ulong, Thread> Musicthreads = new Dictionary<ulong, Thread>();
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
                FfmpegInstancces.Add(Context.Guild.Id, currentsong);
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
                        var song = _queue[0];
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
        [Summary("Plays Music without adding song to queque")]
        public async Task PlayCont()
        {
            try
            {
                channel = Context.Guild.GetVoiceChannelsAsync(CacheMode.AllowDownload).Result.FirstOrDefault(x => x.Name.ToLower() == "Music".ToLower());

                if (channel == null)
                {
                    await Context.Channel.SendMessageAsync("Create a Voice Channel with the name Music");
                    return;

                }
                if (!(await this.CheckIfMusicthreadExistsexists(Context.Guild.Id)))
                {
                    ThreadStart threadStart = new ThreadStart(this.MediaPlayCont);
                 var thr =   new Thread(threadStart);
                    thr.Start();
                    Musicthreads.Add(Context.Guild.Id, thr);

                }
                //var songs = await this.GetServerQueue();
                               
                //   if (songs != null && songs.Count > 0)
                //{
                //    foreach (var song in songs)
                //    {
                //        if (await this.CheckIfFFmpegexists(Context.Guild.Id) != true)
                //        {

                //            await this.Play(song.Item1);
                //            if (CurrentSong.ContainsKey(Context.Guild.Id) != true)
                //            {
                //                CurrentSong.Add(Context.Guild.Id, song.Item1);
                //            }
                //            else
                //            {
                //                CurrentSong[Context.Guild.Id] = song.Item1;
                //            }


                //        }
                //        else
                //        {
                //            //TimeSpan spn = TimeSpan.Parse(song.Item3);

                //            // Thread.Sleep((int)spn.TotalMilliseconds);
                //            (await this.GetFFmpegInstance(Context.Guild.Id)).WaitForExit();
                //            await this.Play(song.Item1);
                //            if (CurrentSong.ContainsKey(Context.Guild.Id) != true)
                //            {
                //                CurrentSong.Add(Context.Guild.Id, song.Item1);
                //            }
                //            else
                //            {
                //                CurrentSong[Context.Guild.Id] = song.Item1;
                //            }
                //        }



                //    }

                //}
         
               

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
                //Process killffmpeg = new Process();


                //killffmpeg.StartInfo = new ProcessStartInfo
                //{
                //    FileName = "taskkill",
                //    Arguments = $"/f /im \"ffmpeg.exe\"",
                //   UseShellExecute = false,
                //  RedirectStandardOutput = true,
                //    CreateNoWindow = true
                //};
                //killffmpeg.Start();




                //Process killyoutubedll = new Process();
                //killyoutubedll.StartInfo = new ProcessStartInfo
                //{
                //    FileName = "taskkill",
                //    Arguments = $"/f /im \"youtube-dl.exe\"",
                //   UseShellExecute = false,
                //    RedirectStandardOutput = true,
                //    CreateNoWindow = true
                //};
                //killyoutubedll.Start();
                if ((await this.CheckIfFFmpegexists(Context.Guild.Id)) == true)
                {
                    FfmpegInstancces[Context.Guild.Id].Kill();
                }
                if (await CheckIfMusicthreadExistsexists(Context.Guild.Id))
                {
                    var x=await GetMusicThreadInstance(Context.Guild.Id);
                    x.Abort();
                }
                   

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
                        int index = _queue.Count - 1;
                        if ( index<0)
                        {
                            index = 0;
                        }
                        var vidInfo = new Tuple<string, string, string, string,ulong,int>(url, info.Item1, info.Item2, Context.User.Mention,Context.Guild.Id,index);

                        _queue.Add(vidInfo);
                       await  this.PlayCont();

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
                var cur =await this.GetCurrentSong();
                await Context.Channel.SendMessageAsync($"{Context.User.Mention} skipped **{cur.Item2}**!");
                if (channel!=null )
                {
                    if(_queue.Count==0)
                    {

                    }
                    var song = await this.GetNextSong();
                    await Context.Channel.SendMessageAsync($"Now playing: **{song.Item2}** ({song.Item3})");

                    await this.Play(song.Item1);
                   

                }
                
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
            }
        }



        public void  MediaPlayCont()
        {
            try
            {
                var songs =  this.GetServerQueue().Result;
               
                    if (songs != null && songs.Count > 0)
                    {
                    var song = songs[0];
                        while (song !=null)
                        {
                            if (this.CheckIfFFmpegexists(Context.Guild.Id).Result != true)
                            {

                                 this.Play(song.Item1);
                                if (CurrentSong.ContainsKey(Context.Guild.Id) != true)
                                {
                                    CurrentSong.Add(Context.Guild.Id, song.Item1);
                                }
                                else
                                {
                                    CurrentSong[Context.Guild.Id] = song.Item1;
                                }


                            }
                            else
                            {
                                //TimeSpan spn = TimeSpan.Parse(song.Item3);

                                // Thread.Sleep((int)spn.TotalMilliseconds);
                                ( this.GetFFmpegInstance(Context.Guild.Id)).Result.WaitForExit();
                                  this.Play(song.Item1);
                                if (CurrentSong.ContainsKey(Context.Guild.Id) != true)
                                {
                                    CurrentSong.Add(Context.Guild.Id, song.Item1);
                                }
                                else
                                {
                                    CurrentSong[Context.Guild.Id] = song.Item1;
                                }
                            }
                        song = this.GetNextSong().Result;



                        }

                    }
                


            }
            catch (Exception ex)
            {
                BotGear.Tools.CommonTools.ErrorReporting(ex);
                //await Context.Channel.SendMessageAsync(ex.ToString());
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
            var songs = await  this.GetServerQueue();
            if (songs!=null &&songs.Count == 0)
            {
                await tchannel.SendMessageAsync("Sorry, Song Queue is empty! Add some songs with the `!add [url]` command!");
            }
            else
            {
                foreach (Tuple<string, string, string, string,ulong,int> song in songs)
                {
                    builder.AddField($"{song.Item2} ({song.Item3})", $"by {song.Item4}");
                }

                await tchannel.SendMessageAsync("", embed: builder.Build());
            }
        }
        private async Task<List<Tuple<string, string, string, string, ulong,int>>> GetServerQueue()
        {
            try
            {
                List < Tuple<string, string,string, string,ulong,int >>ap = null;

                if(_queue !=null)
                {
                    ap = _queue.FindAll(x => x.Item5 == Context.Guild.Id);
                }

                return ap;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }

        private async Task<Tuple<string, string, string, string,ulong,int >> GetNextSong( )
        {
            try
            {
                Tuple<string, string, string, string, ulong,int> ap = null;
                var quarr = _queue.ToArray();
                var songs = await GetServerQueue();
               if ( songs!=null)
                {
                    int i = -1;
                    var cur = await this.GetCurrentSong();
                    i = cur.Item6 ;
                    if ( i>-1 &&(i+1)<_queue.Count)
                    {
                        ap = _queue[i+1];
                    }

                    //songs.FindIndex(x=>x.Item1==CurrentSong[Context.Guild.Id]);   
                }
                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        private async Task<Boolean> CheckIfFFmpegexists(ulong guildid)
        {
            try
            {
                Boolean a = false;
                
                
                    a = FfmpegInstancces.ContainsKey(guildid);
               
                return a;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false;
            }
        }
        
        private async Task<Process> GetFFmpegInstance(ulong guildid)
        {
            try
            {
                Process a = null;


                if (await this.CheckIfFFmpegexists(Context.Guild.Id))
                {
                    a = FfmpegInstancces[Context.Guild.Id];
                }

                return a;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        private async Task<Boolean> CheckIfMusicthreadExistsexists(ulong guildid)
        {
            try
            {
                Boolean a = false;


                a = Musicthreads.ContainsKey(guildid);

                return a;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false;
            }
        }
        private async Task<Thread> GetMusicThreadInstance(ulong guildid)
        {
            try
            {
                Thread a = null;


                if (await this.CheckIfMusicthreadExistsexists(Context.Guild.Id))
                {
                    a = Musicthreads[Context.Guild.Id];
                }

                return a;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        private async Task<Tuple<string, string, string, string, ulong,int>> GetCurrentSong()
        {
            try
            {
                Tuple<string, string, string, string, ulong,int> ap = null;
                
                var songs = await this.GetServerQueue();
                if (songs != null && CurrentSong.ContainsKey(Context.Guild.Id)==true)
                {
                    ap = songs.First(x => x.Item1 == CurrentSong[Context.Guild.Id]);
                    //songs.FindIndex(x=>x.Item1==CurrentSong[Context.Guild.Id]);   
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
