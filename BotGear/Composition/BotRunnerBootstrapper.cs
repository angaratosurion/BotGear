using BotGear.Interfaces;
using BotGear.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Composition
{
    public class BotRunnerBootstrapper
    {
        private static CompositionContainer CompositionContainer;
        private static bool IsLoaded = false;
        //   static CommonTools cmTools = new CommonTools();
        static AggregateCatalog Catlgs;
      
        [ImportMany]
        private static IEnumerable<Lazy<IModuleInfo>> ModuleInfos;
        [ImportMany]
        private static IEnumerable<Lazy<IBot>> Bots = null;
        public static void BootStrap()
        {
            try
            {
                var pluginFolders = new List<string>();

                var plugins = Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "Bots")).ToList();


                plugins.ForEach(s =>
                {
                    var di = new DirectoryInfo(s);
                    pluginFolders.Add(di.Name);
                });


              
                    BotRunnerBootstrapper.Compose(pluginFolders);
                
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
            }
        }

        public static void Compose(List<string> pluginFolders)
        {
            try
            {

                if (IsLoaded) return;

                var catalog = new AggregateCatalog();

              catalog.Catalogs.Add(new DirectoryCatalog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory)));

                foreach (var plugin in pluginFolders)
                {
                    var directoryCatalog = new DirectoryCatalog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        "Bots", plugin));
                     catalog.Catalogs.Add(directoryCatalog);

                }
                CompositionContainer = new CompositionContainer(catalog);
                
                CompositionContainer.ComposeParts();
                 ModuleInfos = CompositionContainer.GetExports<IModuleInfo>();
                Bots= CompositionContainer.GetExports<IBot>();

                Catlgs = catalog;
                IsLoaded = true;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
    
        public static T GetInstance<T>(string contractName = null)
        {
            try
            {

                var type = default(T);

                if (CompositionContainer == null) return type;

                if (!string.IsNullOrWhiteSpace(contractName))
                {
                    type = CompositionContainer.GetExportedValue<T>(contractName);
                }
                else
                {
                    type = CompositionContainer.GetExportedValue<T>();
                }


                return type;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return default(T);
            }
        }
        public static List<T> GetInstances<T>(string contractName = null)
        {
            try
            {

                List<T> type = null;

                if (CompositionContainer == null) return type;

                if (!string.IsNullOrWhiteSpace(contractName))
                {
                    type = CompositionContainer.GetExportedValues<T>(contractName).ToList();
                }
                else
                {
                    type = CompositionContainer.GetExportedValues<T>().ToList();
                }


                return type;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
        public static List<IBot> GetBots()
        {
            try
            {
                List<IBot> ap = null;
                if ( Bots!=null)
                {
                    ap = new List<IBot>();
                    foreach(var bot in Bots)
                    {
                        ap.Add((IBot)bot.Value);
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
