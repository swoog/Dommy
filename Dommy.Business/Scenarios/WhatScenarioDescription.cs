//-----------------------------------------------------------------------
// <copyright file="WhatScenarioDescription.cs" company="TrollCorp">
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

    /// <summary>
    /// This action make the last action executed.
    /// </summary>
    public class WhatScenarioDescription : IScenarioDescription, IActionLogger
    {
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
            // If this not Again action.
            if (scenario.ScenarioName != "What")
            {
                LastScenario = scenario;
            }
        }

        private class WhatName
        {
            public string Name { get; set; }
        }

        public void Create()
        {
            var name = new WhatName { Name = String.Empty };

            Scenario.Create("What")
                        .SpeechTrigger("dernierre action", "qu'a tu fais")
                        .Action(() =>
                        {
                            if (WhatScenarioDescription.LastScenario != null)
                            {
                                name.Name = WhatScenarioDescription.LastScenario.ScenarioName;
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        })
                        .Say(name, "J'ai exécuté {Name}")
                        .Start();
        }
    }
}
