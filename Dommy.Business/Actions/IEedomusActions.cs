//-----------------------------------------------------------------------
// <copyright file="IEedomusActions.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Actions
{
    using System;
    using Dommy.Business.Syntax;

    /// <summary>
    /// Eedomus syntax.
    /// </summary>
    public interface IEedomusActions : IExtendSyntax
    {
        /// <summary>
        /// Call API eedomus.
        /// </summary>
        /// <param name="api">Eedomus API type. (Local or distant)</param>
        /// <param name="action">Action to execute.</param>
        /// <param name="eedomusId">Eedomus id element.</param>
        /// <param name="value">Optional value to set.</param>
        /// <returns>Scenario syntax.</returns>
        IScenarioSyntax EedomusCall(EedomusApi api, EedomusAction action, string eedomusId, string value = null);

        /// <summary>
        /// Call local API eedomus.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        /// <param name="eedomusId">Eedomus id element.</param>
        /// <param name="value">Optional value to set.</param>
        /// <returns>Scenario syntax.</returns>
        IScenarioSyntax EedomusCall(EedomusAction action, string eedomusId, string value = null);

        /// <summary>
        /// Call local API eedomus and make on/off (Light, ...).
        /// </summary>
        /// <param name="eedomusId">Eedomus id element.</param>
        /// <param name="isOn">Light on or off.</param>
        /// <returns>Scenario syntax.</returns>   
        IScenarioSyntax EedomusOnOff(string eedomusId, bool isOn);

        /// <summary>
        /// Set value to local API eedomus.
        /// </summary>
        /// <param name="eedomusId">Eedomus id element.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>Scenario syntax.</returns>
        IScenarioSyntax EedomusValue(string eedomusId, double value);
    }
}
