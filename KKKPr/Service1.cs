using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace KKKPr
{
    public partial class Service1 : ServiceBase
    {
        ConnectionChecker chk ;
        
        Machine mch = new Machine();
        UserInfo user = new UserInfo();
        private static Logger logger = LogManager.GetCurrentClassLogger(); //logger for developer
        public Service1()
        {
            
            InitializeComponent();
        }
        
        protected override void OnStart(string[] args)
        {
            logger.Trace("Service started");
            /*MainPro? */
            
            chk = new ConnectionChecker();
            chk.networkChangedDetected += Chk_networkChangedDetected;
            logger.Trace("Connection checker created.");

        }

        private void Chk_networkChangedDetected()
        {
            logger.Trace("Delegate with handler called");
            //if is connected, send Log here with logsender.

            Log lg = new Log(user.UserName, mch.machineName, mch.machineIp, DateTime.Now);

            LogSender.mapComposite(lg);
            LogSender.sendLog(LogSender.mapComposite(lg));
        }

        protected override void OnStop()
        {
            chk.Dispose();
            logger.Trace("Chk disposed, service going to down.");
        }
    }
}
