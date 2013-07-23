using Dommy.Business.Scenarios;
using Dommy.Business.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Dommy.Business.Triggers
{
    public class RestTrigger : ITrigger
    {
        public string Url { get; set; }

        public void Init(Engine engine, IScenario scenario)
        {
            engine.RestListener.Subscribe(this.Url, scenario);
        }
    }
}
