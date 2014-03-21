using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Business.Triggers
{
    public class TileTrigger : ITrigger
    {
        private TileManager tileManager;

        public TileTrigger(TileManager tileManager)
        {
            this.tileManager = tileManager;
        }

        public void Init(Engine currentEngine, Scenarios.IScenario currentScenario)
        {
            this.tileManager.AddTile(this.Tile, currentScenario);
        }

        public Tile Tile { get; set; }
    }
}
