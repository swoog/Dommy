using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UsbUirt;
using UsbUirt.Enums;

namespace Dommy.Extensions.UsbUirt.x86
{
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
