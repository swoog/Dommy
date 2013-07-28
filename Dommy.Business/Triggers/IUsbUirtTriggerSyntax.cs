using Dommy.Business.Syntax;
using System;
namespace Dommy.Business.Triggers
{
    public interface IUsbUirtTriggerSyntax : IExtendSyntax
    {
        ITriggerScenarioSyntax UsbUirtTrigger(string irCode);
    }
}
