//-----------------------------------------------------------------------
// <copyright file="IUrlActions.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Actions
{
    using Dommy.Business.Syntax;

    /// <summary>
    /// Url syntax.
    /// </summary>
    public interface IUrlActions : IExtendSyntax
    {
        /// <summary>
        /// Get a scrapper for url.
        /// </summary>
        /// <typeparam name="T">Type of object to create when scrap.</typeparam>
        /// <param name="url">Url to scrap.</param>
        /// <returns>Scrapper action.</returns>
        IScrapActions<T> GetUrl<T>(string url);
    }
}
