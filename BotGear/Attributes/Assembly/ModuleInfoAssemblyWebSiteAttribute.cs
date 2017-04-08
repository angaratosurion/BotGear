using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Attributes.Assembly
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ModuleInfoAssemblyWebSiteAttribute : Attribute
    {
        public ModuleInfoAssemblyWebSiteAttribute(string website)
        { this.WebSite = website; }
       

        public string WebSite { get; set; }

        
    }
}
