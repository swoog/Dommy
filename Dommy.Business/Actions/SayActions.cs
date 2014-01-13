//-----------------------------------------------------------------------
// <copyright file="SayActions.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Actions
{
    using Dommy.Business.Syntax;
    using Dommy.Business.Tools;

    /// <summary>
    /// Implementation of say syntax.
    /// </summary>
    public class SayActions : Dommy.Business.Actions.ISayActions
    {
        /// <summary>
        /// Scenario syntax to use.
        /// </summary>
        private IScenarioSyntax scenario;

        /// <summary>
        /// Information logger.
        /// </summary>
        private ISpeechLogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SayActions" /> class.
        /// </summary>
        /// <param name="scenario">Scenario syntax to use.</param>
        /// <param name="logger">Information logger.</param>
        public SayActions(IScenarioSyntax scenario, ISpeechLogger logger)
        {
            this.scenario = scenario;
            this.logger = logger;
        }

        /// <summary>
        /// Implementation of say sentences alternatively.
        /// </summary>
        /// <param name="sentences">Sentences to say.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax Say(params string[] sentences)
        {
            return this.Say(null, sentences);
        }

        /// <summary>
        /// Implementation of say sentences alternatively with context object. See <see cref="StringHelper"/> class for more details.
        /// </summary>
        /// <param name="data">Data object to use for replacement.</param>
        /// <param name="sentences">Sentences to say.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax Say(object data, params string[] sentences)
        {
            this.scenario.Action(() =>
            {
                var s = StringHelper.Format(sentences, data);

                this.logger.Say(Actor.Dommy, s);
                return true;
            });

            return this.scenario;
        }
    }
}
