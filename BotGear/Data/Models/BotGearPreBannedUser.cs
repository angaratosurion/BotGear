using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Data.Models
{
   public  class BotGearPreBannedUser
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string ServerId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string Reason{ get; set; }

    }
}
