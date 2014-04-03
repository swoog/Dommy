using Dommy.Business.Configs;
using Dommy.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dommy.Web.Controllers
{
    public class SettingsController : Controller
    {
        //
        // GET: /Settings/
        public ActionResult Index()
        {
            using (var tileManager = Client<ISettings>.Create())
            {

            }

            return View();
        }
	}
}