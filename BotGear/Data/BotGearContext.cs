using BotGear.Data.Models;
using System;
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

            string path = AppDomain.CurrentDomain.BaseDirectory;

            //System.IO.Directory.GetCurrentDirectory();
            AppDomain.CurrentDomain.SetData("DataDirectory", path);
            this.Configuration.AutoDetectChangesEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            //modelBuilder.Properties<DateTime>()
            //      .Configure(c => c.HasColumnType("datetime2"));

        }
            public static BotGearContext Create()
        {
            return new BotGearContext();
        }

       public IDbSet<BotGearUser> User { get; set; }
    }
}
