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
    public interface IScrapActions<T>
    {
        /// <summary>
        /// Scrap a value.
        /// </summary>
        /// <param name="path">Path of the value.</param>
        /// <param name="propertyName">Property set.</param>
        /// <returns>Scrap action.</returns>
        IScrapActions<T> Scrap(string path, string propertyName);

        /// <summary>
        /// End of scrap.
        /// </summary>
        /// <returns>Scenario syntax.</returns>
        IScenarioSyntax EndScrap();
    }
}
