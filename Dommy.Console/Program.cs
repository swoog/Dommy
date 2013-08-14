using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Dommy.Business;
using Dommy.Business.Action;
using Ninject.Parameters;
using System.Threading;
using Dommy.Business.Speech;
using Dommy.Business.Tools;
using Dommy.Business.Services;
using Dommy.Model;
using Dommy.Business.Result;
using Dommy.Business.Triggers;
using Dommy.Business.Scenarios;
using Ninject.Extensions.Conventions;
using Dommy.Business.Syntax;
using Dommy.Business.Actions;
using Dommy.Business.Scripts;
using Dommy.Business.Config;
using System.IO;
using Dommy.Business.WebHost;
using System.Diagnostics;

namespace Dommy.Console
{
    class Program
    {
        static void Main(string[] args)
        {
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
                .With(c => c.ScriptDirectory, Path.Combine(directory, @"scripts"));

            Configure.Config<WebServerHost.Config>()
                .With(c => c.Port, 5000);

            Configure.LoadConfig(Path.Combine(directory, "config.xml"));

            Configure.Build();

            var web = kernel.Get<WebServerHost>();
            web.Start();
            
            // Scripting configuration

            kernel.Bind(a => a.FromAssembliesMatching("*.dll").SelectAllClasses().InheritedFrom<IExtendSyntax>().BindDefaultInterface());

            kernel.Bind<TileManager>().ToSelf().InSingletonScope();

            kernel.Bind<IServiceHost>().To<ServiceHost<Engine>>();
            kernel.Bind<IServiceHost>().To<ServiceHost<TileManager>>();
            kernel.Bind<IServiceHost>().To<ServiceHost<WebServerHost>>();

            kernel.Bind<AsyncHelper>().ToSelf();
            kernel.Bind<SpeechLogger>().ToSelf();

            kernel.Bind<IActionLogger>().To<AgainScenarioDescription>().InSingletonScope();
            kernel.Bind<IActionLogger>().To<WhatScenarioDescription>().InSingletonScope();

            kernel.Bind<IAction>().To<DommyAction>().InSingletonScope()
                .WithPropertyValue("Id", 2)
                .WithPropertyValue("Name", "Dommy")
                ;

            kernel.Bind<IAction>().To<ProgrammeTVAction>().InSingletonScope()
                .WithPropertyValue("Id", 8)
                .WithPropertyValue("Name", "Programme TV")
                ;

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
            engine.IsSimulation = false;

            engine.Init();

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
