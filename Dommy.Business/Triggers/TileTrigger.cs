//-----------------------------------------------------------------------
// <copyright file="TileTrigger.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
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
