//-----------------------------------------------------------------------
// <copyright file="ITvActions.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Actions
{
    using Dommy.Business.Syntax;
    using Dommy.Business.Tools;

    /// <summary>
    /// TV syntax.
    /// </summary>
    public interface ITVActions : IExtendSyntax
    {
        /// <summary>
        /// Execute command on TV.
        /// </summary>
        /// <param name="command">Command to execute.</param>
        /// <returns>Scenario syntax.</returns>
        IScenarioSyntax TVCommand(TVCommand command);

        /// <summary>
        /// Change canal on TV.
        /// </summary>
        /// <param name="canal">Canal to change.</param>
        /// <returns>Scenario syntax.</returns>
        IScenarioSyntax TVCanal(int canal);

        /// <summary>
        /// Change sound on TV.
        /// </summary>
        /// <param name="quantity">Quantity to change volume.</param>
        /// <returns>Scenario syntax.</returns>
        IScenarioSyntax TVSound(int quantity);

        /// <summary>
        /// Mute on TV.
        /// </summary>
        /// <returns>Scenario syntax.</returns>
        IScenarioSyntax TVMute();
    }
}
