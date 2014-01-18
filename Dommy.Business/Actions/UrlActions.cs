//-----------------------------------------------------------------------
// <copyright file="UrlActions.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Actions
{
    using Dommy.Business.Syntax;
    using Ninject.Extensions.Logging;

    /// <summary>
    /// Url syntax.
    /// </summary>
    public class UrlActions : IUrlActions
    {
        /// <summary>
        /// Scenario syntax to use.
        /// </summary>
        private IScenarioSyntax scenario;

        /// <summary>
        /// Information logger.
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlActions" /> class.
        /// </summary>
        /// <param name="scenario">Scenario syntax to use.</param>
        /// <param name="logger">Information logger.</param>
        public UrlActions(IScenarioSyntax scenario, ILogger logger)
        {
            this.scenario = scenario;
            this.logger = logger;
        }

        /// <summary>
        /// Get a scrapper for url.
        /// </summary>
        /// <typeparam name="T">Type of object to create when scrap.</typeparam>
        /// <param name="url">Url to scrap.</param>
        /// <returns>Scrapper action.</returns>
        public IScrapActions GetUrl<T>(string url)
        {
            var scrapper = new ScrapActions();

            // return this.scenario.Action(() =>
            // {
            //     return true;
            // });
            return scrapper;
        }
    }
}
