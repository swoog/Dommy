using Dommy.Business.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Syntax
{
    public interface ITriggerSyntax  : ISpeechTriggerSyntax, IRestTriggerSyntax, IUsbUirtTriggerSyntax, ITimeTriggerSyntax, ITileTriggerSyntax
    {
        ITriggerScenarioSyntax NoTrigger();
        ITriggerScenarioSyntax Trigger(ITrigger trigger);
    }
}
