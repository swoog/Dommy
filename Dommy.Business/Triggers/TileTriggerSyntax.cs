//-----------------------------------------------------------------------
// <copyright file="TileTriggerSyntax.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business.Triggers
{
    using System.Drawing;
    using Dommy.Business.Syntax;
    using Ninject;

    public class TileTriggerSyntax : ITileTriggerSyntax
    {
        private IKernel kernel;
        private ITriggerSyntax scenario;

        public TileTriggerSyntax(IKernel kernel, ITriggerSyntax scenario)
        {
            this.kernel = kernel;
            this.scenario = scenario;
        }

        public ITriggerScenarioSyntax TileTrigger(string section, string title, TileColor tileColor)
        {
            return TileTrigger(section, title, tileColor, null);
        }

        public ITriggerScenarioSyntax TileTrigger(string section, string title, TileColor tileColor, string url)
        {
            Tile t;

            return TileTrigger(out t, section, title, tileColor, url);
        }

        public ITriggerScenarioSyntax TileTrigger(out Tile tile, string section, string title, TileColor tileColor)
        {
            return TileTrigger(out tile, section, title, tileColor, null);
        }

        public ITriggerScenarioSyntax TileTrigger(out Tile tile, string section, string title, TileColor tileColor, string url)
        {
            var t = this.kernel.Get<TileTrigger>();
            t.Tile = new Tile
            {
                SectionName = section,
                Title = title,
                Color = tileColor,
                Size = TileSize.Medium,
                Url = url,
            };

            tile = t.Tile;

            return this.scenario.Trigger(t);
        }
    }
}
