using Dommy.Business.Syntax;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Triggers
{
    public class StartupTriggerSyntax : IStartupTriggerSyntax
    {
        private IKernel kernel;
        private ITriggerSyntax scenario;

        public StartupTriggerSyntax(IKernel kernel, ITriggerSyntax scenario)
        {
            this.kernel = kernel;
            this.scenario = scenario;
        }

        public ITriggerScenarioSyntax StartupTrigger()
        {
            var t = this.kernel.Get<StartupTrigger>();
            return this.scenario.Trigger(t);
        }
    }
}
