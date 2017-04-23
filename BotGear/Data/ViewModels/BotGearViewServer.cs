using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Audio;
using System.ComponentModel.DataAnnotations;

namespace BotGear.Data.ViewModels
{
    public class BotGearViewServer
    {
        [Required]
        public string Name { get; set; }

        
      
        [Required]
        public string OwnerId { get; set; }


        public DateTime CreatedAt { get; set; }

        [Required]
        [Key]
        public string Id { get; set; }


    }
}
