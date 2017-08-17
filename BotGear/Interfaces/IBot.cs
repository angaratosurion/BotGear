using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Interfaces
{
    public interface IBot
    {
     
        Task Configure();
         Task Start();
          Task Log(LogMessage arg);
        Task Stop();
        

    }
}
