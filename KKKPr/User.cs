using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KKKPr
{
    public class User
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public string username { get; set; }

        public User(string username) {
            this.username = username;
        }
    }
}
