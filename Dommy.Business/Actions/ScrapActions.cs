//-----------------------------------------------------------------------
// <copyright file="ScrapActions.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business.Actions
{
    using System;

    /// <summary>
    /// Scrapper syntax.
    /// </summary>
    public class ScrapActions : IScrapActions
    {
        /// <summary>
        /// End of scrap.
        /// </summary>
        /// <returns>Scenario syntax.</returns>
        public Syntax.IScenarioSyntax EndScrap()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Scrap a value.
        /// </summary>
        /// <param name="path">Path of the value.</param>
        /// <param name="property">Property set.</param>
        /// <returns>Scrap action.</returns>
        public IScrapActions Scrap(string path, string property)
        {
            throw new NotImplementedException();
        }
    }
}
