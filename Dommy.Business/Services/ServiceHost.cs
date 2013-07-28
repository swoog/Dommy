using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Services
{
    public class ServiceHost<T> : IServiceHost
    {
        private T service;

        public ServiceHost(T service)
        {
            this.service = service;
        }

        private ServiceHost host;

        public void Open()
        {
            host = new ServiceHost(service, new Uri[] { new Uri("net.pipe://localhost/dommy/" + typeof(T).Name) });
            //host.Description.Behaviors.Add(new ServiceMetadataBehavior() { HttpGetEnabled = true });

            host.AddServiceEndpoint(typeof(T).GetInterface("I" + typeof(T).Name), new NetNamedPipeBinding(), "");
            //host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

            host.Open();
        }

        public void Close()
        {
            host.Close();
        }
    }
}
