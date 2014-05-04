
namespace Dommy.Business.Triggers
{
    public class StartupTrigger : ITrigger
    {
        public void Init(Engine currentEngine, Scenarios.IScenario currentScenario)
        {
            currentScenario.Run();
        }
    }
}
