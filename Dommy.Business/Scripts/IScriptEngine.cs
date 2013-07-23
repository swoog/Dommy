using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Business.Scripts
{
    public interface IScriptEngine
    {
        string FilePattern { get; }

        void Execute(string file);
    }
}
