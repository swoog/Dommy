using Dommy.Business.Scenarios;
using Dommy.Business.Syntax;
using Dommy.Business.Triggers;
using Dommy.Business.Tools;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dommy.Business.Scenarios
{
    public class UpdateScenarioDescription : IScenarioDescription
    {
        private ISpeechLogger speechLogger;

        public UpdateScenarioDescription(ISpeechLogger speechLogger)
        {
            this.speechLogger = speechLogger;
        }

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
                                this.speechLogger.Say(Actor.Dommy, String.Format("La connection réseau n'est pas disponible. {0}", ex.Message));
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
                                this.speechLogger.Say(Actor.Dommy, StringHelper.Format(new[]
                    {
                        "Mise à jour encours",
                        "Je me met à jour",
                        "Il y a une mise à jour",
                        "Je le fais",
                        "C'est encours",
                    }));
                                ad.Update();
                                this.speechLogger.Say(Actor.Dommy, StringHelper.Format(new[]
                    {
                        "Je redémarre",
                        "A toute",
                        "Près dans quelques secondes",
                    }));
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
