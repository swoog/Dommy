using Ninject;
using Dommy.Business.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Triggers
{
    public class UsbUirtTriggerSyntax : Dommy.Business.Triggers.IUsbUirtTriggerSyntax
    {
        private IKernel kernel;
        private ITriggerSyntax scenario;

        public UsbUirtTriggerSyntax(IKernel kernel, ITriggerSyntax scenario)
        {
            this.kernel = kernel;
            this.scenario = scenario;
        }

        public ITriggerScenarioSyntax UsbUirtTrigger(string infraRedCode)
        {
            var t = this.kernel.Get<UsbUirtTrigger>();
            t.Code = infraRedCode;
            return this.scenario.Trigger(t);
        }
    }
}
