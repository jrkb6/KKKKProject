using AutoMapper;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace LogService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

       
        public void sendLog(CompositeLog lg)
        {
            try {
                using (LogDatabaseEntities2 db = new LogDatabaseEntities2())
                {
                    Logs log;
                    log = Mapper.Map<Logs>(lg);
                    db.Logs.Add(log);
                    db.SaveChanges();

                }
            } catch(Exception ex) {
                logger.Error("Sent failure:" + ex.ToString() );
            } finally {
                
            }
        }
        
    }
}
