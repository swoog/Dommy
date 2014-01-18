//-----------------------------------------------------------------------
// <copyright file="IRestTriggerSyntax.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Triggers
{
    using System;
    using Dommy.Business.Syntax;

    /// <summary>
    /// Rest trigger syntax.
    /// </summary>
    public interface IRestTriggerSyntax : IExtendSyntax
    {
        /// <summary>
        /// Create a REST trigger used to start scenario.
        /// </summary>
        /// <param name="url">Url used to start scenario.</param>
        /// <returns>Trigger scenario syntax.</returns>
        ITriggerScenarioSyntax RestTrigger(string url);

        /// <summary>
        /// Create a REST trigger used to start scenario.
        /// </summary>
        /// <param name="url">Url used to start scenario.</param>
        /// <param name="data">Object used to map the url. Example : /hello/{MyName} and MyName is property of data.</param>
        /// <returns>Trigger scenario syntax.</returns>
        ITriggerScenarioSyntax RestTrigger(string url, object data);
    }
}
