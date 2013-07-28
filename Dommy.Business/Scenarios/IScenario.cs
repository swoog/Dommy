using Dommy.Business.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Business.Scenarios
{
    public interface IScenario
    {
        IList<ITrigger> Triggers { get; set; }

        void Init(Engine engine);

        void Run();

        string ScenarioName { get; }
    }
}
