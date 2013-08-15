using Dommy.Business;
using Dommy.Business.Actions;
using Dommy.Business.Result;
using Dommy.Business.Scenarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Business.Syntax
{
    public interface IScenarioSyntax: IIfActions, IPingActions, ITvActions, ISayActions, IUrlActions, IEedomusActions
    {
        IScenarioSyntax Action(Func<bool> action);

        //IScenarioSyntax Action(Func<ScenarioContext, bool> action);

        IScenario ToScenario();

        void Start();

        IScenarioSyntax UsbUirt(string irCode);

        IScenarioSyntax Precision(string[] sentences, string[] response, Func<string, IScenarioSyntax, IScenarioSyntax> scenario);

        IScenarioSyntax Command(string command, string parameters);

        IScenarioSyntax Log(string format, object obj);

        void AddChild(IScenario scenario);
    }
}
