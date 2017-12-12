using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace KKKPr
{
    public class Machine
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public string machineIp { get; set; }
        public string machineName { get; set; }

       public Machine() {
            machineName = Environment.MachineName;
            machineIp = getIpAdress(machineName);
        }
        
        public string getIpAdress(string machineName)
        {
            string res = "127.0.0.1";
            try
            {
                res = Dns.GetHostAddresses(machineName)[1].ToString();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "DNS FAILED");
            }
            finally {

            }
            return res;
        }

       
    }
}
