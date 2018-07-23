using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using ImageSharp;
using ImageSharp.Drawing.Pens;
using ImageSharp.Drawing.Brushes;
using ImageSharp.Processing;
using ImageSharp.Drawing;
using SixLabors.Fonts;
using ImageSharp.PixelFormats;
using System.Reflection;

namespace DiscordGamesPlugin
{
    public class ImageCore
    {
        private string randomString = ""; /*Creates a empty string*/

        public async Task<ImageSharp.Image<Rgba32>> StartStreamAsync(IUser user = null, string url = null, string path = null) /*Creates a async Task that returns a ImageSharp image with a user and url param*/
        {
            HttpClient httpClient = new HttpClient(); /*Creates a new HttpClient*/
            HttpResponseMessage response = null;
            ImageSharp.Image<Rgba32> image = null; /*Creates a null ImageSharp image*/

            if (user != null) /*Checks if the url is null aka the default value*/
            {
                try
                {/*try to get users avatar*/
                    response = await httpClient.GetAsync(user.GetAvatarUrl()); /*sets the response to the users avatar*/
                }
                catch
                {/*If they didnt set one, get a default one*/
                    response = await httpClient.GetAsync("https://discordapp.com/assets/1cbd08c76f8af6dddce02c5138971129.png"); /*sets the response to the url*/
                }
                Stream inputStream = await response.Content.ReadAsStreamAsync(); /*creates a inputStream variable and reads the url*/
                image = ImageSharp.Image.Load<Rgba32>(inputStream); /*Loads the image to the ImageSharp image we created earlier*/
                inputStream.Dispose();
            }
            if (url != null)/*Do this if a URL is given*/
            {
                response = await httpClient.GetAsync(url); /*sets the response to the url*/
                Stream inputStream = await response.Content.ReadAsStreamAsync(); /*creates a inputStream variable and reads the url*/
                image = ImageSharp.Image.Load<Rgba32>(inputStream); /*Loads the image to the ImageSharp image we created earlier*/
                inputStream.Dispose();
            }
            if (path != null)/*Do this is a path is given*/
            {
                Stream inputStream = File.Open(path, FileMode.Open);
                image = ImageSharp.Image.Load<Rgba32>(inputStream);
                inputStream.Dispose();
            }
            return image; /*returns the image*/

        }

        public async Task StopStreamAsync(IUserMessage msg, ImageSharp.Image<Rgba32> image) /*Creates an async task with UserMessage as a paramater */
        {
            string input = "abcdefghijklmnopqrstuvwxyz0123456789"; /*alphabet abcdefghijklmaopqrstuvwxy and z! now I know my abc's!*/
            char ch;
            string temp = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Temp");
            Random rand = new Random();
            for (int i = 0; i < 8; i++)/*loops through the alphabet and creates a random string with 8 random characters */
            {
                ch = input[rand.Next(0, input.Length)];
                randomString += ch;
            }
            if (image != null) /*Checks if the image we created is not null if it is then this part won't run*/
            {
                Stream outputStream = new MemoryStream();
                image.SaveAsPng(outputStream); /*saves the image as a jpg you can of course change this*/
                outputStream.Position = 0;
                var file = File.Create(temp+$"/{randomString}.png"); /*creates a file with the random string as the name*/
                await outputStream.CopyToAsync(file);
                file.Dispose();
                await msg.Channel.SendFileAsync(temp+$"/{randomString}.png"); /*sends the image we just created*/
                File.Delete($"temp+/{randomString}.png"); /*deletes the image after sending*/
            }
        }
    }
}

