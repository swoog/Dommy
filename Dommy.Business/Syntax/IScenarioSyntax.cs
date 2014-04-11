//-----------------------------------------------------------------------
// <copyright file="IScenarioSyntax.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Syntax
{
    using System;
    using Dommy.Business.Actions;
    using Dommy.Business.Scenarios;

    /// <summary>
    /// Description of the scenario syntax.
    /// </summary>
    public interface IScenarioSyntax : IIfActions, IPingActions, ITVActions, ISayActions, IUrlActions, IEedomusActions, ISynologyDownloadActions
    {
        /// <summary>
        /// Generic lambda action.
        /// </summary>
        /// <param name="actionToPerform">Action to execute.</param>
        /// <returns>Scenario syntax.</returns>
        IScenarioSyntax Action(Func<bool> actionToPerform);

        /// <summary>
        /// Create the scenario.
        /// </summary>
        /// <returns>Instance of the scenario.</returns>
        IScenario ToScenario();

        /// <summary>
        /// Start a scenario.
        /// </summary>
        void Start();

        /// <summary>
        /// Send infra red code on USB-UIRT device.
        /// </summary>
        /// <param name="infraRedCode">infra red ode to send.</param>
        /// <returns>Scenario syntax.</returns>
        IScenarioSyntax UsbUirt(string infraRedCode);

        /// <summary>
        /// Make a precision.
        /// </summary>
        /// <param name="sentences">Sentences of the precision.</param>
        /// <param name="response">Response of the precision.</param>
        /// <param name="scenario">Scenario to execute.</param>
        /// <returns>Scenario syntax.</returns>
        IScenarioSyntax Precision(string[] sentences, string[] response, Func<string, IScenarioSyntax, IScenarioSyntax> scenario);

        /// <summary>
        /// Execute an external process.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="parameters">Parameters of command.</param>
        /// <returns>Scenario syntax.</returns>
        IScenarioSyntax Command(string fileName, string parameters);

        /// <summary>
        /// Log message.
        /// </summary>
        /// <param name="format">Format of the message.</param>
        /// <param name="data">Data of the message.</param>
        /// <returns>Scenario syntax.</returns>
        IScenarioSyntax Log(string format, object data);

        /// <summary>
        /// Add child to this scenario.
        /// </summary>
        /// <param name="scenario">Child scenario.</param>
        void AddChild(IScenario scenario);
    }
}
