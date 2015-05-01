//-----------------------------------------------------------------------
// <copyright file="DommyReceiver.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Extensions.UsbUirt.x86
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.Text;
    using System.Threading.Tasks;
    using Dommy.Business.Services;
    using global::UsbUirt;
    using global::UsbUirt.EventArgs;

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class DommyReceiver : IReceiver, IDisposable
    {
        private Driver driver = null;
        private Receiver receiver = null;
        private IClientFactory<IReceiverCallback> receiverCallback;

        public DommyReceiver(IClientFactory<IReceiverCallback> receiverCallback)
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
            using(var receiverCallback = this.receiverCallback.Create())
            {
                receiverCallback.Channel.Receive(e.IRCode);
            }
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
