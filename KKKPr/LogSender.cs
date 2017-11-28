using KKKPr.ServiceReference1;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            try {
                logger.Debug("Trying to connect with WCF service.");
                client.Open();
                client.sendLog(lg);
                logger.Trace("Log sended with success");
                client.Close();
                logger.Trace("WCF connection closed");
            } catch (Exception ex) {
                logger.Error("Error: " + ex.ToString());
                client.Abort(); //delete everything in the connection
            }         
        }
        public Log createLog(string username)
        {
            Log log = null;
            try
            {
                log = new Log(username, Environment.MachineName, Dns.GetHostAddresses(Environment.MachineName)[1].ToString(), DateTime.Now);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Problem at createLog");
            }
            return log;

        }
        public CompositeLog mapComposite(Log log)
        {
            CompositeLog cmpLog = new CompositeLog();
            cmpLog.machine = log.machine;
            cmpLog.machineIP = log.machineIP;
            cmpLog.logDate = log.logDate;
            cmpLog.user = log.user;
            return cmpLog;
        }

    }

}

}
