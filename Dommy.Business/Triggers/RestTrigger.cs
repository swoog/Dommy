
namespace Dommy.Business.Triggers
{
    using Dommy.Business.Scenarios;

    public class RestTrigger : ITrigger
    {
        public string Url { get; set; }

        public object Data { get; set; }

        public void Init(Engine currentEngine, IScenario currentScenario)
        {
            currentEngine.Listener<RestListener>().Subscribe(this.Url, this.Data, currentScenario);
        }
    }
}
