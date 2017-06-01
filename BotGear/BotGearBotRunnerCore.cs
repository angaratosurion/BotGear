using BotGear.Attributes.Assembly;
using BotGear.Composition;
using BotGear.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BotGear
{
   public  class BotGearBotRunnerCore
    {
        static List<IBot> Bots;
        
        public static IEnumerable<IBotInfo> GetAllModulesInfo()
        {
            List<IBotInfo> ap = new List<IBotInfo>();

            ap = GetAssembliesInfo();

            /* foreach (var inf in ModuleInfos)
             {
                 ap.Add(inf.Value);
             }*/

            return ap;
        }
        //public static List<IBot> GetBots()
        //{
        //    try
        //    {
        //        List<IBot>  ap = new List<IBot> ();


        //        var ass = GetAssemblies();
        //        if ( ass!=null && ass.Count>0)
        //        {
        //            foreach (var asm in ass)
        //            {
        //            //    var myClassType = asm.GetTypes()
        //            //.FirstOrDefault(t => t.GetCustomAttributes()
        //            //.Any(a => a.GetType().Name == "BotAssemblyAtrribute"));
        //               // if (myClassType != null)
        //                {
        //                    var type = typeof(IBot);
        //                    Type t = asm.GetTypes().First(p => type.IsAssignableFrom(p));
        //                    if ( t!=null)
        //                    {
        //                        ap.Add((IBot)t);
        //                    }

                            
        //                }
        //            }
        //        }


        //        return ap;
        //    }

        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

            public async    Task RunBots()
        {
            try
            {
                BotRunnerBootstrapper.BootStrap();
                // Bots = BotRunnerBootstrapper.GetInstances<IBot>(null);
               /* Bots = BotRunnerBootstrapper.GetBots();
                if ( Bots!=null )
                {
                    foreach( var bot in Bots)
                    {
                        bot.ConfigureHostedONBotGearRunnder().GetAwaiter();
                        bot.Start().GetAwaiter();
                    }
                }
                await Task.Delay(-1);*/

            }
            catch (Exception ex)
            {
              //  return null;
            }
        }
        public static List<Assembly> GetAssemblies()
        {
            try
            {
                List<Assembly> ap = new List<Assembly>();

                if (Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bots")) == false)
                {
                    Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        "Bots"));
                }
                var dirs = Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bots"));
                if (dirs != null)
                {
                    foreach (var d in dirs)
                    {
                        var files = Directory.GetFiles(d, "*.dll");

                        foreach (var f in files)
                        {
                            var a = GetAssemblyInfo(f);
                            if (a != null)
                            {
                                var asm = Assembly.LoadFrom(f);
                                ap.Add(asm);

                            }
                        }
                        files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
                        foreach (var f in files)
                        {
                            var a = GetAssemblyInfo(f);
                            if (a != null)
                            {
                                var asm = Assembly.LoadFrom(f);
                                ap.Add(asm);
                            }
                        }
                    }
                }


                return ap;
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<IBotInfo> GetAssembliesInfo()
        {
            try
            {
                List<IBotInfo> ap = new List<IBotInfo>();
                var dirs = Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bots"));
                if (dirs != null)
                {
                    foreach (var d in dirs)
                    {

                        var files = Directory.GetFiles(d, "*.dll");

                        foreach (var f in files)
                        {
                            var a = GetAssemblyInfo(f);
                            if (a != null)
                            {
                                ap.Add(a);
                            }
                        }
                        files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
                        foreach (var f in files)
                        {
                            var a = GetAssemblyInfo(f);
                            if (a != null)
                            {
                                ap.Add(a);
                            }
                        }
                    }
                }
                return ap;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static IBotInfo GetAssemblyInfo(string filename)
        {
            try
            {
                EmptyBotInfo ap = null;
                if (filename != null)
                {
                    var asm = Assembly.LoadFrom(filename);
                    var myClassType = asm.GetTypes()
                     .FirstOrDefault(t => t.GetCustomAttributes()
                     .Any(a => a.GetType().Name == "ExportAttribute"));


                    if (myClassType != null)
                    {
                        ap = new EmptyBotInfo();
                         
                        ap.WebSite = asm.GetCustomAttribute<ModuleInfoAssemblyWebSiteAttribute>().WebSite;
                        ap.Version = asm.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                        ap.Name = asm.GetCustomAttribute<AssemblyProductAttribute>().Product;
                        






                    }


                    //ap = (IModuleInfo)myClassType;
                }



                return ap;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
