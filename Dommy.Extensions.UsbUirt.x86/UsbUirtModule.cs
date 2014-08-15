//-----------------------------------------------------------------------
// <copyright file="UsbUirtModule.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Extensions.UsbUirt.x86
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dommy.Business;
    using Dommy.Business.Services;
    using Ninject;
    using Ninject.Modules;
    public class UsbUirtModule : NinjectModule
    {
        public override void Load()
        {
            this.Kernel.Bind<IClientFactory<IReceiverCallback>>().To<ClientFactory<IReceiverCallback>>();

            this.Kernel.Bind<IServiceHost>().To<ServiceHost<DommyLearner>>();
            this.Kernel.Bind<IServiceHost>().To<ServiceHost<DommyReceiver>>();
            this.Kernel.Bind<IServiceHost>().To<ServiceHost<DommyTransmitter>>();
        }
    }
}
