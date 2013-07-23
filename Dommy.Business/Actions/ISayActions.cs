//-----------------------------------------------------------------------
// <copyright file="ISayActions.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Actions
{
    using System;
    using Dommy.Business.Syntax;

    /// <summary>
    /// Say syntax.
    /// </summary>
    public interface ISayActions : IExtendSyntax
    {
        /// <summary>
        /// Say sentences alternatively.
        /// </summary>
        /// <param name="sentences">Sentences to say.</param>
        /// <returns>Scenario syntax.</returns>
        IScenarioSyntax Say(params string[] sentences);

        /// <summary>
        /// Say sentences alternatively with context object. See <see cref="StringHelper"/> class for more details.
        /// </summary>
        /// <param name="data">Data object to use for replacement.</param>
        /// <param name="sentences">Sentences to say.</param>
        /// <returns>Scenario syntax.</returns>
        IScenarioSyntax Say(object data, params string[] sentences);
    }
}
