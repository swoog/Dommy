//-----------------------------------------------------------------------
// <copyright file="TileManager.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business
{
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;

    using Dommy.Business.Scenarios;
    using Dommy.Business.Services;

    /// <summary>
    /// Tile manager.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class TileManager : ITileManager
    {
        private int nextId = 1;

        private Dictionary<int, Tile> tiles = new Dictionary<int, Tile>();
        private Dictionary<int, IScenario> scenarios = new Dictionary<int, IScenario>();

        /// <summary>
        /// Add a tile to the manager.
        /// </summary>
        /// <param name="tile">Tile data.</param>
        public void AddTile(Tile tile)
        {
            tile.Id = nextId++;
            this.tiles.Add(tile.Id, tile);
        }

        public void AddTile(Tile tile, Scenarios.IScenario scenario)
        {
            this.AddTile(tile);
            scenarios.Add(tile.Id, scenario);
        }

        /// <summary>
        /// Get all tiles.
        /// </summary>
        /// <returns>List of tiles.</returns>
        public IList<Tile> GetTiles()
        {
            return tiles.Values.ToList();
        }

        public Tile GetTile(int id)
        {
            return tiles[id];
        }

        public void Start(int id)
        {
            if (scenarios.ContainsKey(id))
            {
                scenarios[id].RunAsync();
            }
        }
    }
}
