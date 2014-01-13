using Dommy.Business.Scenarios;
using Ninject.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsbUirt;

namespace Dommy.Business
{
    public class UsbUirtListener : IListener
    {
        private ILogger logger;

        public UsbUirtListener(ILogger logger)
        {
            this.logger = logger;
        }

        private Dictionary<string, IScenario> scenarios = new Dictionary<string, IScenario>();

        private Driver driver = null;
        private Receiver receiver = null;

        public void Init(Engine engine)
        {
        
        }

        public void Start()
        {
            try
            {
                driver = new Driver();
                receiver = new UsbUirt.Receiver(driver);
                receiver.Received += t_Received;
            }
            catch
            {
                this.logger.Warn("USB-UIRT Not found.");
                this.logger.Warn("USB-UIRT Listener is off.");
            }
        }

        public void t_Received(object sender, UsbUirt.EventArgs.ReceivedEventArgs e)
        {
            this.logger.Debug("Received IrCode : {0}", e.IRCode);

            if (scenarios.ContainsKey(e.IRCode))
            {
                scenarios[e.IRCode].RunAsync();
            }
        }

        public void Subscribe(string irCode, IScenario scenario)
        {
            scenarios.Add(irCode, scenario);
        }

        public void Stop()
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
}
