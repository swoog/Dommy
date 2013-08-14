using Dommy.Business.Syntax;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Triggers
{
    public interface ITileTriggerSyntax : IExtendSyntax
    {
        ITriggerScenarioSyntax TileTrigger(string title, Color backGround);
    }
}
