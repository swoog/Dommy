using Dommy.Business.Config;
using Dommy.Business.Result;
using Dommy.Business.Scenarios;
using Dommy.Business.Scripts;
using Dommy.Business.Services;
using Dommy.Business.Syntax;
using Dommy.Business.Tools;
using Microsoft.Speech.Recognition;
using Ninject;
using Ninject.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace Dommy.Business
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Engine : IEngine
    {
        public ILogger Logger { get; private set; }

        public SpeechLogger SpeechLogger { get; private set; }

        public ScriptEngine ScriptEngine { get; private set; }

        public Engine(
            IKernel kernel,
            ILogger logger,
            SpeechListener speechListener,
            RestListener restListener,
            UsbUirtListener usbUirtListener,
            SpeechLogger speechLogger,
            ScriptEngine scriptEngine)
        {
            this.Kernel = kernel;
            this.Logger = logger;
            this.SpeechListener = speechListener;
            this.RestListener = restListener;
            this.UsbUirtListener = usbUirtListener;
            this.SpeechLogger = speechLogger;
            this.ScriptEngine = scriptEngine;
        }

        public void Init()
        {
            this.Logger.Info("Execute scripts.");
            this.ScriptEngine.Execute();

            this.Logger.Info("Initializing speech listener");

            this.SpeechListener.Init(this);

            this.Logger.Info("Initializing rest listener");

            try
            {
                this.RestListener.Init();
            }
            catch (Exception ex)
            {
                this.SayError(ex);
            }

            this.Logger.Info("Initializing UsbUirt listener");

            try
            {
                this.UsbUirtListener.Init();
            }
            catch (Exception ex)
            {
                this.SayError(ex);
            }

            this.Logger.Info("Build scenarios");
            try
            {
                Scenario.Build();
            }
            catch (Exception ex)
            {
                this.SayError(ex);
            }

            this.Logger.Info("Initializing scenarios.");
            foreach (var s in this.Kernel.GetAll<IScenario>())
            {
                try
                {
                    this.Logger.Info("Scenario : {0}", s.ScenarioName);
                    s.Init(this);
                }
                catch (Exception ex)
                {
                    this.SayError(ex);
                }
            }

            this.Logger.Info("Start speech listener");

            this.SpeechListener.Start();

            this.SpeechListener.Logs();

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                var ad = ApplicationDeployment.CurrentDeployment;

                this.SpeechLogger.Say(Actor.Dommy, StringHelper.Format(new[] { 
                    "Je suis prête. Version {Version}",
                    "J'attend tes ordres. Version {Version}",
                    "Démaré. Version {Version}",
                }, new { Version = ad.CurrentVersion.ToString() }));
            }
        }

        public void RunResult(IResult result)
        {
            // Execute information of action.
            if (result is PrecisionResult)
            {
                var precision = result as PrecisionResult;

                this.SpeechListener.Precision(precision.SentenceActions, precision.Speech);
            }
            else if (result is SayResult)
            {
                string speech = (result as SayResult).Speech;
                this.SpeechLogger.Say(Actor.Dommy, speech);
            }
        }

        public void SayError(Exception ex)
        {
            var errors = new[]{
                    "Une erreur c'est produite.",
                    "Il y a eu une erreur pendant l'éxécution.",
                    "J'ai identifié une erreur pendant l'éxécution.",
                    "Je n'ai pas réussis a éxécuter l'action.",
                    "Impossible, il y a une erreur.",
                    "Il y a une erreur.",
                    "J'ai eu un problème.",
                    "Je n'y arrive pas."
                };

            this.SpeechLogger.ErrorRecognition(Actor.Dommy, StringHelper.Format(errors));
            this.SpeechLogger.ErrorRecognition(Actor.Dommy, ex.Message);
            this.Logger.Error(ex, "Speech recognition error");
        }

        public void SpeechRecognitionError()
        {
            string sentence = string.Empty;

            if (sentence.StartsWith(this.Name, StringComparison.CurrentCultureIgnoreCase))
            {
                this.Logger.Info("Recognition error : '{0}'", sentence);
            }
        }

        public void Stop()
        {
            this.SpeechListener.Stop();
        }

        public IKernel Kernel { get; private set; }

        public SpeechListener SpeechListener { get; set; }

        public RestListener RestListener { get; set; }

        public string Name { get; set; }

        public UsbUirtListener UsbUirtListener { get; set; }

        public string GetName()
        {
            return this.Name;
        }
    }
}
