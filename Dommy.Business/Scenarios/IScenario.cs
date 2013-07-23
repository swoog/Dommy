using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Business.Scenarios
{
    public interface IScenario
    {
        Triggers.ITrigger Trigger { get; set; }

        void Init(Engine engine);

        void Run();

        string ScenarioName { get; }
    }
}
