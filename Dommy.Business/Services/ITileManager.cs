//-----------------------------------------------------------------------
// <copyright file="ITileManager.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Services
{
    using System.Collections.Generic;
    using System.ServiceModel;

    /// <summary>
    /// Service contract for tile manager.
    /// </summary>
    [ServiceContract]
    public interface ITileManager
    {
        /// <summary>
        /// Gets tiles.
        /// </summary>
        /// <returns>List of tiles.</returns>
        [OperationContract]
        IList<Tile> GetTiles();
    }
}
