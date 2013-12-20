﻿using Dommy.Business.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Triggers
{
    public interface IStartupTriggerSyntax : IExtendSyntax
    {
        ITriggerScenarioSyntax StartupTrigger();
    }
}