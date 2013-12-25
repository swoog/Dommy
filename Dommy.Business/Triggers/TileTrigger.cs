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

        public void Init(Engine engine, Scenarios.IScenario scenario)
        {
            this.tileManager.AddTile(new Tile
            {
                Title = this.Title,
                BackGroundColor = this.BackgroundColor,
                Size = TileSize.Medium,
                Url = this.Url,
            });
        }

        public string Title { get; set; }

        public System.Drawing.Color BackgroundColor { get; set; }

        public string Url { get; set; }
    }
}
