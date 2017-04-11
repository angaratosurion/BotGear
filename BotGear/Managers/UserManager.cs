using BotGear.Data;
using BotGear.Data.Models;
using BotGear.Tools;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Managers
{
    public class UserManager
    {
        BotGearContext db = new BotGearContext();
        ModuleConverter conv = new ModuleConverter();
        public async Task addUser(IUser iuser , DateTime birthday)
        {
            try
            {

                if ( iuser !=null && birthday !=null)
                {
                    BotGearUser user = conv.IUserToBotGearUser(iuser);
                    if ( user!=null && this.GetUserbyId(user.Id)==null)
                    {
                        user.Birthday = birthday;
                        db.Users.Add(user);
                        await db.SaveChangesAsync();

                    }

                }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
               
            }
        }
        public async Task<BotGearUser> GetUserbyId(ulong id)
        {
            try
            {
                BotGearUser user = null;
                if (id >0)
                {
                     user =  db.Users.FirstOrDefault(x => x.Id == id);
                   

                }
                return user;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;

            }
        }
        public async Task EditUser(IUser iuser, DateTime birthday)
        {
            try
            {

                if (iuser != null && birthday != null)
                {
                    BotGearUser tuser = conv.IUserToBotGearUser(iuser);
                    BotGearUser user = await this.GetUserbyId(tuser.Id);
                    if (user != null )
                    {
                        user.Birthday = birthday;
                        db.Entry(user).CurrentValues.SetValues(user);
                        
                        await db.SaveChangesAsync();

                    }

                }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

            }
        }
        public async Task EditUser(IUser iuser, IUser iuser2)
        {
            try
            {

                if (iuser != null && iuser2!=null )
                {
                    BotGearUser tuser = conv.IUserToBotGearUser(iuser);
                    BotGearUser tuser2 = conv.IUserToBotGearUser(iuser2);
                    BotGearUser user = await this.GetUserbyId(tuser.Id);
                    if (user != null)
                    {
                        tuser2.uid = user.uid;
                        tuser2.Birthday = user.Birthday;
                        db.Entry(user).CurrentValues.SetValues(tuser2);

                        await db.SaveChangesAsync();

                    }

                }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

            }
        }
        public async Task DeleteUser(IUser iuser)
        {
            try
            {

                if (iuser != null )
                {
                   
                    BotGearUser user = await this.GetUserbyId(iuser.Id);
                    if (user != null)
                    {

                        db.Users.Remove(user);

                        await db.SaveChangesAsync();

                    }

                }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

            }
        }

    }
}
