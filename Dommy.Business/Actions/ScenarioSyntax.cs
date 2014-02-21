//-----------------------------------------------------------------------
// <copyright file="ScenarioSyntax.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Actions
{
    using Dommy.Business.Result;
    using Dommy.Business.Scenarios;
    using Dommy.Business.Syntax;
    using Dommy.Business.Tools;
    using Dommy.Business.Triggers;
    using Ninject;
    using Ninject.Extensions.Logging;
    using Ninject.Parameters;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using UsbUirt;
    using UsbUirt.Enums;

    /// <summary>
    /// Class of a scenario object.
    /// </summary>
    public class ScenarioSyntax : ITriggerScenarioSyntax, IScenario
    {
        /// <summary>
        /// Instance of AsyncHelper.
        /// </summary>
        private AsyncHelper asyncHelper;

        /// <summary>
        /// Instance of Eedomus helper.
        /// </summary>
        private EedomusHelper eedomusHelper;

        /// <summary>
        /// Information logger.
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// List of actions to executes.
        /// </summary>
        private List<Func<bool>> actions = new List<Func<bool>>();

        /// <summary>
        /// List of childs scenarios.
        /// </summary>
        private List<IScenario> childs = new List<IScenario>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ScenarioSyntax"/> class.
        /// </summary>
        /// <param name="name">Name of scenario.</param>
        /// <param name="kernel">Ninject kernel.</param>
        /// <param name="logger">Information logger.</param>
        /// <param name="eedomusHelper">Eedomus helper.</param>
        /// <param name="asyncHelper">Asynchronous helper.</param>
        public ScenarioSyntax(string name, IKernel kernel, ILogger logger, EedomusHelper eedomusHelper, AsyncHelper asyncHelper)
        {
            this.ScenarioName = name;
            this.Kernel = kernel;
            this.logger = logger;
            this.eedomusHelper = eedomusHelper;
            this.asyncHelper = asyncHelper;
        }

        /// <summary>
        /// Gets or sets trigger of this scenario.
        /// </summary>
        public IList<ITrigger> Triggers { get; set; }

        /// <summary>
        /// Gets core engine.
        /// </summary>
        public Engine Engine { get; private set; }

        /// <summary>
        /// Gets Ninject kernel.
        /// </summary>
        public IKernel Kernel { get; private set; }

        /// <summary>
        /// Gets scenario name.
        /// </summary>
        public string ScenarioName { get; private set; }

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
        /// <param name="infraRedCode">infra red ode to send.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax UsbUirt(string infraRedCode)
        {
            this.actions.Add(() =>
            {
                using (var driver = new Driver())
                {
                    using (var transmitter = new Transmitter(driver))
                    {
                        transmitter.Transmit(infraRedCode, emitter: Emitter.Internal);
                    }

                    return true;
                }
            });

            return this;
        }

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
                this.Engine.RunResult(new PrecisionResult(
                    StringHelper.Format(sentences),
                    new List<PrecisionResult.SentenceAction>
                {
                        new PrecisionResult.SentenceAction()
                        {
                            Sentences = response, Action = s =>
                {
                    scenario(s.Text, ss).ToScenario().Run();
                    return new NoneResult();
                            }
                        }
                    }));

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
        public ITriggerScenarioSyntax Trigger(ITrigger trigger)
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

        /// <summary>
        /// If syntax.
        /// </summary>
        /// <param name="predicate">Predicate of if.</param>
        /// <param name="trueScenario">Scenario when predicate is true.</param>
        /// <param name="falseScenario">Scenario when predicate is false.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax If(Func<bool> predicate, Func<IScenarioSyntax, IScenarioSyntax> trueScenario, Func<IScenarioSyntax, IScenarioSyntax> falseScenario = null)
        {
            return this.Extend<IIfActions>().If(predicate, trueScenario, falseScenario);
        }

        /// <summary>
        /// Create scenario with speech trigger.
        /// </summary>
        /// <param name="sentences">All trigger sentences without prefix.</param>
        /// <returns>Scenario trigger syntax.</returns>
        public ITriggerScenarioSyntax NoPrefixSpeechTrigger(params string[] sentences)
        {
            return this.Extend<ISpeechTriggerSyntax>().NoPrefixSpeechTrigger(sentences);
        }

        /// <summary>
        /// Create scenario with speech trigger.
        /// </summary>
        /// <param name="sentences">All trigger sentences with prefix.</param>
        /// <returns>Scenario trigger syntax.</returns>
        public ITriggerScenarioSyntax SpeechTrigger(params string[] sentences)
        {
            return this.Extend<ISpeechTriggerSyntax>().SpeechTrigger(sentences);
        }

        /// <summary>
        /// Create scenario with speech trigger.
        /// </summary>
        /// <param name="confidence">Acceptable confidence.</param>
        /// <param name="sentences">All trigger sentences with prefix.</param>
        /// <returns>Scenario trigger syntax.</returns>
        public ITriggerScenarioSyntax SpeechTrigger(double confidence, params string[] sentences)
        {
            return this.Extend<ISpeechTriggerSyntax>().SpeechTrigger(confidence, sentences);
        }

        /// <summary>
        /// Create scenario with REST trigger.
        /// </summary>
        /// <param name="p">Relative uri trigger.</param>
        /// <returns>Scenario trigger syntax.</returns>
        public ITriggerScenarioSyntax RestTrigger(string p)
        {
            return this.Extend<IRestTriggerSyntax>().RestTrigger(p);
        }

        /// <summary>
        /// Add child scenario.
        /// </summary>
        /// <param name="scenario">Child scenario.</param>
        public void AddChild(IScenario scenario)
        {
            this.childs.Add(scenario);
        }

        /// <summary>
        /// Ping conditional syntax.
        /// </summary>
        /// <param name="ip">IP to ping.</param>
        /// <param name="b">Status excepted.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax IfPing(string ip, bool b)
        {
            return this.Extend<IPingActions>().IfPing(ip, b);
        }

        /// <summary>
        /// Execute command on TV.
        /// </summary>
        /// <param name="command">Command to execute.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax TvCommand(TvCommand command)
        {
            return this.Extend<ITvActions>().TvCommand(command);
        }

        /// <summary>
        /// Change sound on TV.
        /// </summary>
        /// <param name="quantity">Quantity to change volume.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax TvSound(int quantity)
        {
            return this.Extend<ITvActions>().TvSound(quantity);
        }

        /// <summary>
        /// Mute on TV.
        /// </summary>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax TvMute()
        {
            return this.Extend<ITvActions>().TvMute();
        }

        /// <summary>
        /// Change canal on TV.
        /// </summary>
        /// <param name="canal">Canal to change.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax TvCanal(int canal)
        {
            return this.Extend<ITvActions>().TvCanal(canal);
        }

        /// <summary>
        /// Implementation of say sentences alternatively.
        /// </summary>
        /// <param name="sentences">Sentences to say.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax Say(params string[] sentences)
        {
            return this.Extend<ISayActions>().Say(sentences);
        }

        /// <summary>
        /// Implementation of say sentences alternatively with context object. See <see cref="StringHelper"/> class for more details.
        /// </summary>
        /// <param name="data">Data object to use for replacement.</param>
        /// <param name="sentences">Sentences to say.</param>
        /// <returns>Scenario syntax.</returns>
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

        /// <summary>
        /// Create scenario based on infra red code returned by USB-UIRT module.
        /// </summary>
        /// <param name="infraRedCode">Infra red code.</param>
        /// <returns>Trigger scenario syntax.</returns>
        public ITriggerScenarioSyntax UsbUirtTrigger(string infraRedCode)
        {
            return this.Extend<IUsbUirtTriggerSyntax>().UsbUirtTrigger(infraRedCode);
        }

        /// <summary>
        /// Log to the application log file.
        /// </summary>
        /// <param name="format">Format of the log.</param>
        /// <param name="obj">Object used to get property format.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax Log(string format, object obj)
        {
            this.Action(() =>
            {
                this.logger.Info(StringHelper.Format(format, obj));
                return true;
            });

            return this;
        }

        /// <summary>
        /// Get a scrapper for url.
        /// </summary>
        /// <typeparam name="T">Type of object to create when scrap.</typeparam>
        /// <param name="url">Url to scrap.</param>
        /// <returns>Scrapper action.</returns>
        public IScrapActions GetUrl<T>(string url)
        {
            return this.Extend<IUrlActions>().GetUrl<T>(url);
        }

        /// <summary>
        /// Create scenario based on a TimeSpan.
        /// </summary>
        /// <param name="startDate">Start date of the trigger.</param>
        /// <param name="tick">Delay to execute trigger.</param>
        /// <returns>Trigger scenario syntax.</returns>
        public ITriggerScenarioSyntax TimeTrigger(DateTime startDate, TimeSpan tick)
        {
            return this.Extend<ITimeTriggerSyntax>().TimeTrigger(startDate, tick);
        }

        /// <summary>
        /// Create a tile on the web interface used to start scenario.
        /// </summary>
        /// <param name="title">Tile name.</param>
        /// <param name="backGround">Background color of the tile.</param>
        /// <returns>Trigger scenario syntax.</returns>
        public ITriggerScenarioSyntax TileTrigger(string title, System.Drawing.Color backGround)
        {
            return this.Extend<ITileTriggerSyntax>().TileTrigger(title, backGround);
        }

        /// <summary>
        /// Create a REST trigger used to start scenario.
        /// </summary>
        /// <param name="url">Url used to start scenario.</param>
        /// <param name="data">Object used to map the url. Example : /hello/{MyName} and MyName is property of data.</param>
        /// <returns>Trigger scenario syntax.</returns>
        public ITriggerScenarioSyntax RestTrigger(string url, object data)
        {
            return this.Extend<IRestTriggerSyntax>().RestTrigger(url, data);
        }

        /// <summary>
        /// Call API eedomus.
        /// </summary>
        /// <param name="api">Eedomus API type. (Local or distant)</param>
        /// <param name="action">Action to execute.</param>
        /// <param name="eedomusId">Eedomus id element.</param>
        /// <param name="value">Optional value to set.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax EedomusCall(EedomusApi api, EedomusAction action, string eedomusId, string value = null)
        {
            return this.Extend<IEedomusActions>().EedomusCall(api, action, eedomusId, value);
        }

        /// <summary>
        /// Call local API eedomus.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        /// <param name="eedomusId">Eedomus id element.</param>
        /// <param name="value">Optional value to set.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax EedomusCall(EedomusAction action, string eedomusId, string value = null)
        {
            return this.Extend<IEedomusActions>().EedomusCall(action, eedomusId, value);
        }

        /// <summary>
        /// Create a scenario started a begin of application.
        /// </summary>
        /// <returns>Trigger scenario syntax.</returns>
        public ITriggerScenarioSyntax StartupTrigger()
        {
            return this.Extend<IStartupTriggerSyntax>().StartupTrigger();
        }

        /// <summary>
        /// Start scenario asynchronously.
        /// </summary>
        /// <returns>Return Task to wait on this.</returns>
        public Task RunAsync()
        {
            return Task.Run(() =>
            {
                try
                {
                    this.asyncHelper.Wait(() =>
                    {
                        this.Run();
                    });
                }
                catch (Exception ex)
                {
                    this.Engine.SayError(ex);
                }
            });
        }

        /// <summary>
        /// Set value to local API eedomus.
        /// </summary>
        /// <param name="id">Eedomus id element.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax EedomusValue(string id, double value)
        {
            return this.Extend<IEedomusActions>().EedomusValue(id, value);
        }

        /// <summary>
        /// Add file to synology download manager.
        /// </summary>
        /// <param name="server">Server name with port.</param>
        /// <param name="username">Username used for connect to synology.</param>
        /// <param name="password">Password used for connect to synology.</param>
        /// <param name="file">Url file to download (EDK2, HTTP, ...)</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax SynologyDownloadCreate(string server, string username, string password, string file)
        {
            return this.Extend<ISynologyDownloadActions>().SynologyDownloadCreate(server, username, password, file);
        }

        /// <summary>
        /// Add file to synology download manager.
        /// </summary>
        /// <param name="server">Server name with port.</param>
        /// <param name="username">Username used for connect to synology.</param>
        /// <param name="password">Password used for connect to synology.</param>
        /// <param name="data">Data used to create file url.</param>
        /// <param name="file">Format url file to download (EDK2, HTTP, ...)</param>
        /// <returns>Scenario syntax.</returns>
        public IScenarioSyntax SynologyDownloadCreate(string server, string username, string password, object data, string file)
        {
            return this.Extend<ISynologyDownloadActions>().SynologyDownloadCreate(server, username, password, data, file);
        }

        /// <summary>
        /// Get the implement instance of the interface syntax.
        /// </summary>
        /// <typeparam name="T">Interface syntax.</typeparam>
        /// <returns>Implemented instance.</returns>
        private T Extend<T>()
        {
            return this.Kernel.Get<T>(new ConstructorArgument("scenario", this));
        }

        public ITriggerScenarioSyntax TileTrigger(string title, System.Drawing.Color backgroundColor, string url)
        {
            return this.Extend<ITileTriggerSyntax>().TileTrigger(title, backgroundColor, url);
        }

        public ITriggerScenarioSyntax TileTrigger(out Tile tile, string title, System.Drawing.Color backgroundColor)
        {
            return this.Extend<ITileTriggerSyntax>().TileTrigger(out tile, title, backgroundColor);
        }

        public ITriggerScenarioSyntax TileTrigger(out Tile tile, string title, System.Drawing.Color backgroundColor, string url)
        {
            return this.Extend<ITileTriggerSyntax>().TileTrigger(out tile, title, backgroundColor, url);
        }

        public IScenarioSyntax TileUpdate(Tile tile)
        {
            return this.Extend<ITileActions>().TileUpdate(tile);
        }
    }
}