using BotGear.Attributes.Assembly;
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
   public  class BotGearCore
    {

        public static IEnumerable<IModuleInfo> GetAllModulesInfo()
        {
            List<IModuleInfo> ap = new List<IModuleInfo>();

            ap = GetAssembliesInfo();

            /* foreach (var inf in ModuleInfos)
             {
                 ap.Add(inf.Value);
             }*/

            return ap;
        }

        public static List<Assembly> GetAssemblies()
        {
            try
            {
                List<Assembly> ap = new List<Assembly>();
                //string tmp = Assembly.GetCallingAssembly().Location;

                if (Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins")) == false)
                {
                    Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        "plugins"));
                }
                var files = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins"), "*.dll");

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

                return ap;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static List<Assembly> GetAssemblies(string botname)
        {
            try
            {
                List<Assembly> ap = new List<Assembly>();
                string plgpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bots", botname, "plugins");
                string botpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bots", botname);

                if (Directory.Exists(plgpath) == false)
                {
                    Directory.CreateDirectory(plgpath);
                }
                var files = Directory.GetFiles(plgpath, "*.dll");

                foreach (var f in files)
                {
                    var a = GetAssemblyInfo(f);
                    if (a != null)
                    {
                        var asm = Assembly.LoadFrom(f);
                        ap.Add(asm);

                    }
                }
                files = Directory.GetFiles(botpath, "*.dll");
                foreach (var f in files)
                {
                    var a = GetAssemblyInfo(f);
                    if (a != null)
                    {
                        var asm = Assembly.LoadFrom(f);
                        ap.Add(asm);
                    }
                }

                return ap;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static List<IModuleInfo> GetAssembliesInfo()
        {
            try
            {
                List<IModuleInfo> ap = new List<IModuleInfo>();


                var files = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins"), "*.dll");

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

                return ap;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static IModuleInfo GetAssemblyInfo(string filename)
        {
            try
            {
                EmptyModuleInfo ap = null;
                if (filename != null)
                {
                    var asm = Assembly.LoadFrom(filename);
                    var myClassType = asm.GetTypes()
                     .FirstOrDefault(t => t.GetCustomAttributes()
                     .Any(a => a.GetType().Name == "ExportAttribute"));


                    if (myClassType != null)
                    {
                        ap = new EmptyModuleInfo();
                        object[] c = asm.GetCustomAttributes(typeof(ModuleInfoAssemblySourceCodeAttribute)).ToArray();
                        //.GetCustomAttributes(typeof(ModuleInfoAssemblySourceCodeAttribute)).ToArray();
                        ap.SourceCode = ((ModuleInfoAssemblySourceCodeAttribute)c[0]).SourceCode;
                        ap.WebSite = asm.GetCustomAttribute<ModuleInfoAssemblyWebSiteAttribute>().WebSite;
                        ap.Version = asm.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                        ap.Name = asm.GetCustomAttribute<AssemblyProductAttribute>().Product;
                        ap.Description = asm.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
                        ap.IsPluginBot = asm.GetCustomAttribute<BotPluginAttribute>().IsPluginBot;




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
