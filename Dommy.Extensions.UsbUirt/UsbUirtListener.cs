using Dommy.Business;
using Dommy.Business.Scenarios;
using Ninject;
using Ninject.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dommy.Business.Configs;
using Dommy.Business.Services;
using System.ServiceModel;

namespace Dommy.Extensions.UsbUirt
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public sealed class UsbUirtListener : IListener, IReceiverCallback
    {
        private ILogger logger;
        private Dictionary<string, IScenario> scenarios = new Dictionary<string, IScenario>();
        private IReceiver receiver;

        public UsbUirtListener(ILogger logger, IReceiver receiver)
        {
            this.logger = logger;
            this.receiver = receiver;
        }

        public void Init(Engine currentEngine)
        {

        }

        public void Start()
        {
            try
            {
                this.receiver.Start();
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
            this.receiver.Stop();
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
