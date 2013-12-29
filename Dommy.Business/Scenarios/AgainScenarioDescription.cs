//-----------------------------------------------------------------------
// <copyright file="AgainScenarioDescription.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Scenarios
{
    using System;
    using System.Diagnostics.Contracts;
    using Dommy.Business.Syntax;

    /// <summary>
    /// This action make the last action executed.
    /// </summary>
    public class AgainScenarioDescription : IScenarioDescription, IActionLogger
    {
        /// <summary>
        /// Gets or sets last time executed action.
        /// </summary>
        public static DateTime LastExecution { get; set; }

        /// <summary>
        /// Gets or sets Last executed action.
        /// </summary>
        public static IScenario LastScenario { get; set; }

        /// <summary>
        /// Method to log executed actions.
        /// </summary>
        /// <param name="scenario">Executed scenario.</param>
        public void ExecutionOf(IScenario scenario)
        {
            Contract.Requires(scenario != null);

            // If this not Again action.
            if (scenario.ScenarioName == null || !scenario.ScenarioName.StartsWith("Again", StringComparison.Ordinal))
            {
                LastExecution = DateTime.Now;
                LastScenario = scenario;
            }
        }

        /// <summary>
        /// Create scenario.
        /// </summary>
        public void Create()
        {
            Scenario.Create("Again")
                        .SpeechTrigger("recommence", "encore")
                        .Action(this.Again)
                        .Start();

            Scenario.Create("Again No prefix")
             .NoPrefixSpeechTrigger("recommence", "encore")
             .Action(this.Again)
             .Start();
        }

        /// <summary>
        /// Execute the last logged scenario.
        /// </summary>
        /// <returns>Return true if the operation must to continue.</returns>
        private bool Again()
        {
            var timeSpan = DateTime.Now.Subtract(LastExecution);

            if (LastScenario != null && timeSpan.TotalSeconds > 1 && timeSpan.TotalMinutes < 5)
            {
                LastScenario.Run();
            }

            return true;
        }
    }
}
