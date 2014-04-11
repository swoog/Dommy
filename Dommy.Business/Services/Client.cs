//-----------------------------------------------------------------------
// <copyright file="Client.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Description;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Create client channel for the WCF contract.
    /// </summary>
    /// <typeparam name="T">WCF contract.</typeparam>
    public sealed class Client<T> : IDisposable
    {
        /// <summary>
        /// Channel factory for WCF contract.
        /// </summary>
        private static ChannelFactory<T> channelFactory = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client{T}"/> class.
        /// </summary>
        /// <param name="channel">WCF channel.</param>
        private Client(T channel)
        {
            this.Channel = channel;
        }

        /// <summary>
        /// Gets instance of WCF channel.
        /// </summary>
        /// <returns></returns>
        public T Channel { get; private set; }

        /// <summary>
        /// Create a client of the <see cref="T"/> contract.
        /// </summary>
        /// <returns>Client channel.</returns>
        public static Client<T> Create()
        {
            if (channelFactory == null)
            {
                var contract = ContractDescription.GetContract(typeof(T));

                var endPoint = new EndpointAddress("net.pipe://localhost/dommy/" + typeof(T).Name.Substring(1));

                channelFactory = new ChannelFactory<T>(new ServiceEndpoint(contract, new NetNamedPipeBinding(), endPoint));
            }

            return new Client<T>(channelFactory.CreateChannel());
        }

        /// <summary>
        /// Dispose the channel.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the channel.
        /// </summary>
        /// <param name="disposing">Indicate disposing managed object.</param>
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.Channel != null)
                {
                    ((IDisposable)this.Channel).Dispose();
                }
            }
        }
    }
}
