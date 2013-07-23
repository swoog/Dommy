using Dommy.Business.Scenarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business
{
    public interface IActionLogger
    {
        void ExecutionOf(IScenario scenario);
    }
}
