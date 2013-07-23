//-----------------------------------------------------------------------
// <copyright file="IIfActions.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Actions
{
    using System;
    using Dommy.Business.Syntax;

    /// <summary>
    /// Conditional scenario syntax.
    /// </summary>
    public interface IIfActions : IExtendSyntax
    {
        /// <summary>
        /// Conditional syntax.
        /// </summary>
        /// <param name="predicate">Predicate to evaluate.</param>
        /// <param name="trueScenario">Scenario to execute when it's true.</param>
        /// <param name="falseScenario">Scenario to execute when it's false. (Optional)</param>
        /// <returns>Scenario syntax.</returns>
        IScenarioSyntax If(Func<bool> predicate, Func<IScenarioSyntax, Dommy.Business.Syntax.IScenarioSyntax> trueScenario, Func<Dommy.Business.Syntax.IScenarioSyntax, Dommy.Business.Syntax.IScenarioSyntax> falseScenario = null);
    }
}
