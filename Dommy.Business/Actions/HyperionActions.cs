namespace Dommy.Business.Actions
{
    using System.Net.Http;

    using Dommy.Business.Scenarios;
    using Dommy.Business.Syntax;
    using Dommy.Business.Tools;

    public class HyperionActions : IHyperionActions
    {
        private IScenarioSyntax scenario;

        private HyperionHelper hyperionHelper;

        public HyperionActions(IScenarioSyntax scenario, HyperionHelper hyperionHelper)
        {
            this.scenario = scenario;
            this.hyperionHelper = hyperionHelper;
        }

        public IScenarioSyntax HyperionEffect(string effectName, int duration = -1)
        {
            return this.scenario.Action(() =>
                {
                    this.hyperionHelper.Effect(effectName, duration);

                return true;
            });
        }
    }
}