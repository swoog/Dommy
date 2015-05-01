//-----------------------------------------------------------------------
// <copyright file="TileActions.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Actions
{
    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;
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
        /// Signalr Hub connection.
        /// </summary>
        private HubConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileActions" /> class.
        /// </summary>
        /// <param name="scenario">Scenario syntax to use.</param>
        /// <param name="connection">Signalr hub connection.</param>
        public TileActions(IScenarioSyntax scenario, HubConnection connection)
        {
            Contract.Requires(connection != null);

            this.scenario = scenario;
            this.tileHub = connection.CreateHubProxy("tile");
            this.connection = connection;
        }

        /// <summary>
        /// Update tile information on interface.
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public IScenarioSyntax TileUpdate(Tile tile)
        {
            return this.scenario.Action(() =>
            {
                if (this.connection.State == ConnectionState.Disconnected)
                {
                    Task.WhenAny(this.connection.Start());
                }

                this.tileHub.Invoke("UpdateTile", tile);
                return true;
            });
        }
    }
}