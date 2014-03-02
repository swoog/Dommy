using Dommy.Business;
using Dommy.Business.Configs;
using Dommy.Business.Scenarios;
using Dommy.Business.Scripts;
using Dommy.Business.Services;
using Dommy.Business.Syntax;
using Dommy.Business.Tools;
using Dommy.Business.WebHost;
using Ninject;
using Ninject.Extensions.Conventions;
using System;
using System.IO;
using System.Linq;
using System.Runtime;

namespace Dommy.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ProfileOptimization.SetProfileRoot(@".\");
            ProfileOptimization.StartProfile("DommyProfile");

            log4net.Config.XmlConfigurator.Configure();

            var directory = Environment.CurrentDirectory;

            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                directory = Path.Combine(Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "Dommy");

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }

            var kernel = new StandardKernel();
            kernel.Load("Dommy.*.dll");

            Configure.InitKernel(kernel);
            Scenario.InitKernel(kernel);

            Configure.Engine("Dommy");

            Configure.TextToSpeech()
                .With(c => c.Gender, Gender.Female)
                .With(c => c.Culture, "fr-FR");

            Configure.SpeechToText()
                .With(c => c.Culture, "fr-FR")
                .With(c => c.Confidence, 0.6)
                ;

            Configure.Config<RestListener.Config>()
                .With(c => c.Port, 5555);

            Configure.Config<ScriptEngine.Config>()
                .With(c => c.ScriptDirectory, Path.Combine(directory, @"scenarios"));

            Configure.Config<WebServerHost.Config>()
                .With(c => c.Port, 5556);

            Configure.LoadConfig(Path.Combine(directory, "config.xml"));

            Configure.Build();

            kernel.Bind<IListener>().To<UsbUirtListener>();

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

            Scenario.Create("Web logger")
                .TileTrigger("Salon", "Informations", TileColor.Cinnabar, "/Tile/Logger")
                .Start();

            // TODO : Add scenario to restart freebox and router.

            //kernel.Bind<ActionService>().ToSelf();

            //kernel.Bind<ServiceHost<ActionService>>().ToSelf();
            //var actionService = kernel.Get<ServiceHost<ActionService>>();
            //actionService.Open();
            var services = kernel.GetAll<IServiceHost>().ToList();

            foreach (var item in services)
            {
                item.Open();
            }

            var engine = kernel.Get<Engine>();

            engine.Init();

            web.StartSignalR();

            System.Console.ReadLine();
            engine.Stop();

            foreach (var item in services)
            {
                item.Close();
            }

            web.Stop();
        }
    }
}