using KKKPr.ServiceReference1;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KKKPr
{
    public class LogSender
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public void sendLog(CompositeLog lg)
        {
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            client.Open();
            try { } catch (Exception ex) { } finally { }
            client.Close();
           
        }

        public void setWCFConnection() {
           
        }



    }

}
