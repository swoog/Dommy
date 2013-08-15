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

        public object Data { get; set; }

        public void Init(Engine engine, IScenario scenario)
        {
            engine.Listener<RestListener>().Subscribe(this.Url, this.Data, scenario);
        }
    }
}
