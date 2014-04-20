using Dommy.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business
{
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
