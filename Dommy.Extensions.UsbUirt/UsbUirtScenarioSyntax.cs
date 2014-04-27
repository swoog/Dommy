//-----------------------------------------------------------------------
// <copyright file="UsbUirtScenarioSyntax.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Extensions.UsbUirt
{
    using Dommy.Business.Syntax;
    using Ninject;
    using global::UsbUirt;
    using global::UsbUirt.Enums;


    /// <summary>
    /// USB-UIRT scenario syntax.
    /// </summary>
    public static class UsbUirtScenarioSyntax
    {
        /// <summary>
        /// Send infra red code on USB-UIRT device.
        /// </summary>
        /// <param name="infraRedCode">infra red ode to send.</param>
        /// <param name="scenario">Scenario to add trigger.</param>
        /// <returns>Scenario syntax.</returns>
        public static IScenarioSyntax UsbUirt(this IScenarioSyntax scenario, string infraRedCode)
        {
            return scenario.Action(() =>
            {
                using (var driver = new Driver())
                {
                    using (var transmitter = new Transmitter(driver))
                    {
                        transmitter.Transmit(infraRedCode, emitter: Emitter.Internal);
                    }

                    return true;
                }
            });
        }

        /// <summary>
        /// Create scenario based on infra red code returned by USB-UIRT module.
        /// </summary>
        /// <param name="infraRedCode">Infra red code.</param>
        /// <returns>Trigger scenario syntax.</returns>
        public static ITriggerScenarioSyntax UsbUirtTrigger(this ITriggerScenarioSyntax  scenario,string infraRedCode)
        {
            var t = scenario.Kernel.Get<UsbUirtTrigger>();
            t.Code = infraRedCode;
            return scenario.Trigger(t);
        }
    }
}
