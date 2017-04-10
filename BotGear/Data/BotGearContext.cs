using BotGear.Data.Models;
using System.Data.Entity;

namespace BotGear.Data
{
    public class BotGearContext :DbContext
    {
        public BotGearContext()
            : base("DefaultConnection")
        {
            //this.Configuration.AutoDetectChangesEnabled = true;
            //this.Configuration.LazyLoadingEnabled = true;
            //this.Configuration.ValidateOnSaveEnabled = false;
        }
        public static BotGearContext Create()
        {
            return new BotGearContext();
        }

       public IDbSet<BotGearUser> User { get; set; }
    }
}
