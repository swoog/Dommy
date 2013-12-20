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
    public class TvActions : ITvActions
    {
        /// <summary>
        /// TV helper used to send commands.
        /// </summary>
        private ITvHelper teleHelper;

        /// <summary>
        /// Scenario syntax to use.
        /// </summary>
        private IScenarioSyntax scenario;

        /// <summary>
        /// Information logger.
        /// </summary>
        private ISpeechLogger logger;

        /// <summary>
        /// Asynchronous helper.
        /// </summary>
        private AsyncHelper asyncHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TvActions" /> class.
        /// </summary>
        /// <param name="scenario">Scenario syntax to use.</param>
        /// <param name="teleHelper">TV helper used to send commands.</param>
        /// <param name="asyncHelper">Asynchronous helper.</param>
        public TvActions(IScenarioSyntax scenario, ITvHelper teleHelper, AsyncHelper asyncHelper)
        {
            this.scenario = scenario;
            this.teleHelper = teleHelper;
            this.asyncHelper = asyncHelper;
        }

        /// <summary>
        /// Execute command on TV.
        /// </summary>
        /// <param name="command">Command to execute.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax TvCommand(TvCommand command)
        {
            this.scenario.Action(() =>
            {
                this.asyncHelper.Wait(() =>
                {
                    this.teleHelper.Command(command);
                });

                return true;
            });

            return this.scenario;
        }

        /// <summary>
        /// Change canal on TV.
        /// </summary>
        /// <param name="canal">Canal to change.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax TvCanal(int canal)
        {
            this.scenario.Action(() =>
            {
                this.asyncHelper.Wait(() =>
                {
                    this.teleHelper.Canal(canal);
                });

                return true;
            });

            return this.scenario;
        }

        /// <summary>
        /// Change sound on TV.
        /// </summary>
        /// <param name="quantity">Quantity to change volume.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax TvSound(int quantity)
        {
            this.scenario.Action(() =>
            {
                this.asyncHelper.Wait(() =>
                {
                    if (quantity > 0)
                    {
                        for (int i = 0; i < quantity; i++)
                        {
                            this.teleHelper.Command(Tools.TvCommand.VolUp);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < -quantity; i++)
                        {
                            this.teleHelper.Command(Tools.TvCommand.VolDown);
                        }
                    }
                });

                return true;
            });

            return this.scenario;
        }

        /// <summary>
        /// Mute on TV.
        /// </summary>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax TvMute()
        {
            this.scenario.Action(() =>
            {
                this.asyncHelper.Wait(() =>
                {
                    this.teleHelper.Command(Tools.TvCommand.Mute);
                });

                return true;
            });

            return this.scenario;
        }
    }
}
