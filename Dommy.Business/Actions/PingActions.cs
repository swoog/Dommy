//-----------------------------------------------------------------------
// <copyright file="PingActions.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Actions
{
    using System.Net.NetworkInformation;
    using Dommy.Business.Syntax;
    using Ninject.Extensions.Logging;

    /// <summary>
    /// Implementation of ping conditional syntax.
    /// </summary>
    public class PingActions : Dommy.Business.Actions.IPingActions
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
        public PingActions(IScenarioSyntax scenario, ILogger logger)
        {
            this.scenario = scenario;
            this.logger = logger;
        }

        /// <summary>
        /// Implementation of ping conditional syntax.
        /// </summary>
        /// <param name="ip">IP to ping.</param>
        /// <param name="b">Status excepted.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax IfPing(string ip, bool b)
        {
            this.scenario.Action(() =>
            {
                using (var p = new Ping())
                {
                    var ret = p.Send(ip, 2000);

                    this.logger.Debug("Ping {0} : {1}", ip, ret.Status);

                    var value = ret.Status == IPStatus.Success;

                    if (b)
                    {
                        return value;
                    }

                    return !value;
                }
            });

            return this.scenario;
        }
    }
}
