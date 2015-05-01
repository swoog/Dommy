//-----------------------------------------------------------------------
// <copyright file="UsbUirtListener.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Extensions.UsbUirt
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.Text;
    using System.Threading.Tasks;
    using Dommy.Business;
    using Dommy.Business.Configs;
    using Dommy.Business.Scenarios;
    using Dommy.Business.Services;
    using Ninject;
    using Ninject.Extensions.Logging;
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public sealed class UsbUirtListener : IListener, IReceiverCallback
    {
        private ILogger logger;
        private Dictionary<string, IScenario> scenarios = new Dictionary<string, IScenario>();
        private IClientFactory<IReceiver> receiverFactory;

        public UsbUirtListener(ILogger logger, IClientFactory<IReceiver> receiverFactory)
        {
            this.logger = logger;
            this.receiverFactory = receiverFactory;
        }

        public void Init(Engine currentEngine)
        {

        }

        public void Start()
        {
            try
            {
                using (var receiver = this.receiverFactory.Create())
                {
                    receiver.Channel.Start();
                }
            }
            catch (Exception ex)
            {
                this.logger.Warn("USB-UIRT Not found.");
                this.logger.Warn("USB-UIRT Listener is off.");
                this.logger.Error(ex, "USB-UIRT error");
            }
        }

        public void Subscribe(string irCode, IScenario scenario)
        {
            scenarios.Add(irCode, scenario);
        }

        public void Stop()
        {
            using(var receiver = this.receiverFactory.Create())
            {
                receiver.Channel.Stop();
            }
        }

        public void Receive(string infraredCode)
        {
            this.logger.Debug("Received IrCode : {0}", infraredCode);

            if (scenarios.ContainsKey(infraredCode))
            {
                scenarios[infraredCode].RunAsync();
            }
        }
    }
}
