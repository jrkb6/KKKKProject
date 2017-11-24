using KKKPr.ServiceReference1;
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
    public class ConnectionChecker: IDisposable
    {
    private static string mWebServiceInternet = System.Configuration.ConfigurationManager.AppSettings["WebServiceInternet"]; //webServiceInfo
    private static Logger logger = LogManager.GetCurrentClassLogger(); //logger for developer
    private const string CHECKHOST = "google.com";  //const connaction checker
     LogSender logSender = new LogSender();
    public ConnectionChecker()
    {
        logger.Trace("chk instance created");
            /*
             * set handlers from network static class
             * */
        NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(NetworkChange_NetworkAddressChanged);
        NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
       
   }
    public void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
    {
       logger.Debug("NetworkAvailabilityChanged got");
            if (e.IsAvailable)
            {
                logger.Trace("Network changed to up");
                isStillConnected(CHECKHOST);
            }
            else
            {
                logger.Trace("Network changed to down." );
            }

        }
    public void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
    {
        logger.Debug("NetworkAdressChanged got");
            NetworkInterface[] adapters = null;
            try
            {
                adapters = NetworkInterface.GetAllNetworkInterfaces();
                
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

            logger.Trace("Adapter info loaded");
            bool flagcheckConnection = false;

            foreach (NetworkInterface n in adapters)
            {
                if (n.NetworkInterfaceType.Equals(NetworkInterfaceType.Loopback))
                    continue;
                if (n.OperationalStatus == OperationalStatus.Up) //one of the interface is up set flag truıe
                {
                    logger.Trace(n.Description);
                    
                    //linq for the connected networkinterface to get ip adress
                   // var address = NetworkInterface
                   //.GetAllNetworkInterfaces()
                   //.Where(i => i.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                   //            i.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                   //.SelectMany(i => i.GetIPProperties().UnicastAddresses)
                   //.Where(a => a.Address.AddressFamily == AddressFamily.InterNetwork && !a.Address.ToString().Equals("127.0.0.1")) //if local ip ignore it
                   //.Select(a => a.Address.ToString())
                   //.FirstOrDefault();
                   // logger.Trace("One or more interface is up: {0}", address.First()); //see ip adress on log

                    flagcheckConnection = true; //set connection flag true.
                    break;  //no need to iterate all interfaces.
                }
                else {
                    logger.Trace("CONNECTION DOWN"); //see ip adress on log
                    flagcheckConnection = false;
                }
            }

            if (flagcheckConnection)
            { //if connected then check
                logger.Trace("Connection found checking is still connected.");
                isStillConnected(CHECKHOST);
            }
            else {
                logger.Trace("No connection found.");
            }
        }
    bool isStillConnected(string host) {
            logger.Trace("Checking connection in isStillConnected");
            if (checkConnectionwithPing(host))
            {
                
                Log log = new Log(getUserName(), Environment.MachineName, Dns.GetHostAddresses(Environment.MachineName)[1].ToString(), DateTime.Now);
                log.logDate = DateTime.Now;

                logger.Trace("Log created with: " + log.user + " " + log.machine);
                
                try
                {
                    logger.Trace("Trying to send Log");
                    logger.Trace(" Map to wcf object");
                    //map object to send to WCF service
                    CompositeLog cmpLog = new CompositeLog();
                    cmpLog.machine = log.machine;
                    cmpLog.machineIP = log.machineIP;
                    cmpLog.logDate = log.logDate;
                    cmpLog.user = log.user;
                    
                    logger.Trace(" Created succesfully: " + log.user.Equals(cmpLog.user).ToString());
                    logSender.sendLog(cmpLog);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Failure to send ");
                }
                
            }
            else {
                logger.Trace("No connection found for logging.");
            }
            
        return false;
    }
    public bool checkConnectionwithPing(string host)
    {
        try
        {
            logger.Trace("Trying to ping google");
            Ping myPing = new Ping();
            byte[] buffer = new byte[32];
            int timeout = 1000;
            PingOptions pingOptions = new PingOptions();
            PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
            return (reply.Status == IPStatus.Success);
        }
        catch (Exception ex)
        {
               logger.Trace(ex, "Failure to send ping ");
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

        public void Dispose()
        {
            logger.Debug("Connection checker dispose");
            NetworkChange.NetworkAddressChanged -= new NetworkAddressChangedEventHandler(NetworkChange_NetworkAddressChanged);
            NetworkChange.NetworkAvailabilityChanged -= new NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);
        }
        
    }

}
