using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::UsbUirt;
using UsbUirt.EventArgs;
using System.ServiceModel;

namespace Dommy.Extensions.UsbUirt.x86
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class DommyReceiver : IReceiver, IDisposable
    {
        private Driver driver = null;
        private Receiver receiver = null;
        private IReceiverCallback receiverCallback;

        public DommyReceiver(IReceiverCallback receiverCallback)
        {
            this.receiverCallback = receiverCallback;
        }

        public void Start()
        {
            driver = new Driver();
            receiver = new Receiver(driver);
            receiver.Received += InfraRedReceived;
        }

        public void InfraRedReceived(object sender, ReceivedEventArgs e)
        {
            this.receiverCallback.Receive(e.IRCode);
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.receiver != null)
                {
                    this.receiver.Dispose();
                }

                if (this.driver != null)
                {
                    this.driver.Dispose();
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Stop()
        {
            this.Dispose();
        }
    }
}
