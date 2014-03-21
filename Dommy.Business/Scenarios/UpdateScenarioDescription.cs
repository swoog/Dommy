//-----------------------------------------------------------------------
// <copyright file="UpdateScenarioDescription.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Scenarios
{
using System;
using System.Deployment.Application;
    using System.Globalization;
using System.Windows.Forms;
    using Dommy.Business.Syntax;
    using Dommy.Business.Tools;

    /// <summary>
    /// Initialize scenario to automatic update dommy.
    /// </summary>
    public class UpdateScenarioDescription : IScenarioDescription
    {
        /// <summary>
        /// Speech logger.
        /// </summary>
        private ISpeechLogger speechLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateScenarioDescription"/> class.
        /// </summary>
        /// <param name="speechLogger">Speech logger.</param>
        public UpdateScenarioDescription(ISpeechLogger speechLogger)
        {
            this.speechLogger = speechLogger;
        }

        /// <summary>
        /// Create scenario.
        /// </summary>
        public void Create()
        {
            Scenario.Create("Mise à jour")
                .SpeechTrigger(
                    "met toi à jour",
                    "met à jour",
                    "mise à jour",
                    "update")
                    .Action(() =>
                    {
                        UpdateCheckInfo info = null;

                        if (ApplicationDeployment.IsNetworkDeployed)
                        {
                            ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

                            try
                            {
                                info = ad.CheckForDetailedUpdate();
                            }
                            catch (DeploymentDownloadException ex)
                            {
                                this.speechLogger.Say(Actor.Dommy, string.Format(CultureInfo.InvariantCulture, "La connection réseau n'est pas disponible. {0}", ex.Message));
                                return false;
                            }
                            catch (InvalidDeploymentException)
                            {
                                this.speechLogger.Say(Actor.Dommy, "Le déploiement clickonce est corompu");
                                return false;
                            }
                            catch (InvalidOperationException)
                            {
                                this.speechLogger.Say(Actor.Dommy, "Impossible de me mettre à jour");
                                return false;
                            }

                            if (info.UpdateAvailable)
                            {
                                var saySentences = new[]
                    {
                        "Mise à jour encours",
                        "Je me met à jour",
                        "Il y a une mise à jour",
                        "Je le fais",
                        "C'est encours",
                                        };

                                this.speechLogger.Say(
                                    Actor.Dommy, 
                                    StringHelper.Format(saySentences));
                                ad.Update();
                                var restartSaySentences = new[]
                    {
                        "Je redémarre",
                        "A toute",
                        "Près dans quelques secondes",
                                };

                                this.speechLogger.Say(Actor.Dommy, StringHelper.Format(restartSaySentences));
                                Application.Restart();
                                Environment.Exit(0);
                                this.speechLogger.Say(Actor.Dommy, "Redémarage non exécuté.");
                                return false;
                            }
                        }

                        this.speechLogger.Say(Actor.Dommy, "Je suis à jour.");
                        return true;
                    })
                    .Start();
        }
    }
}
