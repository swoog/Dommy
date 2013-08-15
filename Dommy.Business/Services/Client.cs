﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Services
{
    public class Client<T> : IDisposable
    {
        private static ChannelFactory<T> channelFactory = null;

        public T Channel { get; private set; }

        private Client(T channel)
        {
            this.Channel = channel;
        }

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

        public void Dispose()
        {
            ((IDisposable)this.Channel).Dispose();
        }
    }
}