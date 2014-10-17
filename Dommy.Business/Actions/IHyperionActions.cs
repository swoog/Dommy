namespace Dommy.Business.Actions
{
    using System.Drawing;

    using Dommy.Business.Syntax;

    public interface IHyperionActions : IExtendSyntax
    {
        IScenarioSyntax HyperionEffect(string effectName, int? duration = null);
        IScenarioSyntax HyperionColor(Color color, int? duration = null);
        IScenarioSyntax HyperionClear();
    }
}
