
namespace Dommy.Business.Syntax
{
    using Ninject;

    public interface ITriggerScenarioSyntax : ITriggerSyntax, IScenarioSyntax
    {
        IKernel Kernel { get; }
    }
}
