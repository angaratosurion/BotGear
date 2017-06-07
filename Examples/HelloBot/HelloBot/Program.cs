using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloBotCore;

namespace HelloBot
{
    class Program
    {
       static  HelloBotCore.HelloBot bot = new HelloBotCore.HelloBot();
        static void Main(string[] args)
        {
            bot.Configure();
            bot.Start().GetAwaiter().GetResult();
        }
    }
}
