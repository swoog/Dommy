using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Triggers
{
    public class StartupTrigger : ITrigger
    {
        public void Init(Engine engine, Scenarios.IScenario scenario)
        {
            scenario.Run();
        }
    }
}
