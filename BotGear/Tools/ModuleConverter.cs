using BotGear.Data.Models;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Tools
{
   public  class ModuleConverter
    {

        public BotGearUser IUserToBotGearUser(IUser iuser)
        {
            try
            {
                BotGearUser btuset = null;

                if (iuser!=null)
                {
                    btuset = new BotGearUser();
                    btuset.AvatarId = iuser.AvatarId;
                    btuset.Discriminator = iuser.Discriminator;
                    btuset.DiscriminatorValue = iuser.DiscriminatorValue;
                    btuset.AvatarUrl = iuser.GetAvatarUrl();
                    btuset.Username = iuser.Username;
                    btuset.Id = iuser.Id;
                    btuset.CreatedAt = iuser.CreatedAt.DateTime;
                    
                }


                return btuset;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;
            }
        }
       


    }
}
