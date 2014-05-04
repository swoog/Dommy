using Dommy.Business;
using Dommy.Business.Services;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Extensions.UsbUirt.x86
{
    public class UsbUirtModule : NinjectModule
    {
        public override void Load()
        {
            this.Kernel.Bind<IReceiverCallback>().ToMethod(c => Client<IReceiverCallback>.Create().Channel);

            this.Kernel.Bind<IServiceHost>().To<ServiceHost<DommyLearner>>();
            this.Kernel.Bind<IServiceHost>().To<ServiceHost<DommyReceiver>>();
            this.Kernel.Bind<IServiceHost>().To<ServiceHost<DommyTransmitter>>();
        }
    }
}
