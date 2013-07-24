﻿//-----------------------------------------------------------------------
// <copyright file="ScenarioSyntax.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Actions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Dommy.Business.Result;
    using Dommy.Business.Scenarios;
    using Dommy.Business.Syntax;
    using Dommy.Business.Tools;
    using Dommy.Business.Triggers;
    using Dommy.Model;
    using Ninject;
    using Ninject.Extensions.Logging;
    using Ninject.Parameters;
    using UsbUirt;
    using UsbUirt.Enums;

    /// <summary>
    /// Class of a scenario object.
    /// </summary>
    public class ScenarioSyntax : IScenarioSyntax, ITriggerSyntax, IScenario
    {
        /// <summary>
        /// Instance of Eedomus helper.
        /// </summary>
        private EedomusHelper eedomusHelper;

        /// <summary>
        /// Information logger.
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScenarioSyntax"/> class.
        /// </summary>
        /// <param name="name">Name of scenario.</param>
        /// <param name="kernel">Ninject kernel.</param>
        /// <param name="logger">Information logger.</param>
        public ScenarioSyntax(string name, IKernel kernel, ILogger logger, EedomusHelper eedomusHelper)
        {
            this.ScenarioName = name;
            this.Kernel = kernel;
            this.logger = logger;
            this.eedomusHelper = eedomusHelper;
        }

        /// <summary>
        /// Gets core engine.
        /// </summary>
        public Engine Engine { get; private set; }

        /// <summary>
        /// Gets Ninject kernel.
        /// </summary>
        public IKernel Kernel { get; private set; }

        /// <summary>
        /// Initialize scenario with the instance of engine.
        /// </summary>
        /// <param name="engine">Core engine to use with this scenario.</param>
        public void Init(Engine engine)
        {
            this.Engine = engine;
            if (this.Trigger != null)
            {
                this.Trigger.Init(engine, this);
            }

            this.childs.ForEach(c => c.Init(engine));
        }

        /// <summary>
        /// Gets or sets trigger of this scenario.
        /// </summary>
        public ITrigger Trigger { get; set; }

        /// <summary>
        /// List of actions to executes.
        /// </summary>
        private List<Func<bool>> actions = new List<Func<bool>>();

        /// <summary>
        /// List of childs scenarios.
        /// </summary>
        private List<IScenario> childs = new List<IScenario>();

        /// <summary>
        /// Add an action.
        /// </summary>
        /// <param name="actionData">Action data object to use.</param>
        /// <returns>Scenario syntax.</returns>
        [ObsoleteAttribute]
        public IScenarioSyntax Action(Model.ActionData actionData)
        {
            this.actions.Add(() =>
            {
                this.Engine.Run(actionData, false);
                return true;
            });

            return this;
        }

        /// <summary>
        /// Return an instance of scenario.
        /// </summary>
        /// <returns>Object scenario</returns>
        public IScenario ToScenario()
        {
            return this;
        }

        /// <summary>
        /// Run scenario.
        /// </summary>
        public void Run()
        {
            this.logger.Info("Start scenario {0}", this.ScenarioName);

            var b = true;

            int i = 0;

            while (b && i < this.actions.Count)
            {
                b = this.actions[i]();

                i++;
            }

            this.logger.Debug("End scenario {0}", this.ScenarioName);
        }

        /// <summary>
        /// Implementation to on off light.
        /// </summary>
        /// <param name="eedomusId">eedomus id.</param>
        /// <param name="isOn">On or off the light.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax ActionOnOffLight(string eedomusId, bool isOn)
        {
            return Action(()=>
                {
                    this.eedomusHelper.CallService(EedomusHelper.EedoumusAction.PeriphValue, eedomusId, isOn ? "100" :"0");
                    return true;
                });
        }

        /// <summary>
        /// Send infra red code on USB-UIRT device.
        /// </summary>
        /// <param name="irCode">infra red ode to send.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax UsbUirt(string irCode)
        {
            this.actions.Add(() =>
            {
                using (var driver = new Driver())
                {
                    var transmitter = new Transmitter(driver);
                    transmitter.Transmit(irCode, emitter: Emitter.Internal);

                    return true;
                }
            });

            return this;
        }

        /// <summary>
        /// Gets scenario name.
        /// </summary>
        public string ScenarioName { get; private set; }

        /// <summary>
        /// Generic lambda action.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax Action(Func<bool> action)
        {
            this.actions.Add(action);

            return this;
        }

        /// <summary>
        /// Call precision and execute scenario when precision is say.
        /// </summary>
        /// <param name="sentences">Sentences say by engine.</param>
        /// <param name="response">Expected response.</param>
        /// <param name="scenario">Scenario to execute when expected response.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax Precision(string[] sentences, string[] response, Func<string, IScenarioSyntax, IScenarioSyntax> scenario)
        {
            var ss = Scenario.Create().NoTrigger();
            this.actions.Add(() =>
            {
                this.Engine.RunResult(new PrecisionResult(StringHelper.Format(sentences), new List<PrecisionResult.SentenceAction>{ new PrecisionResult.SentenceAction(){ Sentences = response, Action = s => 
                {
                    scenario(s.Text, ss).ToScenario().Run();
                    return new NoneResult();
                }}}));

                return true;
            });

            return this;
        }

        /// <summary>
        /// Execute an external process.
        /// </summary>
        /// <param name="command">Command name.</param>
        /// <param name="parameters">Parameters of command.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax Command(string command, string parameters)
        {
            this.actions.Add(() =>
            {
                Process.Start(command, parameters);
                return true;
            });

            return this;
        }

        /// <summary>
        /// Create scenario syntax with trigger.
        /// </summary>
        /// <param name="trigger">Trigger to add.</param>
        /// <returns>Scenario syntax.</returns>
        IScenarioSyntax ITriggerSyntax.Trigger(ITrigger trigger)
        {
            this.Trigger = trigger;
            return this;
        }

        /// <summary>
        /// Create scenario syntax with no trigger.
        /// </summary>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax NoTrigger()
        {
            return this;
        }

        public IScenarioSyntax If(Func<bool> predicate, Func<IScenarioSyntax, IScenarioSyntax> trueScenario, Func<IScenarioSyntax, IScenarioSyntax> falseScenario = null)
        {
            return this.Extend<IIfActions>().If(predicate, trueScenario, falseScenario);
        }

        public IScenarioSyntax NoPrefixSpeechTrigger(params string[] sentences)
        {
            return this.Extend<ISpeechTriggerSyntax>().NoPrefixSpeechTrigger(sentences);
        }

        public IScenarioSyntax SpeechTrigger(params string[] sentences)
        {
            return this.Extend<ISpeechTriggerSyntax>().SpeechTrigger(sentences);
        }

        public IScenarioSyntax SpeechTrigger(double confidence, params string[] sentences)
        {
            return this.Extend<ISpeechTriggerSyntax>().SpeechTrigger(confidence, sentences);
        }

        public IScenarioSyntax RestTrigger(string p)
        {
            return this.Extend<IRestTriggerSyntax>().RestTrigger(p);
        }

        public void AddChild(IScenario scenario)
        {
            this.childs.Add(scenario);
        }

        public IScenarioSyntax IfPing(string ip, bool b)
        {
            return this.Extend<IPingActions>().IfPing(ip, b);
        }

        public IScenarioSyntax TvCommand(TvCommand command)
        {
            return this.Extend<ITvActions>().TvCommand(command);
        }

        public IScenarioSyntax TvSound(int quantity)
        {
            return this.Extend<ITvActions>().TvSound(quantity);
        }

        public IScenarioSyntax TvMute()
        {
            return this.Extend<ITvActions>().TvMute();
        }

        public IScenarioSyntax TvCanal(int canal)
        {
            return this.Extend<ITvActions>().TvCanal(canal);
        }

        public IScenarioSyntax Say(params string[] sentences)
        {
            return this.Extend<ISayActions>().Say(sentences);
        }

        public IScenarioSyntax Say(object data, params string[] sentences)
        {
            return this.Extend<ISayActions>().Say(data, sentences);
        }

        /// <summary>
        /// Add scenario to execution context.
        /// </summary>
        public void Start()
        {
            this.Kernel.Bind<IScenario>().ToMethod(c => this);
        }

        private T Extend<T>()
        {
            return this.Kernel.Get<T>(new ConstructorArgument("scenario", this));
        }

        public IScenarioSyntax UsbUirtTrigger(string irCode)
        {
            return this.Extend<IUsbUirtTriggerSyntax>().UsbUirtTrigger(irCode);
        }

        public IScenarioSyntax Log(string format, object obj)
        {
            this.Action(() =>
            {
                this.logger.Info(StringHelper.Format(format, obj));
                return true;
            });

            return this;
        }

        public IScrapActions GetUrl(string url)
        {
            return this.Extend<IUrlActions>().GetUrl(url);
        }
    }
}
