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
                return View(new HomeIndexModel { Name = engine.Channel.GetName(), Tiles = tileManager.Channel.GetTiles() });
            }
        }

        //public ActionResult Tile(int id)
        //{
        //    using (var tileManager = Client<ITileManager>.Create())
        //    {
        //        var tile = tileManager.Channel.GetTile(id);

        //        if (!string.IsNullOrEmpty(tile.Url))
        //        {
        //            return RedirectToAction(tile.Url, "tile");
        //        }
        //    }

        //    return null;
        //}
    }
}
