using Dommy.Business.Syntax;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Triggers
{
    public class TimeTriggerSyntax : ITimeTriggerSyntax
    {
        private IKernel kernel;
        private ITriggerSyntax scenario;

        public TimeTriggerSyntax(IKernel kernel, ITriggerSyntax scenario)
        {
            this.kernel = kernel;
            this.scenario = scenario;
        }

        public ITriggerScenarioSyntax TimeTrigger(DateTime startDate, TimeSpan tick)
        {
            var t = this.kernel.Get<TimeTrigger>();
            t.StartDate = startDate;
            t.Tick = tick;
            return this.scenario.Trigger(t);

        }
    }
}
