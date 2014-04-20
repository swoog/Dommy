using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Triggers
{
    public class StartupTrigger : ITrigger
    {
        public void Init(Engine currentEngine, Scenarios.IScenario currentScenario)
        {
            currentScenario.Run();
        }
    }
}
