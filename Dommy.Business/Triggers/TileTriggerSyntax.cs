using Dommy.Business.Syntax;
using Ninject;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Triggers
{
    public class TileTriggerSyntax : ITileTriggerSyntax
    {
        private IKernel kernel;
        private ITriggerSyntax scenario;

        public TileTriggerSyntax(IKernel kernel, ITriggerSyntax scenario)
        {
            this.kernel = kernel;
            this.scenario = scenario;
        }

        public ITriggerScenarioSyntax TileTrigger(string title, Color backgroundColor)
        {
            return TileTrigger(title, backgroundColor, null);
        }

        public ITriggerScenarioSyntax TileTrigger(string title, Color backgroundColor, string url)
        {
            Tile t;

            return TileTrigger(out t, title, backgroundColor, url);
        }

        public ITriggerScenarioSyntax TileTrigger(out Tile tile, string title, Color backgroundColor)
        {
            return TileTrigger(out tile, title, backgroundColor, null);
        }

        public ITriggerScenarioSyntax TileTrigger(out Tile tile, string title, Color backgroundColor, string url)
        {
            var t = this.kernel.Get<TileTrigger>();
            t.Tile = new Tile
            {
                Title = title,
                BackGroundColor = backgroundColor,
                Size = TileSize.Medium,
                Url = url,
            };

            tile = t.Tile;

            return this.scenario.Trigger(t);
        }
    }
}