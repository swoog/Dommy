//-----------------------------------------------------------------------
// <copyright file="IScrapActions.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Actions
{
    using Dommy.Business.Syntax;

    /// <summary>
    /// Scrapper syntax.
    /// </summary>
    public interface IScrapActions
    {
        /// <summary>
        /// Scrap a value.
        /// </summary>
        /// <param name="path">Path of the value.</param>
        /// <param name="property">Property set.</param>
        /// <returns>Scrap action.</returns>
        IScrapActions Scrap(string path, string property);

        /// <summary>
        /// End of scrap.
        /// </summary>
        /// <returns>Scenario syntax.</returns>
        IScenarioSyntax EndScrap();
    }
}
