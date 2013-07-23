using Dommy.Business.Scenarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Business.Triggers
{
    public class SpeechTrigger : ITrigger
    {
        public void Init(Business.Engine engine, IScenario scenario)
        {
            engine.SpeechListener.Subscribe(this, scenario);
        }

        public bool WithPrefix { get; set; }

        public string[] Speech { get; set; }

        public double? Confidence { get; set; }
    }
}
