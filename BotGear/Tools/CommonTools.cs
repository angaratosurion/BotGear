using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Tools
{
    public class CommonTools
    {

        public void cleanTemp()
        {

            try
            {
                string temp = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Temp");
                if (Directory.Exists(temp) == true)
                {
                    Directory.Delete(temp, true);


                    var files = Directory.GetFiles(temp);
                    if (files != null)
                    {
                        foreach (var f in files)
                        {
                            File.Delete(f);
                        }
                    }
                }
            }
            catch { }
        }
        public static void ErrorReporting(Exception ex)
        {
            //throw (ex);
          

                NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
                logger.Fatal(ex);
               
            

        }
    }
}
