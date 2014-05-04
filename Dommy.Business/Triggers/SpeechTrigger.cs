
namespace Dommy.Business.Triggers
{
    using System.Collections.Generic;
    using Dommy.Business.Scenarios;

    public class SpeechTrigger : ITrigger
    {
        public void Init(Business.Engine currentEngine, IScenario currentScenario)
        {
            currentEngine.Listener<SpeechListener>().Subscribe(this, currentScenario);
        }

        public bool WithPrefix { get; set; }

        public ICollection<string> Speech { get; set; }

        public double? Confidence { get; set; }
    }
}
