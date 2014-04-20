//-----------------------------------------------------------------------
// <copyright file="TvActions.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Actions
{
    using Dommy.Business.Syntax;
    using Dommy.Business.Tools;

    /// <summary>
    /// Implementation of TV actions.
    /// </summary>
    public class TVActions : ITVActions
    {
        /// <summary>
        /// TV helper used to send commands.
        /// </summary>
        private ITVHelper teleHelper;

        /// <summary>
        /// Scenario syntax to use.
        /// </summary>
        private IScenarioSyntax scenario;

        /// <summary>
        /// Initializes a new instance of the <see cref="TVActions" /> class.
        /// </summary>
        /// <param name="scenario">Scenario syntax to use.</param>
        /// <param name="teleHelper">TV helper used to send commands.</param>
        public TVActions(IScenarioSyntax scenario, ITVHelper teleHelper)
        {
            this.scenario = scenario;
            this.teleHelper = teleHelper;
        }

        /// <summary>
        /// Execute command on TV.
        /// </summary>
        /// <param name="command">Command to execute.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax TVCommand(TVCommand command)
        {
            this.scenario.Action(() =>
            {
                this.teleHelper.Command(command);

                return true;
            });

            return this.scenario;
        }

        /// <summary>
        /// Change canal on TV.
        /// </summary>
        /// <param name="canal">Canal to change.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax TVCanal(int canal)
        {
            this.scenario.Action(() =>
            {
                this.teleHelper.Canal(canal);

                return true;
            });

            return this.scenario;
        }

        /// <summary>
        /// Change sound on TV.
        /// </summary>
        /// <param name="quantity">Quantity to change volume.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax TVSound(int quantity)
        {
            this.scenario.Action(() =>
            {
                if (quantity > 0)
                {
                    for (int i = 0; i < quantity; i++)
                    {
                        this.teleHelper.Command(Tools.TVCommand.VolUp);
                    }
                }
                else
                {
                    for (int i = 0; i < -quantity; i++)
                    {
                        this.teleHelper.Command(Tools.TVCommand.VolDown);
                    }
                }

                return true;
            });

            return this.scenario;
        }

        /// <summary>
        /// Mute on TV.
        /// </summary>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax TVMute()
        {
            this.scenario.Action(() =>
            {
                this.teleHelper.Command(Tools.TVCommand.Mute);

                return true;
            });

            return this.scenario;
        }
    }
}
