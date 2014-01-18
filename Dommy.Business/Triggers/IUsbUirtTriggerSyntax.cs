//-----------------------------------------------------------------------
// <copyright file="IUsbUirtTriggerSyntax.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Triggers
{
    using Dommy.Business.Syntax;

    /// <summary>
    /// Syntax of USB-UIRT trigger.
    /// </summary>
    public interface IUsbUirtTriggerSyntax : IExtendSyntax
    {
        /// <summary>
        /// Create scenario based on infra red code returned by USB-UIRT module.
        /// </summary>
        /// <param name="infraRedCode">Infra red code.</param>
        /// <returns>Trigger scenario syntax.</returns>
        ITriggerScenarioSyntax UsbUirtTrigger(string infraRedCode);
    }
}
