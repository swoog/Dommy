
namespace Dommy.Business.Triggers
{
    using Dommy.Business.Scenarios;

    public interface ITrigger
    {
        void Init(Engine currentEngine, IScenario currentScenario);
    }
}
