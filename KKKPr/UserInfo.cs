using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace KKKPr
{
    public class UserInfo
    {
        private static Logger logger = LogManager.GetCurrentClassLogger(); //logger for developer
        public static string getUserName()
        {
            string userName = String.Empty;
            string Domain = String.Empty;
            string OwnerSID = String.Empty;
            string processname = String.Empty;
            try
            {
                logger.Trace("Trying to get username with explorer");
                ObjectQuery sq = new ObjectQuery("Select * from Win32_Process where Name='explorer.exe'"); //("Select * from Win32_Process Where ProcessID = '" + PID + "'");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(sq);
                ManagementObject op = new ManagementObject();
                if (searcher.Get().Count == 0)
                    return null;
                int i = 0;
                foreach (ManagementObject oReturn in searcher.Get())
                {
                    if (i == 1)
                    {
                        break;
                    }
                    string[] o = new String[2];
                    //Invoke the method and populate the o var with the user name and domain
                    oReturn.InvokeMethod("GetOwner", (object[])o);

                    //int pid = (int)oReturn["ProcessID"];
                    processname = (string)oReturn["Name"];
                    //dr[2] = oReturn["Description"];
                    userName = o[0];
                    if (userName == null)
                        userName = String.Empty;
                    i++;
                    return userName;
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Failed to get userName");
                return null;
            }

        }
    }
}
