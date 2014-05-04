
namespace Dommy.Business.Triggers
{
    using System;
    using Dommy.Business.Syntax;
    using Ninject;

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
