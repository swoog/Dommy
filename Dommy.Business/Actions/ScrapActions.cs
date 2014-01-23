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
    public class ScrapActions<T> : IScrapActions<T>
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
        /// <param name="propertyName">Property set.</param>
        /// <returns>Scrap action.</returns>
        public IScrapActions<T> Scrap(string path, string propertyName)
        {
            throw new NotImplementedException();
        }
    }
}
