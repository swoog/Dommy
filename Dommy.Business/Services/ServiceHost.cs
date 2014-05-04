//-----------------------------------------------------------------------
// <copyright file="ServiceHost.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Services
{
    using System;
    using System.Linq;
    using System.ServiceModel;

    /// <summary>
    /// Create a WCF host for a service.
    /// </summary>
    /// <typeparam name="T">Type of the service.</typeparam>
    public sealed class ServiceHost<T> : IServiceHost, IDisposable
    {
        /// <summary>
        /// Instance of the service.
        /// </summary>
        private T service;

        /// <summary>
        /// Instance of the service host.
        /// </summary>
        private ServiceHost host;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHost{T}"/> class.
        /// </summary>
        /// <param name="service">Instance of the service.</param>
        public ServiceHost(T service)
        {
            this.service = service;
        }

        /// <summary>
        /// Open the service.
        /// </summary>
        public void Open()
        {
             var contract = typeof(T)
                .GetInterfaces()
                .Where(t => t.GetCustomAttributes(true)
                             .Cast<ServiceContractAttribute>()
                             .Any()).FirstOrDefault();

            host = new ServiceHost(service, new Uri[] { new Uri("net.pipe://localhost/dommy/" + contract.Name) });
            //host.Description.Behaviors.Add(new ServiceMetadataBehavior() { HttpGetEnabled = true });



            host.AddServiceEndpoint(contract, new NetNamedPipeBinding(), "");
            //host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

            this.host.Open();
        }

        /// <summary>
        /// Close the service.
        /// </summary>
        public void Close()
        {
            this.host.Close();
        }

        /// <summary>
        /// Dispose the service.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the service.
        /// </summary>
        /// <param name="disposing">Indicate disposing managed object.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.host != null)
                {
                    ((IDisposable)this.host).Dispose();
                }
            }
        }
    }
}
