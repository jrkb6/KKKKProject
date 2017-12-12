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
        public static void sendLog(CompositeLog lg)
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
        public static Log createLog(string username,string machineName,string ipAdress)
        {
            Log log = null;
            try
            {
                log = new Log(username, machineName,ipAdress, DateTime.Now);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Problem at createLog");
            }
            return log;

        }
        public static CompositeLog mapComposite(Log log)
        {
            CompositeLog cmpLog = new CompositeLog();
            cmpLog.machine = log.machine;
            cmpLog.machineIP = log.machineIP;
            cmpLog.logDate = log.logDate;
            cmpLog.user = log.user;
            return cmpLog;
        }
        //public static void send(string username, string machineName, string ipAdress) {
        //    sendLog( mapComposite(createLog(username, machineName, ipAdress)));

        //}

    }

}


