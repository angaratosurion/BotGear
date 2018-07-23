using Discord.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Instrumentation;
using Microsoft.Win32;
using System.Management;
using System.IO;
using System.Reflection;
using BotGear.Tools;

namespace SystemCommands
{
    [Export(typeof(ModuleBase))]    
    public class Commands : ModuleBase
    {
        [Command("Specs")]
        [Summary("get Server Specifications")]
        public async Task ServerSpecs()
        {

            try
            {
                string specs = null;

                StringBuilder bld = new StringBuilder();
                bld.AppendLine("==Bot is hosted on a Server/Pc with the Specs==");
                RegistryKey processor_name = Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor\0", RegistryKeyPermissionCheck.ReadSubTree);   //This registry entry contains entry for processor info.

                if (processor_name.GetValue("ProcessorNameString") != null)
                {
                    string cpuinf = processor_name.GetValue("ProcessorNameString").ToString();
                    bld.AppendLine(String.Format(" CPU : " + cpuinf));

                }
                ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
                foreach (ManagementObject managementObject in mos.Get())
                {
                    if (managementObject["TotalVirtualMemorySize"] != null)
                    {
                        bld.AppendLine(String.Format("Total VirtualMemory Size   :  " + managementObject["TotalVirtualMemorySize"].ToString()));     //Display operating system version.
                    }
                    if (managementObject["TotalVisibleMemorySize"] != null)
                    {
                        bld.AppendLine(String.Format("Total VisibleMemory Size   :  " + managementObject["TotalVisibleMemorySize"].ToString()));     //Display operating system version.
                    }
                    if (managementObject["TotalSwapSpaceSize"] != null)
                    {
                        bld.AppendLine(String.Format("Total SwapSpace Size  :  " + managementObject["TotalSwapSpaceSize"].ToString()));     //Display operating system version.
                    }
                    if (managementObject["Caption"] != null)
                    {
                        bld.AppendLine(String.Format("Operating System Name  :  " + managementObject["Caption"].ToString()));   //Display operating system caption
                    }
                    if (managementObject["OSArchitecture"] != null)
                    {
                        bld.AppendLine(String.Format("Operating System Architecture  :  " + managementObject["OSArchitecture"].ToString()));   //Display operating system architecture.
                    }
                    if (managementObject["CSDVersion"] != null)
                    {
                        bld.AppendLine(String.Format("Operating System Service Pack   :  " + managementObject["CSDVersion"].ToString()));     //Display operating system version.
                    }
                    if (managementObject["OtherTypeDescription"] != null)
                    {
                        bld.AppendLine(String.Format("Operating System Type   :  " + managementObject["OtherTypeDescription"].ToString()));     //Display operating system version.
                    }



                }
            ;
                specs = bld.ToString();

                await ReplyAsync(specs);
            }
            catch(Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }

        }
        [Command("geterrorlog")]
        [Summary(" Get Error Log")]
        [RequireOwner]
        public async Task GetErrorLog()
        {

            try
            { 
            string logdir = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Logs");
            string logfile = Path.Combine(logdir, "errorLog.txt");
            var owner = Context.Client.GetApplicationInfoAsync().Result.Owner;
          //  var user = Context.User;

                //if (user == owner)
                {
                    if (Directory.Exists(logdir) == true && File.Exists(logfile) == true)
                    {


                        Stream fil = File.OpenRead(logfile);
                        await Context.Channel.SendFileAsync(fil, logfile, "Exceptions wich have been thrown");
                    }
                    else
                    {
                        await ReplyAsync("There if no error  log file");
                    }
                }
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        [Command("delete_errorlog")]
        [Summary("Delete Error Log")]
        [RequireOwner]
        public async Task DelErrorLog()
        {

            try
            {
                string logdir = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Logs");
                string logfile = Path.Combine(logdir, "errorLog.txt");
                var owner = Context.Client.GetApplicationInfoAsync().Result.Owner;
                //var user = Context.User;

                //if (user == owner)
                {
                    if (Directory.Exists(logdir) == true && File.Exists(logfile) == true)
                    {


                        File.Delete(logdir);
                        await ReplyAsync("There   error  log file deleted");
                    }
                    else
                    {
                        await ReplyAsync("There is no error  log file");
                    }
                }
            }

            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }

    [Command("clear_logs")]
        [Summary("Clears  Log files")]
        [RequireOwner]
        public async Task ClrErrorLog()
        {

            try
            {
                string logdir = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Logs");
                string logfile = Path.Combine(logdir, "errorLog.txt");
                var owner = Context.Client.GetApplicationInfoAsync().Result.Owner;
                var user = Context.User;

                //if (user == owner)
                {
                    if (Directory.Exists(logdir) == true)
                    {

                        foreach (var v in Directory.GetFiles(logdir))
                        {
                            File.Delete(v);
                        }
                        await ReplyAsync("There   error  log file deleted");
                    }
                    else
                    {
                        await ReplyAsync("There is no error  log file");
                    }
                }
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
    }
}
