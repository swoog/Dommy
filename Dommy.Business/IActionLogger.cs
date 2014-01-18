//-----------------------------------------------------------------------
// <copyright file="IActionLogger.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business
{
    using Dommy.Business.Scenarios;

    /// <summary>
    /// Interface of scenario logger.
    /// </summary>
    public interface IActionLogger
    {
        /// <summary>
        /// Indicate execution of scenario.
        /// </summary>
        /// <param name="scenario">Scenario executed.</param>
        void ExecutionOf(IScenario scenario);
    }
}
