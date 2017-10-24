using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KKKPr
{
    public class ConnectionChecker
    {
    private static string mWebServiceInternet = System.Configuration.ConfigurationManager.AppSettings["WebServiceInternet"];
    private static Logger logger = LogManager.GetCurrentClassLogger();
    private const string CHECKHOST = "google.com";
    Log lg;
    public ConnectionChecker()
    {
        NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(NetworkChange_NetworkAddressChanged);
        NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);
    }
    public void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
    {
       logger.Debug("NetworkChanged");
        if (e.IsAvailable)
        {
            isStillConnected(CHECKHOST);
        }
    }
    public void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
    {
        logger.Debug("Network Adress changed.");
        NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
        bool flagcheckConnection = false;

        foreach (NetworkInterface n in adapters)
        {
            if (n.OperationalStatus == OperationalStatus.Up)
            {
                //NEED TO FIX
                  var address = NetworkInterface
                 .GetAllNetworkInterfaces()
                 .Where(i => i.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                             i.NetworkInterfaceType == NetworkInterfaceType.Ethernet )
                 .SelectMany(i => i.GetIPProperties().UnicastAddresses)
                 .Where(a => a.Address.AddressFamily == AddressFamily.InterNetwork && !a.Address.ToString().Equals("127.0.0.1"))
                 .Select(a => a.Address.ToString())
                 .ToList();
                    logger.Trace("One or more interface is up: {0}", address.First());
                    
                    flagcheckConnection = true;
                break;
            }
        }

        if (flagcheckConnection)
           isStillConnected(CHECKHOST);
    }
    bool isStillConnected(string host) {
       if (checkConnectionwithPing(host))
        {
                //Create and send Log.
                User usr = new User(getUserName());
                Machine mch = new Machine(Environment.MachineName, Dns.GetHostAddresses(Environment.MachineName)[1].ToString());
                Log log = new Log(usr,mch,DateTime.Now);
                log.logDate = DateTime.Now;

                logger.Trace("Log created with: " + log.user.username +" " +log.machine.machineIp);
                //send log 
                try {
                    logger.Trace("Trying to send Log");
                    LogSender.sendLog(log);
                } catch (Exception ex) {
                    logger.Debug("Failure to send " + ex.ToString());
                } finally {
                }
            }
        return false;
    }
    public bool checkConnectionwithPing(string host)
    {
        try
        {
            Ping myPing = new Ping();
            byte[] buffer = new byte[32];
            int timeout = 1000;
            PingOptions pingOptions = new PingOptions();
            PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
            return (reply.Status == IPStatus.Success);
        }
        catch (Exception)
        {
            return false;
        }
    }
    public static string getUserName()
        {
            string userName = String.Empty;
            string Domain = String.Empty;
            string OwnerSID = String.Empty;
            string processname = String.Empty;
            try
            {
                logger.Debug("Trying to get username with explorer");
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
                logger.Debug("Failed to get userName: "+ ex.ToString());
                return null;
            }

        }
        /* void CheckConnectionState()
             {
                 try
                 {
                     string resInternet = CheckConnection(mWebServiceInternet);
                     if (!(resInternet == "" || resInternet.StartsWith("hata")))
                     {
                         // WriteLog("Network Changed.");
                         string[] arr = resInternet.Split('#');
                         DateTime userLogDate = DateTime.Now;
                         DateTime parsed = DateTime.Now;

                         //log olarak webservis saatini yazalim istendi
                         if (arr.Length > 0)
                         {
                             if (DateTime.TryParseExact(arr[1], "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
                                 userLogDate = parsed;
                         }
                         string strCurrentUserName = Environment.UserName;
                         strCurrentUserName = GetUserName().username;
                         if (string.IsNullOrEmpty(strCurrentUserName))
                         {
                             System.Diagnostics.Process[] objArrProcess = System.Diagnostics.Process.GetProcessesByName("explorer");

                             if (objArrProcess != null && objArrProcess.Length > 0)
                                 strCurrentUserName = objArrProcess[0].StartInfo.EnvironmentVariables["username"];
                         }
                         if (string.IsNullOrEmpty(strCurrentUserName))
                             strCurrentUserName = "SYSTEM";
                     }
                 }
                 catch (Exception ex)
                 {
                     EventLog.WriteEntry("Application", Environment.MachineName + "-" + Environment.UserName + "5 Detay:" + ex.ToString(), EventLogEntryType.Information, 6543);

                 }
                 EventLog.WriteEntry("Application", "LogActivities /CheckConnectionState1", EventLogEntryType.Information, 6543);
                 EventLog.WriteEntry("Application", "LogActivities / CheckConnectionState2", EventLogEntryType.Information, 6543);
             }*/
        /* string CheckConnection(string wsAddress)
   {
       string result = "";
       try
       {
           Thread.Sleep(1500);
           //WSInternetAccess.CheckStateSoapClient cs = new WSInternetAccess.CheckStateSoapClient();
           //cs.Endpoint.Address = new System.ServiceModel.EndpointAddress(wsAddress);
           //cs.ClientCredentials.Windows.ClientCredential = System.Net.CredentialCache.DefaultNetworkCredentials;
           string hostname = Dns.GetHostName();
           string userIPs = "";          
           for (int i = Dns.GetHostEntry(hostname).AddressList.Length; i > 0; i--)
           {
               userIPs += Dns.GetHostEntry(hostname).AddressList[i - 1].ToString() + " - ";
           }
           userIPs += Guid.NewGuid().ToString();
           string strCurrentUserName = Environment.UserName;
           strCurrentUserName = GetUserName().username;
           if (string.IsNullOrEmpty(strCurrentUserName))
           {
               System.Diagnostics.Process[] objArrProcess = System.Diagnostics.Process.GetProcessesByName("explorer");
               if (objArrProcess.Length > 0)
                   strCurrentUserName = objArrProcess[0].StartInfo.EnvironmentVariables["username"];
           }
       }
       catch (Exception ex)
       {
           result = "hata:" + ex.ToString();
           EventLog.WriteEntry("Application", Environment.MachineName + "-" + Environment.UserName + "6 Detay:" + ex.ToString(), EventLogEntryType.Information, 6543);
       }
       EventLog.WriteEntry("Application", result + "xxxxx");
       return result;
   }*/
    }

}
