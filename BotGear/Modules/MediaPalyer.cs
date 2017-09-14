using BotGear.Tools;
using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BotGear.Modules
{
    [Export(typeof(ModuleBase))]
    public class MediaPlayer : ModuleBase
    {
        
        private IVoiceChannel _voiceChannel;
        private static  ITextChannel _textChannel;
        private readonly Queue<Tuple<string, string, string, string>> queue;
        private TaskCompletionSource<bool> _tcs;
        private CancellationTokenSource _disposeToken;
        private IAudioClient _audio;
        //private const string ImABot = " *I'm a Bot, beep boop blop*";
         

        private Queue<Tuple<string, string, string, string>> _queue;
        public MediaPlayer()
        {
            // _client = Context.Client;
           //_voiceChannel = Context.Guild.GetVoiceChannelsAsync(CacheMode.AllowDownload).Result.FirstOrDefault(x => x.Name.ToLower() == "Music".ToLower());

           // if (_voiceChannel == null)
           // {
           //       Context.Channel.SendMessageAsync("Create a Voice Channel with the name Music");
                

           // }
           // _textChannel= Context.Guild.GetTextChannelsAsync(CacheMode.AllowDownload).Result.FirstOrDefault(x => x.Name.ToLower() == "Music".ToLower());
           // if (_textChannel == null)
           // {
           //     Context.Channel.SendMessageAsync("Create a Text Channel with the name Music");
               
           // }
            _queue = new Queue<Tuple<string, string, string, string>>();
            _tcs = new TaskCompletionSource<bool>();
            _disposeToken = new CancellationTokenSource();
            InitThread();

        }
        public void InitThread()
        {
            //TODO: Main Thread or New Thread?
            //MusicPlay();
            var threadStart = new ParameterizedThreadStart(MusicPlay);


            new Thread(threadStart).Start(Context);
        }
        [Command("come",RunMode =RunMode.Async)]
        [Summary("Comes joins to the channel")]
        public async Task Come()
        {
            try
            {
                _audio?.Dispose();

                _voiceChannel = Context.Guild.GetVoiceChannelsAsync(CacheMode.AllowDownload).Result.FirstOrDefault(x => x.Name.ToLower() == "Music".ToLower());

                if (_voiceChannel == null)
                {
                   await  Context.Channel.SendMessageAsync("Create a Voice Channel with the name Music");


                }
                _textChannel = Context.Guild.GetTextChannelsAsync(CacheMode.AllowDownload).Result.FirstOrDefault(x => x.Name.ToLower() == "Music".ToLower());
                if (_textChannel == null)
                {
                    await Context.Channel.SendMessageAsync("Create a Text Channel with the name Music");

                }

                _audio = await _voiceChannel.ConnectAsync();
                
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("play",RunMode =RunMode.Async)]
        [Summary("Palys music")]
        public async Task Play()
        {
            try
            {
                Pause = false;
                 this.MusicPlay(Context);
                await _textChannel.SendMessageAsync($"{Context.User.Mention} resumed playback!");
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("pause", RunMode = RunMode.Async)]
        [Summary("Pauses music")]
        public async Task PauseCmd()
        {
            try
            {
                await _textChannel.SendMessageAsync($"{Context.User.Mention} paused playback!");
                Pause = true;
         
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("skip", RunMode = RunMode.Async)]
        [Summary("Skips to the next song")]
        public async Task Skipcmd()
        {
            try
            {
                await _textChannel.SendMessageAsync($"{Context.User.Mention} skipped **{_queue.Peek().Item2}**!");
                this.Skip = true;
                this.Pause = false;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("Add", RunMode = RunMode.Async)]
        [Summary("add only a song to play list ")]
        public async Task Add(string parameter)
        {
            try
            {
                if (parameter != null && _textChannel !=null)
                {
                    using (_textChannel.EnterTypingState())
                    {

                        //Test for valid URL
                        bool result = Uri.TryCreate(parameter, UriKind.Absolute, out Uri uriResult)
                                  && (uriResult.Scheme == "http" || uriResult.Scheme == "https");

                        //Answer
                        if (result)
                        {
                            try
                            { 
                                Tuple<string, string> info = await DownloadHelper.GetInfo(parameter);
                                await SendMessage($"{Context.User.Mention} requested \"{info.Item1}\" ({info.Item2})! Downloading now..." );

                                //Download
                                string file = await DownloadHelper.Download(parameter);
                                var vidInfo = new Tuple<string, string, string, string>(file, info.Item1, info.Item2, Context.User.Mention);

                                _queue.Enqueue(vidInfo);
                                Pause = false;
                                
                            }
                            catch (Exception ex)
                            {
                                
                                await SendMessage(
                                    $"Sorry {Context.User.Mention}, unfortunately I can't play that Song!" )    ;
                            }
                        }
                        else
                        {
                            await _textChannel.SendMessageAsync(
                                $"Sorry {Context.User.Mention}, but that was not a valid URL!" );
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("Addplaylist", RunMode = RunMode.Async)]
        [Summary("add a palylist to the queque")]
        public async Task AddPlaylist(string parameter)
        {
            try
            {
                if (parameter != null)
                {
                    using (_textChannel.EnterTypingState())
                    {

                        //Test for valid URL
                        bool result = Uri.TryCreate(parameter, UriKind.Absolute, out Uri uriResult)
                                      && (uriResult.Scheme == "http" || uriResult.Scheme == "https");

                        //Answer
                        if (result)
                        {
                            try
                            {
                                
                                Tuple<string, string> info = await DownloadHelper.GetInfo(parameter);
                                await SendMessage($" {Context.User.Mention}requested Playlist \"{info.Item1}\" ({info.Item2})! Downloading now..." 
                                                   );

                                //Download
                                string file = await DownloadHelper.DownloadPlaylist(parameter);
                                var vidInfo = new Tuple<string, string, string, string>(file, info.Item1, info.Item2, Context.User.Mention);

                                _queue.Enqueue(vidInfo);
                                Pause = false;
                                 
                            }
                            catch (Exception ex)
                            {
                                
                                await SendMessage(
                                    $"Sorry {Context.User.Mention}  unfortunately I can't play that Playlist!"  );
                            }
                        }
                        else
                        {
                            await _textChannel.SendMessageAsync(
                                $"Sorry  {Context.User.Mention}  but that was not a valid URL!"  );
                        }
                    }
                }
            }
            catch (Exception ex )
            {

                CommonTools.ErrorReporting(ex);
            }

        }
        [Command("clear", RunMode = RunMode.Async)]
        [Summary("Clears the queque")]
        public async Task Clear()
        {
            try
            {
                Pause = true;
                _queue.Clear();
                await SendMessage(
                           $"{Context.User.Mention}cleared the Playlist!" );
            }
            catch (Exception ex )
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
                await SendQueue(_textChannel);
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }

        }

        private bool Pause
        {
            get => _internalPause;
            set
            {
                new Thread(() => _tcs.TrySetResult(value)).Start();
                _internalPause = value;
            }
        }
        private bool _internalPause;
        private bool Skip
        {
            get
            {
                bool ret = _internalSkip;
                _internalSkip = false;
                return ret;
            }
            set => _internalSkip = value;
        }
        private bool _internalSkip;


       


        #region Discord Helper

        //Login as Bot and Start Bot
       

        //Log DiscordBot Messages to console
    
        //Send Message to channel
        public async Task SendMessage(string message)
        {
            if (_textChannel != null)
                await _textChannel.SendMessageAsync(message);
        }

        //Send Song queue in channel
        private async Task SendQueue(IMessageChannel channel)
        {
            EmbedBuilder builder = new EmbedBuilder()
            {
                Author = new EmbedAuthorBuilder { Name = "Song Queue" },
                Footer = new EmbedFooterBuilder() { Text = "(I don't actually sing)" },
                Color = Pause ? new Color(244, 67, 54) /*Red*/ : new Color(00, 99, 33) /*Green*/
            };
            //builder.ThumbnailUrl = "some cool url";
           
            if (_queue.Count == 0)
            {
                await channel.SendMessageAsync("Sorry, Song Queue is empty! Add some songs with the `!add [url]` command!" );
            }
            else
            {
                foreach (Tuple<string, string, string, string> song in _queue)
                {
                    builder.AddField($"{song.Item2} ({song.Item3})", $"by {song.Item4}");
                }

                await channel.SendMessageAsync("", embed: builder.Build());
            }
        }

       

        //Dispose this Object (Async)
       

        //Refresh Status of DiscordClient
     
        #endregion

       

        #region Audio
        //Audio: PCM | 48000hz | mp3

        //Get ffmpeg Audio Procecss
        private static Process GetFfmpeg(string path)
        {
            ProcessStartInfo ffmpeg = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-xerror -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,    //TODO: true or false?
                RedirectStandardOutput = true
            };
            return Process.Start(ffmpeg);
        }

        //Get ffplay Audio Procecss
        private static Process GetFfplay(string path)
        {
            ProcessStartInfo ffplay = new ProcessStartInfo
            {
                FileName = "ffplay",
                Arguments = $"-i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1 -autoexit",
                UseShellExecute = false ,   //TODO: true or false?
                RedirectStandardOutput = true
            };

            return new Process { StartInfo = ffplay };
        }

        //Send Audio with ffmpeg
        private async Task SendAudio(string path)
        {
            //FFmpeg.exe
            Process ffmpeg = GetFfmpeg(path);
            //Read FFmpeg output
             using (Stream output = ffmpeg.StandardOutput.BaseStream)
            //using (Stream output = File.OpenRead(path))
            {
                using (AudioOutStream discord = _audio.CreatePCMStream(AudioApplication.Music, 1920))
                {

                    //Adjust?
                    int bufferSize = 1024;
                    int bytesSent = 0;
                    bool fail = false;
                    bool exit = false;
                    byte[] buffer = new byte[bufferSize];

                    while (
                        !Skip &&                                    // If Skip is set to true, stop sending and set back to false (with getter)
                        !fail &&                                    // After a failed attempt, stop sending
                        !_disposeToken.IsCancellationRequested &&   // On Cancel/Dispose requested, stop sending
                        !exit                                       // Audio Playback has ended (No more data from FFmpeg.exe)
                            )
                    {
                        try
                        {
                            int read = await output.ReadAsync(buffer, 0, bufferSize, _disposeToken.Token);
                            if (read == 0)
                            {
                                //No more data available
                                exit = true;
                                break;
                            }

                            await discord.WriteAsync(buffer, 0, read, _disposeToken.Token);

                            if (Pause)
                            {
                                bool pauseAgain;

                                do
                                {
                                    pauseAgain = await _tcs.Task;
                                    _tcs = new TaskCompletionSource<bool>();
                                } while (pauseAgain);
                            }

                            bytesSent += read;
                        }
                        catch (TaskCanceledException)
                        {
                            exit = true;
                        }
                        catch
                        {
                            fail = true;
                            // could not send
                        }
                    }
                    //await output.CopyToAsync(discord);
                    await discord.FlushAsync();
                }
            }
        }

        //Looped Music Play
        private async void  MusicPlay(object _context)
        {
            bool next = false;
            ICommandContext context = (ICommandContext)_context;
            while (true)
            {
                bool pause = false;
                //Next song if current is over
                if (!next)
                {
                    pause = await _tcs.Task;
                    _tcs = new TaskCompletionSource<bool>();
                }
                else
                {
                    next = false;
                }

                try
                {
                    if (_queue.Count == 0)
                    {
                        await SendMessage("Nothing :/");
                      //  Print("Playlist ended.", ConsoleColor.Magenta);
                    }
                    else
                    {
                        if (!pause)
                        {
                            //Get Song
                            var song = _queue.Peek();
                            //Update "Playing .."
                           
                            await SendMessage($"Now playing: **{song.Item2}** ({song.Item3})");

                            //Send audio (Long Async blocking, Read/Write stream)
                            await SendAudio(song.Item1);

                            try
                            {
                                File.Delete(song.Item1);
                            }
                            catch
                            {
                                // ignored
                            }
                            finally
                            {
                                //Finally remove song from playlist
                                _queue.Dequeue();
                            }
                            next = true;
                        }
                    }
                }
                catch
                {
                    //audio can't be played
                }
            }
        }

        #endregion

    }
}
