
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace LogService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        
        [OperationContract]
        void sendLog(CompositeLog lg);
        
    }
    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeLog
    {

        [DataMember]
        public string user { get; set; }
        [DataMember]
        public string machine { get; set; }
        [DataMember]
        public string machineIP { get; set; }
        [DataMember]
        public DateTime logDate { get; set; }
      
    }
}
