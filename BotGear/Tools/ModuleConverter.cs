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
                   // btuset.AvatarId = iuser.AvatarId;
                    btuset.Discriminator = iuser.Discriminator;
                    btuset.DiscriminatorValue = iuser.DiscriminatorValue;
                   // btuset.AvatarUrl = iuser.GetAvatarUrl();
                  
                    btuset.Username = iuser.Username;
                    btuset.Id = Convert.ToString(iuser.Id);
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
        public BotGearServer IGuildToBotGearServer(IGuild iguild)
        {
            try
            {
                BotGearServer btuset = null;

                if (iguild != null)
                {
                    btuset = new BotGearServer();
                    // btuset.AvatarId = iuser.AvatarId;
                    btuset.CreatedAt = iguild.CreatedAt.DateTime;
                    btuset.Id = Convert.ToString(iguild.Id);
                    btuset.OwnerId = Convert.ToString(iguild.OwnerId);
                    btuset.Name = iguild.Name;
                    

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
