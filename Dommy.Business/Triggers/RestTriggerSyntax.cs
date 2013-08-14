using Ninject;
using Dommy.Business.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Triggers
{
    public class RestTriggerSyntax : Dommy.Business.Triggers.IRestTriggerSyntax
    {
        private IKernel kernel;
        private ITriggerSyntax scenario;

        public RestTriggerSyntax(IKernel kernel, ITriggerSyntax scenario)
        {
            this.kernel = kernel;
            this.scenario = scenario;
        }

        public ITriggerScenarioSyntax RestTrigger(string p)
        {
            var t = this.kernel.Get<RestTrigger>();
            t.Url = p;
            return this.scenario.Trigger(t);
        }

        public ITriggerScenarioSyntax RestTrigger(string p, object data)
        {
            var t = this.kernel.Get<RestTrigger>();
            t.Url = p;
            t.Data = data;
            return this.scenario.Trigger(t);
        }
    }
}
