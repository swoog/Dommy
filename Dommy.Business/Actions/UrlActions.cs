using Dommy.Business.Syntax;
using Ninject.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Actions
{
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
        /// Initializes a new instance of the <see cref="PingActions" /> class.
        /// </summary>
        /// <param name="scenario">Scenario syntax to use.</param>
        /// <param name="logger">Information logger.</param>
        public UrlActions(IScenarioSyntax scenario, ILogger logger)
        {
            this.scenario = scenario;
            this.logger = logger;
        }

        public IScrapActions GetUrl<T>(string url)
        {
            var scrapper = new ScrapActions();

            //return this.scenario.Action(() =>
            //{
            //    return true;
            //});

            return scrapper;
        }
    }
}
