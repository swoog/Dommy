using Dommy.Business.Scenarios;
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
        private Dictionary<int, IScenario> scenarios = new Dictionary<int, IScenario>();

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
