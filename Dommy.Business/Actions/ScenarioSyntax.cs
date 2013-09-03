//-----------------------------------------------------------------------
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
    using Ninject;
    using Ninject.Extensions.Logging;
    using Ninject.Parameters;
    using UsbUirt;
    using UsbUirt.Enums;

    /// <summary>
    /// Class of a scenario object.
    /// </summary>
    public class ScenarioSyntax : ITriggerScenarioSyntax, IScenario
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
            if (this.Triggers != null)
            {
                foreach (var t in this.Triggers)
                {
                    t.Init(engine, this);
                }
            }

            this.childs.ForEach(c => c.Init(engine));
        }

        /// <summary>
        /// Gets or sets trigger of this scenario.
        /// </summary>
        public IList<ITrigger> Triggers { get; set; }

        /// <summary>
        /// List of actions to executes.
        /// </summary>
        private List<Func<bool>> actions = new List<Func<bool>>();

        /// <summary>
        /// List of childs scenarios.
        /// </summary>
        private List<IScenario> childs = new List<IScenario>();

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
        public IScenarioSyntax EedomusOnOff(string eedomusId, bool isOn)
        {
            return this.Extend<IEedomusActions>().EedomusOnOff(eedomusId, isOn);
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
        ITriggerScenarioSyntax ITriggerSyntax.Trigger(ITrigger trigger)
        {
            if (this.Triggers == null)
            {
                this.Triggers = new List<ITrigger>();
            }

            this.Triggers.Add(trigger);
            return this;
        }

        /// <summary>
        /// Create scenario syntax with no trigger.
        /// </summary>
        /// <returns>Scenario syntax.</returns>
        public ITriggerScenarioSyntax NoTrigger()
        {
            return this;
        }

        public IScenarioSyntax If(Func<bool> predicate, Func<IScenarioSyntax, IScenarioSyntax> trueScenario, Func<IScenarioSyntax, IScenarioSyntax> falseScenario = null)
        {
            return this.Extend<IIfActions>().If(predicate, trueScenario, falseScenario);
        }

        public ITriggerScenarioSyntax NoPrefixSpeechTrigger(params string[] sentences)
        {
            return this.Extend<ISpeechTriggerSyntax>().NoPrefixSpeechTrigger(sentences);
        }

        public ITriggerScenarioSyntax SpeechTrigger(params string[] sentences)
        {
            return this.Extend<ISpeechTriggerSyntax>().SpeechTrigger(sentences);
        }

        public ITriggerScenarioSyntax SpeechTrigger(double confidence, params string[] sentences)
        {
            return this.Extend<ISpeechTriggerSyntax>().SpeechTrigger(confidence, sentences);
        }

        public ITriggerScenarioSyntax RestTrigger(string p)
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

        public ITriggerScenarioSyntax UsbUirtTrigger(string irCode)
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

        public IScrapActions GetUrl<T>(string url)
        {
            return this.Extend<IUrlActions>().GetUrl<T>(url);
        }

        public ITriggerScenarioSyntax TimeTrigger(DateTime startDate, TimeSpan tick)
        {
            return this.Extend<ITimeTriggerSyntax>().TimeTrigger(startDate, tick);
        }

        public ITriggerScenarioSyntax TileTrigger(string title, System.Drawing.Color backGround)
        {
            return this.Extend<ITileTriggerSyntax>().TileTrigger(title, backGround);
        }

        public ITriggerScenarioSyntax RestTrigger(string p, object data)
        {
            return this.Extend<IRestTriggerSyntax>().RestTrigger(p, data);
        }

        public IScenarioSyntax EedomusCall(EedomusApi api, EedoumusAction action, string eedomusId, string value = null)
        {
            return this.Extend<IEedomusActions>().EedomusCall(api, action, eedomusId, value);
        }

        public IScenarioSyntax EedomusCall(EedoumusAction action, string eedomusId, string value = null)
        {
            return this.Extend<IEedomusActions>().EedomusCall(action, eedomusId, value);
        }

        public ITriggerScenarioSyntax StartupTrigger()
        {
            return this.Extend<IStartupTriggerSyntax>().StartupTrigger();
        }
    }
}
