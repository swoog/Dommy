using Dommy.Business.Scenarios;
using Dommy.Business.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Dommy.Business.Triggers
{
    public class UsbUirtTrigger : ITrigger
    {
        public string Code { get; set; }

        public void Init(Engine currentEngine, IScenario currentScenario)
        {
            currentEngine.Listener<UsbUirtListener>().Subscribe(this.Code, currentScenario);
        }
    }
}
