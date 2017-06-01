using BotGear.Data;
using BotGear.Data.Models;
using BotGear.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotGear.Managers
{
    public class ServerConfigManager
    {
        static BotGearContext db = new BotGearContext();
        public async Task<List<BotGearServerConfiguration>> GetAllServerConfiguration()
        {
            try
            {
                var ap = db.ServerConfiguration.ToList();
                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;

            }
        }
        public async Task<bool> ServersConfigurationExists(string servetid)
        {
            try
            {
                Boolean ap = false;
                if (String.IsNullOrWhiteSpace(servetid) != true)
                {
                    var x = await this.GetServersConfigurationById(servetid);
                    if (x != null)
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
        public async Task<BotGearServerConfiguration> GetServersConfigurationById(string servetid)
        {
            try
            {
                BotGearServerConfiguration ap = null;
                if (String.IsNullOrWhiteSpace(servetid) != true)
                {
                    var x = await this.GetAllServerConfiguration();
                    ap = x.FirstOrDefault(s => s.ServerId == servetid);
                }




                return ap;
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);
                return null;

            }
        }
        public async Task AddServerConfiguration(BotGearServerConfiguration mod)
        {
            try
            {

                if (mod != null && await this.ServersConfigurationExists(mod.ServerId) == false)
                {
                    db.ServerConfiguration.Add(mod);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);


            }
        }


        public async Task DeleteServerConfiguration(string ServerId)
        {
            try
            {

                if (String.IsNullOrWhiteSpace(ServerId) != true && await this.ServersConfigurationExists(ServerId) != false)
                {
                    var x = await this.GetServersConfigurationById(ServerId);
                    db.ServerConfiguration.Remove(x);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                CommonTools.ErrorReporting(ex);


            }
        }

    }
}

