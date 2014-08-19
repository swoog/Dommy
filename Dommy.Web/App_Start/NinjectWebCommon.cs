//-----------------------------------------------------------------------
// <copyright file="NinjectWebCommon.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

[assembly: WebActivator.PreApplicationStartMethod(typeof(Dommy.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(Dommy.Web.App_Start.NinjectWebCommon), "Stop")]

namespace Dommy.Web.App_Start
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Dommy.Business;
    using Dommy.Business.Services;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Extensions.Conventions;
    using Ninject.Web.Common;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IClientFactory<IEngine>>().To<ClientFactory<IEngine>>();
            kernel.Bind<IClientFactory<ITileManager>>().To<ClientFactory<ITileManager>>();
        }

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            ViewEngines.Engines.Add(new ResolverEngine(ViewEngines.Engines.OfType<RazorViewEngine>().FirstOrDefault()));

            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
    }
}