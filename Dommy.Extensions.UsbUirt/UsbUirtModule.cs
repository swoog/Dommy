using Dommy.Business;
using Dommy.Business.Services;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Extensions.UsbUirt
{
    public class UsbUirtModule : NinjectModule
    {
        public override void Load()
        {
            this.Kernel.Bind<ILearner>().ToMethod(c => Client<ILearner>.Create().Channel);
            this.Kernel.Bind<ITransmitter>().ToMethod(c => Client<ITransmitter>.Create().Channel);
            this.Kernel.Bind<IReceiver>().ToMethod(c => Client<IReceiver>.Create().Channel);

            this.Kernel.Bind<UsbUirtListener>().ToSelf().InSingletonScope();
            this.Kernel.Bind<IListener>().ToMethod(c=>c.Kernel.Get<UsbUirtListener>()).InSingletonScope();
            this.Kernel.Bind<IServiceHost>().ToMethod(c => new ServiceHost<UsbUirtListener>(c.Kernel.Get<UsbUirtListener>()));
        }
    }
}
