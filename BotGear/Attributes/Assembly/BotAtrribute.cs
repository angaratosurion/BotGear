using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Attributes.Assembly
{
    [AttributeUsage(AttributeTargets.Assembly)]
    
    public class BotAtrribute :  Attribute
    {
        public BotAtrribute(Boolean arg)
        {
            isBot = arg;
        }
        public Boolean isBot { get; set; }
    }
}
