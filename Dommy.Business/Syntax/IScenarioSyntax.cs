using Dommy.Business;
using Dommy.Business.Actions;
using Dommy.Business.Scenarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Business.Syntax
{
    public interface IScenarioSyntax: IIfActions, IPingActions, ITVActions, ISayActions, IUrlActions, IEedomusActions, ISynologyDownloadActions
    {
        /// <summary>
        /// Generic lambda action.
        /// </summary>
        /// <param name="actionToPerform">Action to execute.</param>
        /// <returns>Scenario syntax.</returns>
        IScenarioSyntax Action(Func<bool> actionToPerform);

        //IScenarioSyntax Action(Func<ScenarioContext, bool> action);

        IScenario ToScenario();

        void Start();

        /// <summary>
        /// Send infra red code on USB-UIRT device.
        /// </summary>
        /// <param name="infraRedCode">infra red ode to send.</param>
        /// <returns>Scenario syntax.</returns>
        IScenarioSyntax UsbUirt(string infraRedCode);

        IScenarioSyntax Precision(string[] sentences, string[] response, Func<string, IScenarioSyntax, IScenarioSyntax> scenario);

        /// <summary>
        /// Execute an external process.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="parameters">Parameters of command.</param>
        /// <returns>Scenario syntax.</returns>
        IScenarioSyntax Command(string fileName, string parameters);

        IScenarioSyntax Log(string format, object data);

        void AddChild(IScenario scenario);
    }
}
