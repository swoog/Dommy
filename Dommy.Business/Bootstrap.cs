﻿//-----------------------------------------------------------------------
// <copyright file="Bootstrap.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime;
    using System.Web.UI.WebControls;

    using Dommy.Business.Configs;
    using Dommy.Business.Scenarios;
    using Dommy.Business.Scripts;
    using Dommy.Business.Services;
    using Dommy.Business.Syntax;
    using Dommy.Business.Tools;
    using Dommy.Business.WebHost;
    using Ninject;

    /// <summary>
    /// Bootstrap code for the process.
    /// </summary>
    public static class Bootstrap
    {
        /// <summary>
        /// Bootstrap for default process.
        /// </summary>
        public static void Run()
        {
            ProfileOptimization.SetProfileRoot(@".\");
            ProfileOptimization.StartProfile(string.Format("{0}.profile", AppDomain.CurrentDomain.FriendlyName));

            log4net.Config.XmlConfigurator.Configure();

            var kernel = new StandardKernel();
            var modules = GetModules();

            var directoriesModule = modules.Select(m => Path.GetDirectoryName(m)).ToList();

            AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
                {
                    foreach (var module in directoriesModule)
                    {
                        var file = Path.Combine(module, e.Name);
                        if (File.Exists(file))
                        {
                            return Assembly.LoadFile(file);
                        }
                    }

                    return null;
                };

            kernel.Load(modules);

            Configure.InitKernel(kernel);
            Scenario.InitKernel(kernel);

            Configure.Engine();

            Configure.TextToSpeech()
                .With(c => c.Gender, Gender.Female)
                .With(c => c.Culture, "fr-FR");

            Configure.SpeechToText()
                .With(c => c.Culture, "fr-FR")
                .With(c => c.Confidence, 0.6);

            Configure.Config<RestListener.Config>()
                .With(c => c.Port, 5555);

            Configure.Config<ScriptEngine.Config>()
                .With(c => c.ScriptDirectory, Path.Combine(Environment.CurrentDirectory, @"scenarios"));

            Configure.Config<WebServerHost.Config>()
                .With(c => c.Port, 5556);

            Configure.LoadConfig(Path.Combine(Environment.CurrentDirectory, "config.xml"));

            Configure.Build();

            var web = kernel.Get<WebServerHost>();
            web.Start();

            // Scripting configuration
            kernel.Bind<TileManager>().ToSelf().InSingletonScope();

            kernel.Bind<IServiceHost>().To<ServiceHost<Engine>>();
            kernel.Bind<IServiceHost>().To<ServiceHost<TileManager>>();
            kernel.Bind<IServiceHost>().To<ServiceHost<WebServerHost>>();

            kernel.Bind<AsyncHelper>().ToSelf();
            kernel.Bind<ISpeechLogger>().To<SpeechLogger>().InSingletonScope();

            kernel.Bind<IActionLogger>().To<AgainScenarioDescription>().InSingletonScope();
            kernel.Bind<IActionLogger>().To<WhatScenarioDescription>().InSingletonScope();

            kernel.Bind<IWebRequest>().To<DommyWebRequest>();

            IEnumerable<IServiceHost> services = OpenServices(kernel);

            var engine = kernel.Get<Engine>();

            engine.Init();

            Console.ReadLine();
            engine.Stop();

            CloseServices(services);

            web.Stop();
        }

        private static IEnumerable<string> GetModules()
        {
            var modulesDirectory = Path.Combine(Environment.CurrentDirectory, "modules");

#if DEBUG
            modulesDirectory = Path.Combine(Environment.CurrentDirectory, @"..\..\..\");
#endif

            if (!Directory.Exists(modulesDirectory))
            {
                Directory.CreateDirectory(modulesDirectory);
            }

            return Directory.GetFiles(modulesDirectory, "Dommy.Extensions.*.dll", SearchOption.AllDirectories)
                .Where(f => !f.EndsWith(".x86.dll"));
        }

        /// <summary>
        /// Bootstrap for x86 process.
        /// </summary>
        public static void RunX86()
        {
            ProfileOptimization.SetProfileRoot(@".\");
            ProfileOptimization.StartProfile(string.Format("{0}.profile", AppDomain.CurrentDomain.FriendlyName));

            log4net.Config.XmlConfigurator.Configure();

            var kernel = new StandardKernel();
            kernel.Load("Dommy.*.x86.dll");

            IEnumerable<IServiceHost> services = OpenServices(kernel);

            Console.ReadLine();

            CloseServices(services);
        }

        /// <summary>
        /// Close all service host.
        /// </summary>
        /// <param name="services">All service host</param>
        private static void CloseServices(IEnumerable<IServiceHost> services)
        {
            Contract.Requires(services != null);

            foreach (var item in services)
            {
                item.Close();
            }
        }

        /// <summary>
        /// Open all service host.
        /// </summary>
        /// <param name="kernel">Ninject kernel.</param>
        /// <returns>All open service host.</returns>
        private static IEnumerable<IServiceHost> OpenServices(StandardKernel kernel)
        {
            var services = kernel.GetAll<IServiceHost>().ToList();

            foreach (var item in services)
            {
                item.Open();
            }

            return services;
        }
    }
}
