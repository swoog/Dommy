//-----------------------------------------------------------------------
// <copyright file="EedomusActions.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Actions
{
    using Dommy.Business.Syntax;
    using Dommy.Business.Tools;
    using Ninject.Extensions.Logging;

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
        /// <param name="eedomusHelper">Eedomus helper</param>
        public EedomusActions(IScenarioSyntax scenario, ILogger logger, EedomusHelper eedomusHelper)
        {
            this.scenario = scenario;
            this.logger = logger;
            this.eedomusHelper = eedomusHelper;
        }

        /// <summary>
        /// Call API eedomus.
        /// </summary>
        /// <param name="api">Eedomus API type. (Local or distant)</param>
        /// <param name="action">Action to execute.</param>
        /// <param name="eedomusId">Eedomus id element.</param>
        /// <param name="value">Optional value to set.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax EedomusCall(EedomusApi api, EedomusAction action, string eedomusId, string value = null)
        {
            return this.scenario.Action(() =>
            {
                this.eedomusHelper.CallService(api, action, eedomusId, value);
                return true;
            });
        }

        /// <summary>
        /// Call local API eedomus.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        /// <param name="eedomusId">Eedomus id element.</param>
        /// <param name="value">Optional value to set.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax EedomusCall(EedomusAction action, string eedomusId, string value = null)
        {
            return this.scenario.Action(() =>
            {
                this.eedomusHelper.CallService(EedomusApi.Local, action, eedomusId, value);
                return true;
            });
        }

         /// <summary>
        /// Call local API eedomus and make on/off (Light, ...).
        /// </summary>
        /// <param name="id">Eedomus id element.</param>
        /// <param name="isOn">Light on or off.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax EedomusOnOff(string id, bool isOn)
        {
            return this.scenario.Action(() =>
            {
                this.eedomusHelper.CallService(EedomusApi.Local, EedomusAction.PeriphValue, id, isOn ? "100" : "0");
                return true;
            });
        }

        /// <summary>
        /// Set value to local API eedomus.
        /// </summary>
        /// <param name="id">Eedomus id element.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax EedomusValue(string id, double value)
        {
            return this.scenario.Action(() =>
            {
                this.eedomusHelper.CallService(EedomusApi.Local, EedomusAction.PeriphValue, id, string.Format("{0}", value));
                return true;
            });
        }
    }
}
