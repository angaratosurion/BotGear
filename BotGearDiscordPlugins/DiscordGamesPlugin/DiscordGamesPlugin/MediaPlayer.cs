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
using System.Reflection;

namespace DiscordGamesPlugin
{
    //[Export(typeof(ModuleBase))]
    //public class MediaPlayer : ModuleBase
    //{
    //    private Process CreateStream(string url)
    //    {
    //        try
    //        {
    //            Process currentsong = new Process();

    //            currentsong.StartInfo = new ProcessStartInfo
    //            {
    //                FileName = "cmd.exe",
    //                Arguments = $"/C youtube-dl.exe -o - {url} | ffmpeg -i pipe:0 -ac 2 -f s16le -ar 48000 pipe:1",
    //                UseShellExecute = !false,
    //                RedirectStandardOutput = true,
    //                CreateNoWindow = true
    //            };

    //            currentsong.Start();
    //            return currentsong;
    //        }
    //        catch (Exception ex )
    //        {
    //            BotGear.Tools.CommonTools.ErrorReporting(ex);
    //             Context.Channel.SendMessageAsync(ex.ToString());
    //            return null;
    //        }
    //    }
    //    [Command("play")]
    //    [Summary("Plays Music")]
    //    public async Task play(string url)
    //    {
    //        try
    //        {
    //            IVoiceChannel channel = Context.Guild.GetVoiceChannelsAsync(CacheMode.AllowDownload).Result.FirstOrDefault(x => x.Name.ToLower() == "Music".ToLower());

    //                if ( channel ==null)
    //            {
    //                await Context.Channel.SendMessageAsync("Create a Voice Channel with the name Music");
    //                return;

    //            }
    //                //(Context.User as IVoiceState).VoiceChannel;
    //        IAudioClient client = await channel.ConnectAsync();

    //        var output = CreateStream(url).StandardOutput.BaseStream;
    //        var stream = client.CreatePCMStream(AudioApplication.Music, 128 * 1024);
    //        output.CopyToAsync(stream);
    //        stream.FlushAsync().ConfigureAwait(false);

    //        }
    //        catch (Exception ex)
    //        {
    //            BotGear.Tools.CommonTools.ErrorReporting(ex);
    //            await Context.Channel.SendMessageAsync(ex.ToString());
    //        }
    //    }
    //}
}
