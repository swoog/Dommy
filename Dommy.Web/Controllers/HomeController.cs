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
                return View(new HomeIndexModel { Name = engine.Channel.GetEngineName(), Tiles = tileManager.Channel.GetTiles() });
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