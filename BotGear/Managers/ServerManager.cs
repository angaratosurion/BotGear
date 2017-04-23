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
    public class ServerManager
    {
        static BotGearContext db = new BotGearContext();
        ModuleConverter conv = new ModuleConverter();
       static UserManager usrmngr = new UserManager();
        public async Task addServer( IGuild iguild)
        {
            try
            {
                BotGearServer srv =  conv.IGuildToBotGearServer(iguild);
                if (srv!=null && iguild != null && await this.ServerExists(srv.Id)==false)
                {

                    db.Servers.Add(srv);
                    db.SaveChanges();

                    


                }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

            }
        }
        public  async Task<BotGearServer> getServerbyId(string id)
        {
            try
            {
                BotGearServer ap = null;
                if (id != null)
                {

                    ap = db.Servers.FirstOrDefault(x => x.Id == id);




                }

                return ap;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;

            }
        }
        public async Task<List<BotGearServer>> getServerbymeMeberId(string memid)
        {
            try
            {
                List < BotGearServer> ap = null;
                if (memid != null)
                {

                    var servusr = db.UsersServers.ToList().FindAll(x => x.UserId == memid).ToList();
                    if (servusr !=null)
                    {
                        ap = new List<BotGearServer>();
                        foreach( var s in servusr)
                        {
                            BotGearServer serv = await this.getServerbyId(s.ServerId);
                             if (serv !=null)
                            {
                                ap.Add(serv);
                            }
                        }
                    }




                }

                return ap;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;

            }
        }
        public async Task  deleteServerbyId(string id)
        {
            try
            {
                
                if (id != null && await  this.ServerExists(id)==true)
                {

                    BotGearServer srv = await this.getServerbyId(id);
                     if ( srv !=null)
                    {
                        db.Servers.Remove(srv);
                        db.SaveChanges();
                    }




                }

               

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                 

            }
        }
        public async Task<Boolean> ServerExists(string id)
        {
            try
            {
                Boolean ap = false;
                if (id != null)
                {


                    BotGearServer serv = await this.getServerbyId(id);
                    if ( serv !=null)
                    {
                        ap = true;
                    }



                }

                return ap;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false; ;

            }
        }
        public async Task EditServerInfo(string id ,IGuild iguild)
        {
            try
            {
                BotGearServer srv = conv.IGuildToBotGearServer(iguild);
                if (srv != null && iguild != null && await this.ServerExists(id) == false)
                {
                    BotGearServer srv0 =  await this.getServerbyId(id);
                    if (srv0 != null)
                    {
                       db.Entry(srv0).CurrentValues.SetValues(srv);

                        db.SaveChanges();
                    }




                }

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);

            }
        }
        public async Task<List<BotGearUser>> getServerMembersbyServerId(string id)
        {
            try
            {
                List<BotGearUser> ap = null;
                if (id != null && await this.ServerExists(id)==true)
                {

                    BotGearServer srv = await this.getServerbyId(id);

                    List<BotGearUsersServers> usrvs = db.UsersServers.ToList().FindAll(x => x.ServerId == id);
                    if ( usrvs !=null)
                    {
                        ap = new List<BotGearUser>();
                         foreach(var u in usrvs)
                        {

                            BotGearUser user = await usrmngr.GetUserbyId(u.UserId);
                             if ( user!=null)
                            {
                                ap.Add(user);
                            }
                        }
                    }

                    




                }

                return ap;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;

            }
        }
        public async Task<Boolean>IsMembersToServer(string sid,string uid)
        {
            try
            {
                List<BotGearUser> users = await this.getServerMembersbyServerId(sid);
                BotGearUser user = await usrmngr.GetUserbyId(uid);
                Boolean ap = false;
              
                if (sid != null && await this.ServerExists(sid) == true && users !=null && uid!=null && user!=null)
                {
 
                       var tuser = db.UsersServers.FirstOrDefault(x => x.ServerId == sid && x.UserId == uid);
                    if ( tuser!=null)
                    {
                        ap = true;
                    }






                }

                return ap;

            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return false;

            }
        }
        public async Task AddMemberToServer(IGuild iguild, IUser iuser)
        {
            try
            {
                var tuser = conv.IUserToBotGearUser(iuser);
                var tser = conv.IGuildToBotGearServer(iguild);
                BotGearUser user = await usrmngr.GetUserbyId(tuser.Id);
                
                if (tser != null && await this.ServerExists(tser.Id) == true && await  IsMembersToServer(tser.Id,user.Id)==false && user != null)
                {
                    string  sid = tser.Id;
                    tser = await this.getServerbyId(sid);
                    BotGearUsersServers usserv = new BotGearUsersServers();
                    usserv.ServerId = tser.Id;
                    usserv.UserId = user.Id;
                    db.UsersServers.Add(usserv);
                   await  db.SaveChangesAsync();




                }



            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
               

            }
        }
        public async Task<BotGearUser> GetAMemberToServer(IGuild iguild, IUser iuser)
        {
            try
            {
                var tuser = conv.IUserToBotGearUser(iuser);
                var tser = conv.IGuildToBotGearServer(iguild);
                BotGearUser user = await usrmngr.GetUserbyId(tuser.Id);
                BotGearUser ap = null;

                if (tser != null && await this.ServerExists(tser.Id) == true && await IsMembersToServer(tser.Id, user.Id) == false && user == null)
                {
                    string sid = tser.Id;
                    tser = await this.getServerbyId(sid);
                    var t = db.UsersServers.FirstOrDefault(x => x.ServerId == sid && x.UserId ==  user.Id);
                     if ( t!=null)
                    {
                        ap = await usrmngr.GetUserbyId(t.UserId);
                    }




                }
                return ap;


            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;


            }
        }
        public async Task deleteMemberfromServer(IGuild iguild, IUser iuser)
        {
            try
            {
                var tuser = conv.IUserToBotGearUser(iuser);
                var tser = conv.IGuildToBotGearServer(iguild);
                BotGearUser user = await usrmngr.GetUserbyId(tuser.Id);

                if (tser != null && await this.ServerExists(tser.Id) == true && await IsMembersToServer(tser.Id, user.Id) == false && user != null)
                {
                    string sid = tser.Id;
                    tser = await this.getServerbyId(sid);
                    var suser = db.UsersServers.FirstOrDefault(x => x.ServerId == sid && x.UserId ==user.Id);
                    if (tuser != null)
                    {
                        db.UsersServers.Remove(suser);
                        await db.SaveChangesAsync();
                    }




                }



            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);


            }
        }
        public async Task deleteMemberfromServer(string sid , IUser iuser)
        {
            try
            {
                var tuser = conv.IUserToBotGearUser(iuser);
               
                BotGearUser user = await usrmngr.GetUserbyId(tuser.Id);

                if ( await this.ServerExists(sid) == true && await IsMembersToServer(sid, user.Id) == false && user != null)
                {
                   
                    var suser = db.UsersServers.FirstOrDefault(x => x.ServerId == sid && x.UserId == user.Id);
                    if (tuser != null)
                    {
                        db.UsersServers.Remove(suser);
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
