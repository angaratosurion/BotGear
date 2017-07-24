using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Security.Cryptography;

namespace BotGear.Tools
{
  
    public class CommonTools
    {
       public  static bool IsWindows10()
        {

            var reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");

            string productName = (string)reg.GetValue("ProductName");

            return productName.StartsWith("Windows 10");
        }



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
        public static string getHash(byte [] cont)
        {
            try
            {
                string ap = null;

                if (cont != null)
                {
                    StringBuilder sBuilder = new StringBuilder();
                    using (var md5 = MD5.Create())
                    {
                        var data = md5.ComputeHash(cont);
                        for (int i = 0; i < data.Length; i++)
                        {
                            sBuilder.Append(data[i].ToString("x2"));
                            // sBuilder.Append(data[i].ToString());
                        }
                    }
                    ap = sBuilder.ToString();

                }
                    return ap;

                
            }
            catch (Exception ex)
            {

                ErrorReporting(ex);
                return null;
            }
        }
    }
}
