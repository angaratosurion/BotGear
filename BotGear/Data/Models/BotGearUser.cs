using Discord;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Data.Models
{
    public class BotGearUser//, IMentionable//, IPresence
    {
        /// <summary> Gets the id of this user's avatar. </summary>
        //public string AvatarId { get; set; }
        ///// <summary> Gets the url to this user's avatar. </summary>
        //public string AvatarUrl { get; set; }
        /// <summary> Gets the per-username unique id for this user. </summary>
        public string Discriminator { get; set; }
        /// <summary> Gets the per-username unique id for this user. </summary>
        public ushort DiscriminatorValue { get; set; }
        ///// <summary> Returns true if this user is a bot user. </summary>
        //public bool IsBot { get; set; }
        ///// <summary> Returns true if this user is a webhook user. </summary>
        //public bool IsWebhook { get; set; }
        ///// <summary> Gets the username for this user. </summary>


        [Required]
        [Key]
        public string Username { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime RegisteredAt { get; set; }
        [Required]
        public string Id { get; set; }
        [Required]
      //  [Key]
       // public int uid { get; set; }
        public DateTime  Birthday{ get; set; }

        
    }
}
