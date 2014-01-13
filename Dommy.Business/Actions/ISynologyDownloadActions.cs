using Dommy.Business.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Business.Actions
{
    public interface ISynologyDownloadActions : IExtendSyntax
    {
        IScenarioSyntax SynologyDownloadCreate(string server, string username, string password, string file);

        IScenarioSyntax SynologyDownloadCreate(string server, string username, string password, object data, string file);
    }
}
