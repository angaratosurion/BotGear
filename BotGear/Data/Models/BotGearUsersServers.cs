using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Data.Models
{
   public class BotGearUsersServers
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public string  UserId { get; set; }
        [Required]
        public string  ServerId { get; set; }
        
    }
}
