//-----------------------------------------------------------------------
// <copyright file="ClientFactory.cs" company="TrollCorp">
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

    public class ClientFactory<T> : IClientFactory<T>
    {
        private static ChannelFactory<T> channelFactory = null;

        public Client<T> Create()
        {
            if (channelFactory == null)
            {
                var contract = ContractDescription.GetContract(typeof(T));

                var endPoint = new EndpointAddress("net.pipe://localhost/dommy/" + typeof(T).Name);

                channelFactory = new ChannelFactory<T>(new ServiceEndpoint(contract, new NetNamedPipeBinding(), endPoint));
            }

            return new Client<T>(channelFactory.CreateChannel());
        }
    }
}
