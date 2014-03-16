using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Dommy.Web.Controllers
{
    public class ResourceController : Controller
    {
        //
        // GET: /Resource/
        public ActionResult Index(string resourceName)
        {
            var assembly = typeof(Dommy.Business.Engine).Assembly;

            Stream stream = assembly.GetManifestResourceStream(resourceName);
            return this.File(stream, GetContentType(resourceName));
        }

        private static string GetContentType(string resourceName)
        {
            var extention = resourceName.Substring(resourceName.LastIndexOf('.')).ToLower();
            switch (extention)
            {
                case ".gif":
                    return "image/gif";
                case ".js":
                    return "text/javascript";
                case ".css":
                    return "text/css";
                default:
                    return "text/html";
            }
        }

    }
}