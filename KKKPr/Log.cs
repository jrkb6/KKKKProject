using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KKKPr
{
    public class Log
    {
        
        public string user { get; set; }
        public string machine { get; set; }
        public string machineIP { get; set; }
        public DateTime logDate { get; set; }

        public Log(string user, string machine, string machineIP,DateTime logDate)
        {
            this.user = user;
            this.machine = machine;
            this.logDate = logDate;
            this.machineIP = machineIP;

        }
    }
}
