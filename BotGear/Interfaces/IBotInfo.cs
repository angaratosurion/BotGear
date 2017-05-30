using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Interfaces
{
   public  interface IBotInfo
    {
        String Name { get; set; }
        String Version { get; set; }
        String  WebSite { get; set; }
        String OathUrl { get; set; }
    }
}
