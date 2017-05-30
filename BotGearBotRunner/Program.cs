using BotGear;
using BotGear.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BotGearBotRunner
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        public static void Main(string[] args)
        {
            var handle = GetConsoleWindow();

#if DEBUG
          ShowWindow(handle, SW_SHOW);
#else
            ShowWindow(handle, SW_HIDE);
#endif
            var ass = BotGearBotRunnerCore.GetAssemblies();
            if (ass != null && ass.Count > 0)
            {
                foreach (var asm in ass)
                {
                    //    var myClassType = asm.GetTypes()
                    //.FirstOrDefault(t => t.GetCustomAttributes()
                    //.Any(a => a.GetType().Name == "BotAssemblyAtrribute"));
                    // if (myClassType != null)
                    {
                        var type = typeof(IBot);
                        Type t = asm.GetTypes().First(p => type.IsAssignableFrom(p));
                        if (t != null)
                        {
                           _MethodInfo   mi= t.GetMethod("Start");
                            object o= asm.CreateInstance(t.Name);
                            mi.Invoke(o, null);
                                
                         }


                    }
                }
            }
            while (true)
            {
                Task.Delay(-1);
            }

        }
    }
}
