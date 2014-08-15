//-----------------------------------------------------------------------
// <copyright file="SettingsController.cs" company="TrollCorp">
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
    using Dommy.Business.Configs;
    using Dommy.Business.Services;
    public class SettingsController : Controller
    {
        private IClientFactory<ISettings> clientSettings;

        public SettingsController(IClientFactory<ISettings> clientSettings)
        {
            this.clientSettings = clientSettings;
        }

        //
        // GET: /Settings/
        public ActionResult Index()
        {
            using (var tileManager = clientSettings.Create())
            {
                var settings = tileManager.Channel.GetSettings();
            }

            return View();
        }
	}
}