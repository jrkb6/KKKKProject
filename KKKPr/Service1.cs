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
        ConnectionChecker chk;
        private static Logger logger = LogManager.GetCurrentClassLogger(); //logger for developer
        public Service1()
        {
            
            InitializeComponent();
        }
        
        protected override void OnStart(string[] args)
        {
            logger.Trace("Service started");
            chk = new ConnectionChecker();
            logger.Trace("Connection checker created.");

        }
        protected override void OnStop()
        {
            chk.Dispose();
            logger.Trace("Chk disposed, service going to down.");
        }
    }
}
