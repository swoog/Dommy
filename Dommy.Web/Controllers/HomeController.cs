using Dommy.Business;
using Dommy.Business.Services;
using Dommy.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dommy.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var tileManager = Client<ITileManager>.Create())
            using (var engine = Client<IEngine>.Create())
            {
                var tiles = tileManager.Channel.GetTiles();

                var sections = (from t in tiles
                                group t by t.SectionName into g
                                select new SectionModel
                                {
                                    Name = g.First().SectionName ?? "Aucun",
                                    Tiles = g.ToList(),
                                }).ToList();

                var enums = Enum.GetNames(typeof(TileColor));

                for (int i = 0; i < sections.Count; i++)
                {
                    sections[i].Id = i;
                    sections[i].Color = enums[i % enums.Length];
                }

                return View(new HomeIndexModel { Name = engine.Channel.GetEngineName(), Sections = sections });
            }
        }

        public ActionResult TileRender(int tileId)
        {
            using (var tileManager = Client<ITileManager>.Create())
            {
                var tile = tileManager.Channel.GetTile(tileId);

                if (string.IsNullOrEmpty(tile.View))
                {
                    return View(new HomeTileRenderModel { Render = string.Empty });
                }

                var html = RazorEngine.Razor.Parse(tile.View, tile);

                return View(new HomeTileRenderModel { Render = html });
            }
        }

        [HttpPost]
        public ActionResult Tile(int id)
        {
            using (var tileManager = Client<ITileManager>.Create())
            {
                tileManager.Channel.Start(id);
            }

            return null;
        }
    }
}