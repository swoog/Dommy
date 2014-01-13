//-----------------------------------------------------------------------
// <copyright file="ITimeTriggerSyntax.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Triggers
{
    using System;
    using Dommy.Business.Syntax;

    /// <summary>
    /// Time trigger syntax.
    /// </summary>
    public interface ITimeTriggerSyntax : IExtendSyntax
    {
        /// <summary>
        /// Create scenario based on a TimeSpan.
        /// </summary>
        /// <param name="startDate">Start date of the trigger.</param>
        /// <param name="tick">Delay to execute trigger.</param>
        /// <returns>Trigger scenario syntax.</returns>
        ITriggerScenarioSyntax TimeTrigger(DateTime startDate, TimeSpan tick);
    }
}
