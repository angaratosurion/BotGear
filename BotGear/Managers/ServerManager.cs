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
        BotGearContext db = new BotGearContext();
        ModuleConverter conv = new ModuleConverter();
        public async Task addServer( IGuild iguild)
        {
            try
            {
                BotGearServer srv =  conv.IGuildToBotGearServer(iguild);
                if (srv!=null && iguild != null && await this.ServerExists(srv.Id)==false)
                {

                    this.db.Servers.Add(srv);
                    this.db.SaveChanges();

                    


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

                    ap = this.db.Servers.FirstOrDefault(x => x.Id == id);




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
                        this.db.Servers.Remove(srv);
                        this.db.SaveChanges();
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
        public async Task EditServer(string id ,IGuild iguild)
        {
            try
            {
                BotGearServer srv = conv.IGuildToBotGearServer(iguild);
                if (srv != null && iguild != null && await this.ServerExists(id) == false)
                {
                    BotGearServer srv0 =  await this.getServerbyId(id);
                    if (srv0 != null)
                    {
                        this.db.Entry(srv0).CurrentValues.SetValues(srv);

                        this.db.SaveChanges();
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
