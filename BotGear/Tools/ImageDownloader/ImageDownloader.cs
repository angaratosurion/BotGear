
using FoxBooru;
using FoxBooru.Models;
using FoxBooru.Search;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BotGear.Tools.ImageDownloader
{
    public class DanbooruDownloader
    {
        MultySourceSearch sImage = new FoxBooru.Search.MultySourceSearch();


        public async Task<string> DownloadRandomimage(string charact)
        {

            try
            { 
            
            string ap = null;
                int rndint;
                


            List<ImageInfo> foundPosts = new List<ImageInfo>();
                if (charact != null)
                {
                    string temp = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Temp");
                    //if (Directory.Exists(temp) == true)
                    //{
                    //    Directory.Delete(temp, true);
                    //}
                    Directory.CreateDirectory(temp);
                    FoxBooru.Models.SearchOption sOption = new FoxBooru.Models.SearchOption(charact);
                    sOption.Rating = Ratings.All;
                    //while (true)
                    {
                        var list  = sImage.Search(sOption);

                        if ( list == null || list.Count==1)
                        {
                            return null;
                        }
                        foundPosts.AddRange(list);
                        if (list.Count == 1 && list[0] != null)
                        {
                            // break;
                        }
                       


                    }
                   
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
                   
                   
                    var a = foundPosts[rndint];
                    // foreach (var a in foundPosts)
                    {
                        download:
                        try
                        {
                            var imageLink = a.OrigUrl;

                           var imageLinkShortened = imageLink.Substring(imageLink.LastIndexOf('/') + 1);
                            Match match = new Regex(@"(.*?)(\.[a-z,0-5]{0,5})", RegexOptions.Singleline).Match(imageLinkShortened);
                            var filen = a.md5;//match.Groups[1].Value + match.Groups[2].Value;

                            WebClient client = new WebClient();
                            var data=client.DownloadData(a.OrigUrl);
                            string filename = temp + "\\" + filen+"."+ a.OrigUrl.Substring(a.OrigUrl.LastIndexOf("."));
                            //var data = a.DownloadFullImage(imageLink, out bool wasRedirected, false, sizeLimit);
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

