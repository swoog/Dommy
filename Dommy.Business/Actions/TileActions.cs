//-----------------------------------------------------------------------
// <copyright file="TileActions.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Actions
{
    using Dommy.Business.Syntax;
    using Microsoft.AspNet.SignalR.Client;

    /// <summary>
    /// Implementation of tile actions syntax.
    /// </summary>
    public class TileActions : ITileActions
    {
        /// <summary>
        /// Scenario syntax to use.
        /// </summary>
        private IScenarioSyntax scenario;

        /// <summary>
        /// Tile hub to update.
        /// </summary>
        private IHubProxy tileHub;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileActions" /> class.
        /// </summary>
        /// <param name="scenario">Scenario syntax to use.</param>
        public TileActions(IScenarioSyntax scenario, HubConnection connection)
        {
            this.scenario = scenario;
            this.tileHub = connection.CreateHubProxy("tile");
        }

        /// <summary>
        /// Update tile information on interface.
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public IScenarioSyntax TileUpdate(Tile tile)
        {
            return this.scenario.Action(() =>
            {
                this.tileHub.Invoke("UpdateTile", tile);
                return true;
            });
        }
    }
}