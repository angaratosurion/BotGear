using BotGear.Data.Models;
using BotGear.Managers;
using BotGear.Tools;
using Discord;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Data.ViewModels
{
    public class BotGearViewUser
    {

        ServerManager srvMngr = new ServerManager();
        public string Discriminator { get; set; }
        
        public ushort DiscriminatorValue { get; set; }
      
       // [Required]
       
        public string Username { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime RegisteredAt { get; set; }
       // [Required]
       // [Key]
        public string Id { get; set; }
      
        public DateTime  Birthday{ get; set; }

       // [Required]
        public List <BotGearServer> Servers { get; set; }
        public async void ImportFromModel (BotGearUser model)
        {
            try
            {

                if (model != null)
                {
                    this.Id = model.Id;
                    this.Birthday = model.Birthday;
                    this.CreatedAt = model.CreatedAt;
                    this.Discriminator = model.Discriminator;
                    this.DiscriminatorValue = model.DiscriminatorValue;
                    this.RegisteredAt = model.RegisteredAt;
                    this.Username = model.Username;
                    this.Servers = await this.srvMngr.getServerbymeMeberId(this.Id);

                }

            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
            }
        }
        public BotGearUser ExportTomodel()
        {
            try
            {
                BotGearUser ap = new BotGearUser();

                ap.Id = this.Id;
                ap.Birthday = this.Birthday;
                ap.CreatedAt = this.CreatedAt;
                ap.Discriminator = this.Discriminator;
                ap.DiscriminatorValue = this.DiscriminatorValue;
                ap.RegisteredAt = this.RegisteredAt;
                ap.Username = this.Username;

                return ap;
            }
            catch (Exception ex)
            {
                CommonTools.ErrorReporting(ex);
                return null;
            }
        }


    }
}
