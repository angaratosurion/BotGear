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

                    
                    BotGearPreBannedUser  exuser = await this.GetPreBannedUserbyIdAndServerId(iuser, Convert.ToString(iguild.Id));
                    BotGearServer srv = this.conv.IGuildToBotGearServer(iguild);

                    if (exuser == null && srv != null)
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
                        exuser.UserId = iuser;
                        exuser.Date = DateTime.Now;
                        exuser.Reason = reason;
                        db.PreBannedUsers.Add(exuser);

                        db.SaveChanges();                        
                    }
                   


                }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
               

            }
        }
        public async Task<BotGearPreBannedUser> GetPreBannedUserbyIdAndServerId(string id,string serverid)
        {
            try
            {
                BotGearPreBannedUser user = null;
                if (id != null && serverid!=null)
                {
                    user = db.PreBannedUsers.FirstOrDefault(x => x.UserId== id && x.ServerId==serverid);


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
                    BotGearPreBannedUser user = await this.GetPreBannedUserbyIdAndServerId(iuser,Convert.ToString(iguild.Id) );
                    var srvid =   Convert.ToString(iguild.Id);
                   

                       
                        db.PreBannedUsers.Remove(user);

                        await db.SaveChangesAsync();

                    

                }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

            }
        }
        public List<BotGearPreBannedUser> GetPreBannedUsers(string serverid)
        {
            try
            {
                List<BotGearPreBannedUser> users = null;
                if (serverid != null)
                {
                    users = db.PreBannedUsers.Where(x=>x.ServerId==serverid).ToList();
                }



                return users;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;

            }
        }
    }
}
