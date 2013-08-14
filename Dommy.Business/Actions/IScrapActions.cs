using Dommy.Business.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Business.Actions
{
    public interface IScrapActions
    {
        IScrapActions Scrap(string path, string property);

        IScenarioSyntax EndScrap();
    }
}
