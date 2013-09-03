//-----------------------------------------------------------------------
// <copyright file="AgainAction.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Scenarios
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dommy.Business.Scenarios;
    using Dommy.Business.Syntax;
    using System.Diagnostics.Contracts;

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
        /// <param name="sentence">Sentence match.</param>
        /// <param name="action">Executed action.</param>
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

        public void Create()
        {
            Scenario.Create("Again")
                        .SpeechTrigger("recommence", "encore")
                        .Action(Again)
                        .Start();

            Scenario.Create("Again No prefix")
             .NoPrefixSpeechTrigger("recommence", "encore")
             .Action(Again)
             .Start();
        }

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
