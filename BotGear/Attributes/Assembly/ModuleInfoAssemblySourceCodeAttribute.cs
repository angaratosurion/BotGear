using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Attributes.Assembly
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ModuleInfoAssemblySourceCodeAttribute:Attribute
    {
        public ModuleInfoAssemblySourceCodeAttribute(string sourcecode)
        {
            SourceCode = sourcecode;

        }
        public string SourceCode { get; set; }
    }
}
