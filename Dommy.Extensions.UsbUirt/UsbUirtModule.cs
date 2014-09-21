
namespace Dommy.Extensions.UsbUirt
{
    using Dommy.Business;
    using Dommy.Business.Services;
    using Ninject;
    using Ninject.Modules;

    public class UsbUirtModule : NinjectModule
    {
        public override void Load()
        {
            this.Kernel.Bind<IClientFactory<ILearner>>().To<ClientFactory<ILearner>>();
            this.Kernel.Bind<IClientFactory<ITransmitter>>().To<ClientFactory<ITransmitter>>();
            this.Kernel.Bind<IClientFactory<IReceiver>>().To<ClientFactory<IReceiver>>();


            this.Kernel.Bind<UsbUirtListener>().ToSelf().InSingletonScope();
            this.Kernel.Bind<IListener>().ToMethod(c => c.Kernel.Get<UsbUirtListener>()).InSingletonScope();
            this.Kernel.Bind<IServiceHost>().ToMethod(c => new ServiceHost<UsbUirtListener>(c.Kernel.Get<UsbUirtListener>()));
        }
    }
}
