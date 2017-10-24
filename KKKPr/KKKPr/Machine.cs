using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Machine(string MachineIp, string MachineName) {
            this.machineIp = machineIp;
            this.machineName = MachineName;
        }

       
    }
}
