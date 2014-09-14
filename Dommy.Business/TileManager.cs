//-----------------------------------------------------------------------
// <copyright file="TileManager.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using Dommy.Business.Services;

    /// <summary>
    /// Tile manager.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class TileManager : ITileManager
    {
        /// <summary>
        /// List of all tiles.
        /// </summary>
        private readonly List<Tile> tiles = new List<Tile>();

        /// <summary>
        /// Add a tile to the manager.
        /// </summary>
        /// <param name="tile">Tile data.</param>
        public void AddTile(Tile tile)
        {
            this.tiles.Add(tile);
        }

        /// <summary>
        /// Get all tiles.
        /// </summary>
        /// <returns>List of tiles.</returns>
        public IList<Tile> GetTiles()
        {
            return this.tiles;
        }
    }
}
