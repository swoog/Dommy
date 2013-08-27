using Dommy.Business.Syntax;
using System;
namespace Dommy.Business.Triggers
{
    public interface IRestTriggerSyntax : IExtendSyntax
    {
        ITriggerScenarioSyntax RestTrigger(string url);
    
        ITriggerScenarioSyntax RestTrigger(string url, object data);
    }
}
