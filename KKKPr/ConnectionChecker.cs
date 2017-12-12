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

    public delegate void isConnectionChanged();
    public event isConnectionChanged networkChangedDetected;

    
    public ConnectionChecker()
    {
        logger.Trace("chk instance created");
            /*
             * set handlers from network static class
             * */
        NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(NetworkChange_NetworkAddressChanged);
        NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
       
   }
    private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
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
    private void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
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
            //bool flagcheckConnection = false;

            foreach (NetworkInterface n in adapters)
            {
                if (n.NetworkInterfaceType.Equals(NetworkInterfaceType.Loopback))
                    continue;
                else {
                    isStillConnected(CHECKHOST);
                }
              
            }

           
        }
    private bool isStillConnected(string host) {
            logger.Trace("Checking connection in isStillConnected");
            if (checkConnectionwithPing(host))
            {
                if (networkChangedDetected != null) {
                    networkChangedDetected();
                }
                                
            }
            else {
                logger.Trace("No connection found for logging.");
            }
            
        return false;
    }
    private bool checkConnectionwithPing(string host)
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
   
        /*
         * since we have connection with network adapters they will be disposed if connection checker is quit.
         * */
    public void Dispose()
    {
        logger.Debug("Connection checker dispose");
        NetworkChange.NetworkAddressChanged -= new NetworkAddressChangedEventHandler(NetworkChange_NetworkAddressChanged);
        NetworkChange.NetworkAvailabilityChanged -= new NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);
    }
      
    }
    
    

}
