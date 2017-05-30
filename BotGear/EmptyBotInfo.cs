using BotGear.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotGear
{
    public class EmptyBotInfo : IBotInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string WebSite { get; set; }
        public string OathUrl { get; set; }
    }
}
