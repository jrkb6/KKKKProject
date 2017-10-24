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
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public User user { get; set; }
        public Machine machine { get; set; }
        public DateTime logDate { get; set; }

        public Log(User user, Machine machine, DateTime logDate)
        {
            this.user = user;
            this.machine = machine;
            this.logDate = logDate;

        }
    }
}
