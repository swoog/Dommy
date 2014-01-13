//-----------------------------------------------------------------------
// <copyright file="IStartupTriggerSyntax.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Triggers
{
    using Dommy.Business.Syntax;

    /// <summary>
    /// Startup trigger syntax.
    /// </summary>
    public interface IStartupTriggerSyntax : IExtendSyntax
    {
        /// <summary>
        /// Create a scenario started a begin of application.
        /// </summary>
        /// <returns>Trigger scenario syntax.</returns>
        ITriggerScenarioSyntax StartupTrigger();
    }
}
