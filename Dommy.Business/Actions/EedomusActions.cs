//-----------------------------------------------------------------------
// <copyright file="ISayActions.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Actions
{
    using System;
    using Dommy.Business.Syntax;
    using Ninject.Extensions.Logging;
    using Dommy.Business.Tools;

    /// <summary>
    /// Eedomus syntax.
    /// </summary>
    public class EedomusActions : IEedomusActions
    {
        /// <summary>
        /// Scenario syntax to use.
        /// </summary>
        private IScenarioSyntax scenario;

        /// <summary>
        /// Information logger.
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// Eedomus helper.
        /// </summary>
        private EedomusHelper eedomusHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="EedomusActions" /> class.
        /// </summary>
        /// <param name="scenario">Scenario syntax to use.</param>
        /// <param name="logger">Information logger.</param>
        public EedomusActions(IScenarioSyntax scenario, ILogger logger, EedomusHelper eedomusHelper)
        {
            this.scenario = scenario;
            this.logger = logger;
            this.eedomusHelper = eedomusHelper;
        }

        public IScenarioSyntax EedomusCall(EedomusApi api, EedoumusAction action, string eedomusId, string value = null)
        {
            return this.scenario.Action(() =>
            {
                this.eedomusHelper.CallService(api, action, eedomusId, value);
                return true;
            });
        }

        public IScenarioSyntax EedomusCall(EedoumusAction action, string eedomusId, string value = null)
        {
            return this.scenario.Action(() =>
            {
                this.eedomusHelper.CallService(EedomusApi.Local, action, eedomusId, value);
                return true;
            });
        }

        public IScenarioSyntax EedomusOnOff(string id, bool isOn)
        {
            return this.scenario.Action(() =>
            {
                this.eedomusHelper.CallService(EedomusApi.Local, EedoumusAction.PeriphValue, id, isOn ? "100" : "0");
                return true;
            });
        }

        public IScenarioSyntax EedomusValue(string id, double value)
        {
            return this.scenario.Action(() =>
            {
                this.eedomusHelper.CallService(EedomusApi.Local, EedoumusAction.PeriphValue, id, String.Format("{0}", value));
                return true;
            });
        }
    }
}
