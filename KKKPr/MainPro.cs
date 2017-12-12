using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KKKPr
{
    
    public class MainPro
    {

        private static Logger logger = LogManager.GetCurrentClassLogger(); //logger for developer
        
        ConnectionChecker chk = new ConnectionChecker();
        Machine mch = new Machine();
        UserInfo user = new UserInfo();
        public MainPro() {
            logger.Trace("Main pro created");
            
            
          
            logger.Trace("networkChangedDetected initiliazed with isConnected");

        }        
      
      
    }
       
    
}
