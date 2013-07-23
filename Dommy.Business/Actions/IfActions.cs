//-----------------------------------------------------------------------
// <copyright file="IfActions.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Actions
{
    using System;
    using Dommy.Business.Scenarios;
    using Dommy.Business.Syntax;

    /// <summary>
    /// Conditional scenario syntax.
    /// </summary>
    public class IfActions : IIfActions
    {
        /// <summary>
        /// Scenario syntax to use.
        /// </summary>
        private IScenarioSyntax scenario;

        /// <summary>
        /// Initializes a new instance of the <see cref="IfActions" /> class.
        /// </summary>
        /// <param name="scenario">Scenario syntax to use.</param>
        public IfActions(IScenarioSyntax scenario)
        {
            this.scenario = scenario;
        }

        /// <summary>
        /// Implementation of conditional syntax.
        /// </summary>
        /// <param name="predicate">Predicate to evaluate.</param>
        /// <param name="trueScenario">Scenario to execute when it's true.</param>
        /// <param name="falseScenario">Scenario to execute when it's false. (Optional)</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax If(Func<bool> predicate, Func<IScenarioSyntax, IScenarioSyntax> trueScenario, Func<IScenarioSyntax, IScenarioSyntax> falseScenario = null)
        {
            var trueSc = trueScenario(Scenario.Create().NoTrigger()).ToScenario();
            this.scenario.AddChild(trueSc);

            IScenario falseSc = null;
            if (falseScenario != null)
            {
                falseSc = falseScenario(Scenario.Create().NoTrigger()).ToScenario();
                this.scenario.AddChild(falseSc);
            }

            this.scenario.Action(() =>
            {
                if (predicate())
                {
                    trueSc.Run();
                }
                else if (falseSc != null)
                {
                    falseSc.Run();
                }

                return true;
            });

            return this.scenario;
        }
    }
}
