﻿using Dommy.Business.Triggers;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Syntax
{
    public interface ITriggerSyntax  : ISpeechTriggerSyntax, IRestTriggerSyntax, ITimeTriggerSyntax, ITileTriggerSyntax, IStartupTriggerSyntax
    {
        ITriggerScenarioSyntax NoTrigger();

        ITriggerScenarioSyntax Trigger(ITrigger instanceTrigger);
    }
}
