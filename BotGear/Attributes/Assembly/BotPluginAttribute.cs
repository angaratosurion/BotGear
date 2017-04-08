using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Attributes.Assembly
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class BotPluginAttribute : Attribute
    {
        public BotPluginAttribute(Boolean arg)
        {

            IsPluginBot = arg;
        }
        public Boolean IsPluginBot { get; set; }
    }
}
