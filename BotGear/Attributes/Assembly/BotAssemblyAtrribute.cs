using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Attributes.Assembly
{
    [AttributeUsage(AttributeTargets.Assembly)]
    
    public class BotAssemblyAtrribute :  Attribute
    {
        public BotAssemblyAtrribute(bool arg)
        {
            isBot = arg;
        }
        public Boolean isBot { get; set; }
    }
}
