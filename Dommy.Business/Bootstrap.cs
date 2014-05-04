namespace Dommy.Business
{
    using Dommy.Business.Configs;
    using Dommy.Business.Scenarios;
    using Dommy.Business.Scripts;
    using Dommy.Business.Services;
    using Dommy.Business.Syntax;
    using Dommy.Business.Tools;
    using Dommy.Business.WebHost;
    using Ninject;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime;
    using System.Text;
    using System.Threading.Tasks;

    public static class Bootstrap
    {
        public static void Run()
        {
            ProfileOptimization.SetProfileRoot(@".\");
            ProfileOptimization.StartProfile(String.Format("{0}.profile", AppDomain.CurrentDomain.FriendlyName));

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
            kernel.Load(Directory.GetFiles(directory, "Dommy.*.dll").Where(f => !f.EndsWith(".x86.dll")));

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

            List<IServiceHost> services = OpenServices(kernel);

            var engine = kernel.Get<Engine>();

            engine.Init();

            System.Console.ReadLine();
            engine.Stop();

            CloseServices(services);

            web.Stop();
        }

        private static void CloseServices(List<IServiceHost> services)
        {
            foreach (var item in services)
            {
                item.Close();
            }
        }

        private static List<IServiceHost> OpenServices(StandardKernel kernel)
        {
            var services = kernel.GetAll<IServiceHost>().ToList();

            foreach (var item in services)
            {
                item.Open();
            }

            return services;
        }

        public static void RunX86()
        {
            ProfileOptimization.SetProfileRoot(@".\");
            ProfileOptimization.StartProfile(String.Format("{0}.profile", AppDomain.CurrentDomain.FriendlyName));

            log4net.Config.XmlConfigurator.Configure();

            var kernel = new StandardKernel();
            kernel.Load("Dommy.*.x86.dll");

            List<IServiceHost> services = OpenServices(kernel);

            Console.ReadLine();

            CloseServices(services);
        }
    }
}
