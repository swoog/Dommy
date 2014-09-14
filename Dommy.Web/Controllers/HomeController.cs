
namespace Dommy.Web.Controllers
{
    using System.Web.Mvc;
    using Dommy.Business.Services;
    using Dommy.Web.Models;

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

    }
}
