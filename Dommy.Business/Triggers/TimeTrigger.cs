using Dommy.Business.Scenarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Triggers
{
    public class TimeTrigger : ITrigger
    {
        public DateTime StartDate { get; set; }

        public TimeSpan Tick { get; set; }

        System.Threading.Timer timer;

        private IScenario scenario;

        public void Init(Engine engine, Scenarios.IScenario scenario)
        {
            this.scenario = scenario;
            timer = new System.Threading.Timer(CallBack, null, TimeSpan.FromSeconds(0), this.Tick);
        }

        private void CallBack(object state)
        {
            this.scenario.Run();
        }
    }
}
