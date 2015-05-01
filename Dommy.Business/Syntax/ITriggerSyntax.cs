//-----------------------------------------------------------------------
// <copyright file="ITriggerSyntax.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Syntax
{
    using Dommy.Business.Triggers;

    /// <summary>
    /// Trigger syntax.
    /// </summary>
    public interface ITriggerSyntax : ISpeechTriggerSyntax, IRestTriggerSyntax, ITimeTriggerSyntax, ITileTriggerSyntax, IStartupTriggerSyntax
    {
        /// <summary>
        /// Scenario without trigger.
        /// </summary>
        /// <returns>Trigger and action syntax.</returns>
        ITriggerScenarioSyntax NoTrigger();

        /// <summary>
        /// Scenario with a trigger.
        /// </summary>
        /// <param name="instanceTrigger">Instance of the trigger.</param>
        /// <returns>Trigger and action syntax.</returns>
        ITriggerScenarioSyntax Trigger(ITrigger instanceTrigger);
    }
}
