using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KKKPr
{
    
    public class MainPro
    {
        public MainPro() {
            UserInfo gt = new UserInfo();
            LogSender logSender = new LogSender();
            ConnectionChecker chk = new ConnectionChecker();
            chk.networkChangedDetected += _isConnected;

        } 
        public static void _isConnected() {
            //if is connected, send Log here with logsender.
        }
    }
       
    
}
