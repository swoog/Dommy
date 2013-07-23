using Dommy.Business;
using Dommy.Business.Scenarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Business.Triggers
{
    public interface ITrigger
    {
        void Init(Engine engine, IScenario scenario);
    }
}
