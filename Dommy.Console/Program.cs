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

namespace Dommy.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            var kernel = new StandardKernel();
            Configure.InitKernel(kernel);
            Scenario.InitKernel(kernel);

            kernel.Bind(a => a.FromAssembliesMatching("*.dll").SelectAllClasses().InheritedFrom<IScriptEngine>().BindSingleInterface());

            kernel.Bind<ScriptEngine>().ToSelf()
                .WithPropertyValue("ScriptDirectory", @"c:\DommyScripts\")
                ;

            Configure.Engine("Dommy");

            Configure.TextToSpeech()
                .Gender(Gender.Female)
                .Culture("fr-FR");

            Configure.SpeechToText()
                .Culture("fr-FR")
                .Confidence(0.6)
                ;

            Configure.RestListener()
                .Port(5555)
                ;

            Configure.Build();

            // Scripting configuration

            kernel.Bind(a => a.FromAssembliesMatching("*.dll").SelectAllClasses().InheritedFrom<IExtendSyntax>().BindDefaultInterface());

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
            var engine = kernel.Get<Engine>();
            engine.IsSimulation = false;

            engine.Init();

            System.Console.ReadLine();

            //actionService.Close();
            engine.Stop();
        }
    }
}
