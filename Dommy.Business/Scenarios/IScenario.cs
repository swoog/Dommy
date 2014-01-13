using Dommy.Business.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Scenarios
{
    public interface IScenario
    {
        IList<ITrigger> Triggers { get; set; }

        void Init(Engine engine);

        void Run();

        Task RunAsync();

        string ScenarioName { get; }
    }
}
