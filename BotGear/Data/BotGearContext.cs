using BotGear.Data.Models;
using System;
using System.Data.Entity;
using System.IO;

namespace BotGear.Data
{
    public class BotGearContext : DbContext
    {
        public BotGearContext()
            : base("DefaultConnection")
        {
            try
            {
            //    this.Configuration.AutoDetectChangesEnabled = true;
            //    //this.Configuration.LazyLoadingEnabled = true;
            //    //this.Configuration.ValidateOnSaveEnabled = false;

                string path = AppDomain.CurrentDomain.BaseDirectory;

                //System.IO.Directory.GetCurrentDirectory();
                AppDomain.CurrentDomain.SetData("DataDirectory", path);
                this.Configuration.AutoDetectChangesEnabled = true;
              
            }
            catch(StackOverflowException ex)
            {

            }

        }
      
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

             
            modelBuilder.Properties<DateTime?>()
                  .Configure(c => c.HasColumnType("datetime2"));
            
            base.OnModelCreating(modelBuilder);

        }
        public static BotGearContext Create()
        {
            return new BotGearContext();
        }

        public IDbSet<BotGearUser> Users { get; set; }
        public IDbSet<BotGearServer> Servers { get; set; }
        public IDbSet<BotGearUsersServers> UsersServers { get; set; }
        public IDbSet<BotGearServerConfiguration> ServerConfiguration { get; set; }
    }
}
