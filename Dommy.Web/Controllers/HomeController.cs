﻿//-----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Dommy.Business;
    using Dommy.Business.Services;
    using Dommy.Web.ActionResults;
    using Dommy.Web.Models;
    public class HomeController : Controller
    {
        private IClientFactory<IEngine> clientEngine;
        private IClientFactory<ITileManager> clientTileManager;

        public HomeController(IClientFactory<IEngine> clientEngine, IClientFactory<ITileManager> clientTileManager)
        {
            this.clientEngine = clientEngine;
            this.clientTileManager = clientTileManager;
        }

        public ActionResult Index()
        {
            using (var engine = clientEngine.Create())
            {
                return View(new HomeIndexModel { Name = engine.Channel.GetEngineName() });
            }
        }

        public ActionResult Tiles()
        {
            using (var tileManager = clientTileManager.Create())
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

                return new JsonNetResult() { Data = sections };
            }
        }

        public ActionResult TileRender(int tileId)
    {
            using (var tileManager = clientTileManager.Create())
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
            using (var tileManager = clientTileManager.Create())
            {
                tileManager.Channel.Start(id);
        }

            return null;
        }
    }
}
