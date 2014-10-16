namespace Dommy.Business.Actions
{
    using Dommy.Business.Syntax;

    public interface IHyperionActions : IExtendSyntax
    {
        IScenarioSyntax HyperionEffect(string effectName, int duration = -1);
    }
}
