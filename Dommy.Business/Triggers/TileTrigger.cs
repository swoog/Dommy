﻿//-----------------------------------------------------------------------
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
            this.tileManager.AddTile(new Tile { Title = this.Title, BackGroundColor = this.BackgroundColor, Size = TileSize.Medium });
        }

        public string Title { get; set; }

        public System.Drawing.Color BackgroundColor { get; set; }
    }
}
