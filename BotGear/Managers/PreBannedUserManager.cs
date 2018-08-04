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
    public class PreBannedUserManager
    {
        static BotGearContext db = new BotGearContext();
        ModuleConverter conv = new ModuleConverter();
        ServerManager srvMngr = new ServerManager();
        UserManager userMngr = new UserManager();
        public async Task banUser(string  iuser, string reason , IGuild iguild)
        {
            try
            {

                if (iuser != null && reason != null && iguild != null)
                {

                    BotGearUser user = await userMngr.GetUserbyId(iuser);
                    BotGearPreBannedUser  exuser = await this.GetPreBannedUserbyId(user.Id);
                    BotGearServer srv = this.conv.IGuildToBotGearServer(iguild);

                    if (user != null && exuser == null && srv != null)
                    {
                        if (await srvMngr.ServerExists(srv.Id) != true)
                        {
                            await this.srvMngr.addServer(iguild);
                        }
                        else
                        {
                            string id = srv.Id;
                            srv = await srvMngr.getServerbyId(id);
                        }
                        exuser = new BotGearPreBannedUser();
                        exuser.ServerId = srv.Id;
                        exuser.UserId = user.Id;
                        exuser.Date = DateTime.Now;
                        exuser.Reason = reason;
                        db.PreBannedUsers.Add(exuser);

                        db.SaveChanges();                        
                    }
                    else if (user != null && exuser != null && srv != null)
                    {
                        if (exuser.UserId==user.Id && exuser.ServerId==srv.Id)
                        {
                           // await this.srvMngr.addServer(iguild);
                        }
                       
                    }


                }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

            }
        }
        public async Task<BotGearPreBannedUser> GetPreBannedUserbyId(string id)
        {
            try
            {
                BotGearPreBannedUser user = null;
                if (id != null)
                {
                    user = db.PreBannedUsers.FirstOrDefault(x => x.UserId== id);


                }
                return user;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;

            }
        }
        public List<BotGearPreBannedUser> GetPreBannedUsers()
        {
            try
            {
                List<BotGearPreBannedUser> users = null;

                users = db.PreBannedUsers.ToList();



                return users;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;

            }
        }
        public async Task UnBanUser(string  iuser, IGuild iguild)
        {
            try
            {

                if (iuser != null)
                {
                    BotGearPreBannedUser user = await this.GetPreBannedUserbyId(iuser );
                    var srvid =   Convert.ToString(iguild.Id);
                    if (user != null &&  user.ServerId==srvid)
                    {

                       
                        db.PreBannedUsers.Remove(user);

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
