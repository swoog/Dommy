using Dommy.Business.Syntax;
using System;
namespace Dommy.Business.Triggers
{
    public interface IRestTriggerSyntax : IExtendSyntax
    {
        IScenarioSyntax RestTrigger(string p);
    }
}
