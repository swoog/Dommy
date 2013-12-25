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
        private int nextId = 1;

        private Dictionary<int, Tile> tiles = new Dictionary<int, Tile>();

        public void AddTile(Tile tile)
        {
            tile.Id = nextId++;
            this.tiles.Add(tile.Id, tile);
        }

        public List<Tile> GetTiles()
        {
            return tiles.Values.ToList();
        }

        public Tile GetTile(int id)
        {
            return tiles[id];
        }
    }
}
