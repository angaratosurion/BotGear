using Discord;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Data.Models
{
    public class BotGearUser
    { 
        public string Discriminator { get; set; }
        
        public ushort DiscriminatorValue { get; set; }
      
        [Required]
      
        public string Username { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime RegisteredAt { get; set; }
        [Required]
        [Key]
        public string Id { get; set; }
        [Required]
     
        public DateTime  Birthday{ get; set; }

       

        
    }
}
