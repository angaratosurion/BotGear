
using BotGear.Tools.Sankaku.SankakuChannelAPI;
using OneRandomImageDownloader.Sankaku.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BotGear.Tools.Sankaku
{
    public class SankakuDownloader
    {
        public static SankakuChannelUser User= new SankakuChannelUser("" , "");
        public async Task<string> DownloadRandomimage(string charact)
        {

            try
            { 
            int count, rndint=0;
            double sizeLimit = 0;
            int pageLimit = 20;
            int startingPage = 1, pageCount = 1, limit = 20;
            string ap = null;


            List<SankakuPost> foundPosts = new List<SankakuPost>();
                if (charact != null)
                {
                    string temp = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Temp");
                    //if (Directory.Exists(temp) == true)
                    //{
                    //    Directory.Delete(temp, true);
                    //}
                    Directory.CreateDirectory(temp);
                    DownloadStats stats = new DownloadStats();
                    //while (true)
                    {
                        var list = User.Search(charact, pageCount, limit);

                        if( list == null || list.Count==1)
                        {
                            return null;
                        }
                        foundPosts.AddRange(list);
                        if (list.Count == 1 && list[0] != null)
                        {
                            // break;
                        }
                        pageCount++;


                    }
                    stats.PostsFound = foundPosts.Count;
                    Random rnd = new Random();
                      
                    int max = foundPosts.Count - 1;
                    if (max == -1)
                    {
                        return null;
                    }
                    if ( max <0)
                    {
                        max = foundPosts.Count;
                    }
                     
                        rndint = rnd.Next(max);
                   
                   
                    SankakuPost a = foundPosts[rndint];
                    // foreach (var a in foundPosts)
                    {
                        download:
                        try
                        {
                            var imageLink = a.GetFullImageLink();

                            var imageLinkShortened = imageLink.Substring(imageLink.LastIndexOf('/') + 1);
                            Match match = new Regex(@"(.*?)(\.[a-z,0-5]{0,5})", RegexOptions.Singleline).Match(imageLinkShortened);
                            var filen = match.Groups[1].Value + match.Groups[2].Value;
                            string filename = temp + "\\" + filen;
                            var data = a.DownloadFullImage(imageLink, out bool wasRedirected, false, sizeLimit);
                            File.WriteAllBytes(filename, data);
                            ap = filename;

                        }
                        catch (UriFormatException ex)
                        {
                            CommonTools.ErrorReporting(ex);
                            // This exception gets thrown when a flash game is encountered on Sankaku and does not have a source link

                            // <param name=movie value="//cs.sankakucomplex.com/data/f6/23/f623ea7559ef39d96ebb0ca7530586b8.swf?3378073">



                        }
                        catch (Exception ex)
                        {
                            CommonTools.ErrorReporting(ex);
                            //WriteToLog("ERROR: " + ex.Message + $"({ex.GetType().ToString()})", exMessage: ex.Message);
                        }
                    }


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

