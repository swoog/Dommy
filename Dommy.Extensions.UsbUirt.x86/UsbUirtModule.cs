
namespace Dommy.Extensions.UsbUirt.x86
{
    using Dommy.Business.Services;
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
