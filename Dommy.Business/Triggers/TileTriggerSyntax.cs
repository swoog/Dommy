using Dommy.Business.Syntax;
using Ninject;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Triggers
{
    public class TileTriggerSyntax : ITileTriggerSyntax
    {
        private IKernel kernel;
        private ITriggerSyntax scenario;

        public TileTriggerSyntax(IKernel kernel, ITriggerSyntax scenario)
        {
            this.kernel = kernel;
            this.scenario = scenario;
        }

        public ITriggerScenarioSyntax TileTrigger(string title, Color backGround)
        {
            var t = this.kernel.Get<TileTrigger>();
            t.Title = title;
            t.BackgroundColor = backGround;
            return this.scenario.Trigger(t);
        }
    }
}
