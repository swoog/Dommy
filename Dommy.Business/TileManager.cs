
namespace Dommy.Business
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using Dommy.Business.Services;

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class TileManager : ITileManager
    {
        private List<Tile> tiles = new List<Tile>();

        public void AddTile(Tile tile)
        {
            this.tiles.Add(tile);
        }

        public IList<Tile> GetTiles()
        {
            return tiles;
        }
    }
}
