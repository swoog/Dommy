using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Dommy.Web.App_Start
{
    public class ResolverEngine : IViewEngine
    {
        private RazorViewEngine razorViewEngine;

        public ResolverEngine(RazorViewEngine razorViewEngine)
        {
            // TODO: Complete member initialization
            this.razorViewEngine = razorViewEngine;
        }

        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            return new ViewEngineResult(new string[0]);
        }

        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return new ViewEngineResult(new string[0]);
        }

        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
        }
    }
}