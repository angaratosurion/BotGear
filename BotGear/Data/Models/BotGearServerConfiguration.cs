using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Data.Models
{
   public  class BotGearServerConfiguration
    {
        [Required]
        [Key]
        public string ServerId { get; set; }
        public Boolean Notify_everyon_rulesChange { get; set; }

        public string rules_channel_name { get; set; }
        public string rules { get; set; }

        public string welcome_channel_name { get; set; }

        public string allow_channels_name { get; set; }
        public string allow_channels_mentions { get; set; }
        public string welcome_message { get; set; }
    }
}
