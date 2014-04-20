//-----------------------------------------------------------------------
// <copyright file="IPingActions.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Actions
{
    using System;
    using Dommy.Business.Syntax;

    /// <summary>
    /// Ping conditional syntax.
    /// </summary>
    public interface IPingActions : IExtendSyntax
    {
        /// <summary>
        /// Ping conditional syntax.
        /// </summary>
        /// <param name="ip">IP to ping.</param>
        /// <param name="expectedStatus">Status excepted.</param>
        /// <returns>Scenario syntax.</returns>
        IScenarioSyntax IfPing(string ip, bool expectedStatus);
    }
}
