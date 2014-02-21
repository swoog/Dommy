using Dommy.Business.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Actions
{
    public interface ITileActions : IExtendSyntax
    {
        IScenarioSyntax TileUpdate(Tile tile);
    }
}