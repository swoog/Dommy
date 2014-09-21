
namespace Dommy.Extensions.UsbUirt.x86
{
    using System.ServiceModel;
    using global::UsbUirt;
    using global::UsbUirt.Enums;

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class DommyTransmitter : ITransmitter
    {
        public void Transmit(string infraRedCode)
        {
            using (var driver = new Driver())
            {
                using (var transmitter = new Transmitter(driver))
                {
                    transmitter.Transmit(infraRedCode, emitter: Emitter.Internal);
                }
            }
        }
    }
}
