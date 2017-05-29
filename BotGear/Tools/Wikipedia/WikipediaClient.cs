using BotGear.Tools;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Tools.Wikipedia
{
    public  class WikipediaClient
    {

        public async Task<string>Search(string querry)
        {try
            {
                string ap = null;

                if (querry != null)
                {
                    WebClient client = new WebClient();
                    StringBuilder strbld = new StringBuilder();
                    Uri url = new Uri(String.Format("http://en.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&explaintext=1&titles={0}&redirects", querry));
                    using (Stream stream = client.OpenRead(url))

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        JsonSerializer ser = new JsonSerializer();
                        Result result = ser.Deserialize<Result>(new JsonTextReader(reader));

                        foreach (Page page in result.query.pages.Values)
                            strbld.AppendLine(page.extract);
                    }
                    ap = strbld.ToString();
                }
                return ap;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;
            }
        }

        //public async Task<string> Search(string querry)
        //{
        //    string ap = null;

        //    if (querry != null)
        //    {
        //        WebClient client = new WebClient();
        //        StringBuilder strbld = new StringBuilder();
        //        Uri url = new Uri(String.Format("http://en.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&explaintext=1&titles={0}&redirects", querry));
        //        using (Stream stream = client.OpenRead(url))

        //        using (StreamReader reader = new StreamReader(stream))
        //        {
        //            JsonSerializer ser = new JsonSerializer();
        //            Result result = ser.Deserialize<Result>(new JsonTextReader(reader));

        //            foreach (Page page in result.query.pages.Values)
        //                strbld.AppendLine(page.extract);
        //        }
        //        ap = strbld.ToString();
        //    }
        //    return ap;
        //}


    }
}
