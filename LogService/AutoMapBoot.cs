using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Web;
using System.ServiceModel.Channels;
using AutoMapper;


namespace LogService
{
    public class AutoMapBoot
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static void InitializeMap()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<CompositeLog, Logs>()
             .ReverseMap()
             );
            logger.Debug("Mapper initializer called. ");
        }
    }
    public sealed class AutomapServiceBehavior : Attribute, IServiceBehavior
    {
       
    
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
            AutoMapBoot.InitializeMap();
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {

        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {

        }
    }
}
