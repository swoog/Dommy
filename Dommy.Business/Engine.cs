//-----------------------------------------------------------------------
// <copyright file="Engine.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business
{
    using System;
    using System.Collections.Generic;
    using System.Deployment.Application;
    using System.Linq;
    using System.ServiceModel;
    using Dommy.Business.Result;
    using Dommy.Business.Scenarios;
    using Dommy.Business.Scripts;
    using Dommy.Business.Services;
    using Dommy.Business.Syntax;
    using Dommy.Business.Tools;
    using Ninject;
    using Ninject.Extensions.Logging;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Dommy engine.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Engine : IEngine
    {
        /// <summary>
        /// Engine logger.
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// Speech logger.
        /// </summary>
        private ISpeechLogger speechLogger;

        /// <summary>
        /// Script engine.
        /// </summary>
        private ScriptEngine scriptEngine;

        /// <summary>
        /// All listeners.
        /// </summary>
        private IList<IListener> listeners;

        /// <summary>
        /// Ninject kernel.
        /// </summary>
        private IKernel kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="Engine"/> class.
        /// </summary>
        /// <param name="kernel">Ninject kernel.</param>
        /// <param name="logger">Information logger.</param>
        /// <param name="speechLogger">Speech logger.</param>
        /// <param name="scriptEngine">Script engine.</param>
        /// <param name="listeners">All listeners.</param>
        public Engine(
            string name,
            IKernel kernel,
            ILogger logger,
            ISpeechLogger speechLogger,
            ScriptEngine scriptEngine,
            IList<IListener> listeners)
        {
            this.Name = name;
            this.kernel = kernel;
            this.logger = logger;
            this.speechLogger = speechLogger;
            this.scriptEngine = scriptEngine;
            this.listeners = listeners.OrderBy(this.OrderListener).ToArray();
        }

        private int OrderListener(IListener arg)
        {
            var att = arg.GetType().GetCustomAttributes(typeof(OrderAttribute), true);

            if (att.Length != 0)
            {
                return ((OrderAttribute)att[0]).Order;
            }
            else
            {
                return 100;
            }
        }

        /// <summary>
        /// Gets name of engine.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets name of engine.
        /// </summary>
        /// <returns>Return name of the engine.</returns>
        public string GetEngineName()
        {
            return this.Name;
        }

        /// <summary>
        /// Initialize engine.
        /// </summary>
        public void Init()
        {
            AppDomain.CurrentDomain.UnhandledException += this.UnhandledException;

            this.logger.Info("Execute scripts.");
            this.scriptEngine.Execute();

            foreach (var listener in this.listeners)
            {
                this.logger.Info("Initializing {0}", listener.GetType());

                try
                {
                    listener.Init(this);
                }
                catch (Exception ex)
                {
                    this.SayError(ex);
                }
            }

            this.logger.Info("Build scenarios");
            try
            {
                Scenario.Build();
            }
            catch (Exception ex)
            {
                this.SayError(ex);
            }

            this.logger.Info("Initializing scenarios.");
            foreach (var s in this.kernel.GetAll<IScenario>())
            {
                try
                {
                    this.logger.Info("Scenario : {0}", s.ScenarioName);
                    s.Init(this);
                }
                catch (Exception ex)
                {
                    this.SayError(ex);
                }
            }

            foreach (var listener in this.listeners)
            {
                this.logger.Info("Start {0}", listener.GetType());

                try
                {
                    listener.Start();
                }
                catch (Exception ex)
                {
                    this.SayError(ex);
                }
            }

            this.Listener<SpeechListener>().Logs();

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                var ad = ApplicationDeployment.CurrentDeployment;

                this.speechLogger.Say(
                    Actor.Dommy,
                    StringHelper.Format(
                        new[] 
                        {
                            "Je suis prête. Version {Version}",
                            "J'attend tes ordres. Version {Version}",
                            "Démaré. Version {Version}",
                        },
                        new
                        {
                            Version = ad.CurrentVersion.ToString()
                        }));
            }
        }

        /// <summary>
        /// Run result.
        /// </summary>
        /// <param name="result">Run an implementation of IResult.</param>
        public void RunResult(IResult result)
        {
            // Execute information of action.
            var precisionResult = result as PrecisionResult;
            if (precisionResult != null)
            {
                this.Listener<SpeechListener>().Precision(precisionResult.SentenceActions, precisionResult.Speech);
                return;
            }

            var sayResult = result as SayResult;
            if (sayResult != null)
            {
                string speech = sayResult.Speech;
                this.speechLogger.Say(Actor.Dommy, speech);
                return;
            }
        }

        /// <summary>
        /// Say an error.
        /// </summary>
        /// <param name="ex">Exception of the error.</param>
        public void SayError(Exception ex)
        {
            Contract.Requires(ex != null);

            var errors = new[]
            {
                "Une erreur c'est produite.",
                "Il y a eu une erreur pendant l'éxécution.",
                "J'ai identifié une erreur pendant l'éxécution.",
                "Je n'ai pas réussis a éxécuter l'action.",
                "Impossible, il y a une erreur.",
                "Il y a une erreur.",
                "J'ai eu un problème.",
                "Je n'y arrive pas."
            };

            this.speechLogger.ErrorRecognition(Actor.Dommy, StringHelper.Format(errors));
            this.speechLogger.ErrorRecognition(Actor.Dommy, ex.Message);
            this.logger.Error(ex, "Speech recognition error");
        }

        /// <summary>
        /// Speech recognition error.
        /// </summary>
        public void SpeechRecognitionError()
        {
            string sentence = string.Empty;

            if (sentence.StartsWith(this.Name, StringComparison.CurrentCultureIgnoreCase))
            {
                this.logger.Info("Recognition error : '{0}'", sentence);
            }
        }

        /// <summary>
        /// Stop engine.
        /// </summary>
        public void Stop()
        {
            foreach (var listener in this.listeners)
            {
                this.logger.Info("Stop {0}", listener.GetType());

                try
                {
                    listener.Stop();
                }
                catch (Exception ex)
                {
                    this.SayError(ex);
                }
            }
        }

        /// <summary>
        /// Find instance of the listener.
        /// </summary>
        /// <typeparam name="T">Listener Type.</typeparam>
        /// <returns>Listener instance.</returns>
        public T Listener<T>()
            where T : IListener
        {
            return this.listeners.OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Match all unhandled exception.
        /// </summary>
        /// <param name="sender">Unhandled exception sender.</param>
        /// <param name="e">Event exception.</param>
        private void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            this.SayError(e.ExceptionObject as Exception);
        }
    }
}
