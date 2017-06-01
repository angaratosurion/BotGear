using BotGear;
using BotGear.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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
            //var ass = BotGearBotRunnerCore.GetAssemblies();
            //if (ass != null && ass.Count > 0)
            //{
            //    foreach (var asm in ass)
            //    {
            //        //    var myClassType = asm.GetTypes()
            //        //.FirstOrDefault(t => t.GetCustomAttributes()
            //        //.Any(a => a.GetType().Name == "BotAssemblyAtrribute"));
            //        // if (myClassType != null)
            //        {
            //            var type = typeof(IBot);
            //            Type t = asm.GetTypes().First(p => type.IsAssignableFrom(p));
            //            if (t != null)
            //            {
            //                _MethodInfo mi = t.GetMethod("Start");
            //               object o= asm.CreateInstance(t.Name);


            //                //bot.Start().GetAwaiter().GetResult();
            //                object[] a = new object[1];
            //               mi.Invoke(o, a);

            //             }


            //        }
            //    }

            BotGearBotRunnerCore botgcore = new BotGearBotRunnerCore();
            botgcore.RunBots().GetAwaiter();


            bool createdNew;
            var waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, "CF2D4313-33DE-489D-9721-6AFF69841DEA", out createdNew);
            var signaled = false;

            // If the handle was already there, inform the other process to exit itself.
            // Afterwards we'll also die.
            if (!createdNew)
            {
                Log("Inform other process to stop.");
                waitHandle.Set();
                Log("Informer exited.");

                return;
            }

            // Start a another thread that does something every 10 seconds.
            var timer = new Timer(OnTimerElapsed, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

            // Wait if someone tells us to die or do every five seconds something else.
            do
            {
                signaled = waitHandle.WaitOne(TimeSpan.FromSeconds(5));
                // ToDo: Something else if desired.
            } while (!signaled);



        }
        private static void Log(string message)
        {
            Console.WriteLine(DateTime.Now + ": " + message);
        }

        private static void OnTimerElapsed(object state)
        {
            Log("Timer elapsed.");
        }
    }
}
