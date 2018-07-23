using Discord;
using Discord.Commands;
using Discord.WebSocket;
using ImageSharp;
using SixLabors.Primitives;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;
using ImageSharp.Drawing.Brushes;
using ImageSharp.Processing;
using SixLabors.Fonts;

using ImageSharp.Dithering;
using System.Drawing;
using System.Reflection;
using System.IO;
namespace DiscordGamesPlugin
{
    [Export(typeof(ModuleBase))]
 public    class SpinBottleGame : ModuleBase
    {
        //[Command("spin")]
        //public async Task DeathSpinAsync(IGuildUser _user1 = null, IGuildUser _user2 = null, IGuildUser _user3 = null, IGuildUser _user4 = null)
        //{
        //    /*We can input 4 users to make the "gameboard". If no users are given, it will check to see if you in a voice channel also. 
        //     * If not, get 4 random users from the server.*/
        //    Random rand = new Random();/*Random number generator*/
        //    IVoiceChannel channel2 = null;/*In case wher are in a voice channel*/
        //    IGuildUser[] randomUsers = new IGuildUser[4];/*A 1 dimensional array of size 4 for our users*/
        //    try
        //    {
        //        /*If the user calling the command isn't in voice channel the we can move on
        //         *This can probablt be done better ¯\_(ツ)_/¯ */
        //        channel2 = (Context.User as IVoiceState).VoiceChannel;
        //        /*If we are in a voice channel, pick the 4 ppl in it, this is kinda a custom set up ;)
        //         * atm only works with a vpoce channel with 4 or more users.*/
        //        var vc = channel2 as SocketVoiceChannel;
        //        var voiceUsers = vc.Users;
        //        /*Get users 0-3, or 4 users*/
        //        randomUsers[0] = voiceUsers.ElementAt(0);
        //        randomUsers[1] = voiceUsers.ElementAt(1);
        //        randomUsers[2] = voiceUsers.ElementAt(2);
        //        randomUsers[3] = voiceUsers.ElementAt(3);

        //    }
        //    catch
        //    {
        //        /*If the user isn't in a voice channel, get users from the server*/
        //        var users = await Context.Guild.GetUsersAsync();
        //        randomUsers[0] = users.ElementAt(rand.Next(0, users.Count));
        //        randomUsers[1] = users.ElementAt(rand.Next(0, users.Count));
        //        randomUsers[2] = users.ElementAt(rand.Next(0, users.Count));
        //        randomUsers[3] = users.ElementAt(rand.Next(0, users.Count));
        //    }
        //    /*If any user names are given with the command, set them to over write random users*/
        //    if (_user1 != null) randomUsers[0] = _user1;
        //    if (_user2 != null) randomUsers[1] = _user2;
        //    if (_user3 != null) randomUsers[2] = _user3;
        //    if (_user4 != null) randomUsers[3] = _user4;
        //    /*Using ImageCore*/
        //    ImageCore core = new ImageCore();
        //    /*Load our users avatars*/
        //    ImageSharp.Image<Rgba32> user1 = await core.StartStreamAsync(randomUsers[0]);
        //    ImageSharp.Image<Rgba32> user2 = await core.StartStreamAsync(randomUsers[1]);
        //    ImageSharp.Image<Rgba32> user3 = await core.StartStreamAsync(randomUsers[2]);
        //    ImageSharp.Image<Rgba32> user4 = await core.StartStreamAsync(randomUsers[3]);
        //    /*Lead the arrow image, but using "path:" we are tellint what paramater we are passing*/

        //    ImageSharp.Image<Rgba32> arrow = await core.StartStreamAsync(path: "Images/arrow.png");
        //    /*Image we will draw the other images into. and display at the end*/
        //    ImageSharp.Image<Rgba32> finalImage = new ImageSharp.Image<Rgba32>(500, 500);
        //    /*We need to make a size varaiable to make it easy to resize our images, avatars are 128x128 px, and we want to make them a little bigger*/
        //   Size size250 = new Size(250, 250);
        //   Size size500 = new Size(500, 500);
        //    /*Set size to 250x250 px*/
        //    user1.Resize(size250);
        //    user2.Resize(size250);
        //    user3.Resize(size250);
        //    user4.Resize(size250);
        //    /*Now we can start the fun part.  Now we draw each image into the larger final image*/
        //    finalImage.DrawImage(user1, 1f, size250, new Point(0, 0));
        //    finalImage.DrawImage(user2, 1f, size250, new Point(250, 0));
        //    finalImage.DrawImage(user3, 1f, size250, new Point(0, 250));
        //    finalImage.DrawImage(user4, 1f, size250, new Point(250, 250));
        //    /*Get the random direction to point the arrow*/
        //    float dir = rand.Next(0, 360);
        //    /*String for the user that the arrow points too*/
        //    string winner = null;
        //    /*Determine which quadrant the arrow is aimed.*/
        //    if (dir > 270 && dir < 360) winner = randomUsers[0].Username;
        //    if (dir > 0 && dir < 90) winner = randomUsers[1].Username;
        //    if (dir > 90 && dir < 180) winner = randomUsers[3].Username;
        //    if (dir > 180 && dir < 270) winner = randomUsers[2].Username;
        //    /*In the rare instance the arrow is at a 90 degree angle, its not pointing at anyone!*/
        //    if (winner == null) winner = "No one, try again";
        //    /*Rotate the arrow image.*/
        //    arrow.Rotate(dir);
        //    /*Then draw it on top off the final image.*/
        //    finalImage.DrawImage(arrow, 1f, size500, new Point(0, 0));
        //    /*Use ImageCore to save the image and send it to discord*/
        //    await core.StopStreamAsync(Context.Message, finalImage);
        //    /*Finally tell, state the user name of the winner, incase its a massive server and you don't know people by there avatar!*/
        //    await Context.Channel.SendMessageAsync($"The spinner is pointing at **{winner}**!");
        //}
    }
}
