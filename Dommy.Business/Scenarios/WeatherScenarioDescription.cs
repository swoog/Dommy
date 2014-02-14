using Dommy.Business.Actions;
using Dommy.Business.Syntax;
using Dommy.Business.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Dommy.Business.Scenarios
{
    public class WeatherScenarioDescription : IScenarioDescription
    {
        protected EedomusHelper eedomusHelper;

        private AsyncHelper wait;

        public string EedomusRainId { get; set; }

        public string EedomusTemperatureId { get; set; }

        public string EedomusWindId { get; set; }

        public EedomusApi Mode { get; set; }

        private string[] speech = new[]{
            "{Condition}, Il fait {Degre} degré.",
            "{Degre} degré, {Condition} !",
        };

        public WeatherScenarioDescription(AsyncHelper wait, EedomusHelper eedomusHelper)
        {
            this.wait = wait;
            this.eedomusHelper = eedomusHelper;
            this.Mode = EedomusApi.Local;
        }

        private class WeatherData
        {
            public string Sentence { get; set; }
        }

        public void Create()
        {
            var data = new WeatherData();

            Scenario.Create("Météo")
                    .SpeechTrigger(
                        "Quel temps fait il",
                        "La météo s'il te plait",
                        "Est-ce qu'il fait beau",
                        "Donne mois la météo",
                        "La météo")
                    .Action(() =>
                    {
                        var weather = DommyCache.Get("WeatherAction", TimeSpan.FromHours(1), () =>
                        {
                            var temperature = this.eedomusHelper.CallService(this.Mode, EedomusAction.PeriphCaract, this.EedomusTemperatureId);

                            var rain = this.eedomusHelper.CallService(this.Mode, EedomusAction.PeriphCaract, this.EedomusRainId);

                            var wind = this.eedomusHelper.CallService(this.Mode, EedomusAction.PeriphCaract, this.EedomusWindId);

                            return new Weather
                            {
                                Temperature = Convert.ToDouble(temperature, CultureInfo.InvariantCulture.NumberFormat),
                                Rain = Convert.ToDouble(rain, CultureInfo.InvariantCulture.NumberFormat),
                                Wind = Convert.ToDouble(wind, CultureInfo.InvariantCulture.NumberFormat),
                            };
                        });

                        var responses = new List<string>();

                        if (weather.Temperature <= 0)
                        {
                            if (weather.Rain > 0)
                            {
                                responses.Add("Il neige.");
                                responses.Add("Il y à de la neige.");
                            }
                            else
                            {
                                // Il fais très froid
                                responses.Add("Il fais très froid.");
                                responses.Add("Un manteau chaud est de rigueur.");
                                responses.Add("Un temps a ne pas mêtre un chat dehors.");
                            }
                        }
                        else if (weather.Temperature < 20)
                        {
                            if (weather.Rain > 0)
                            {
                                responses.Add("Il fais froid et il pleu.");
                                responses.Add("De la pluit.");
                                responses.Add("Il pleut.");
                            }
                            else
                            {
                                responses.Add("Il fais froid mais il ne pleu pas.");
                                responses.Add("Il fais frais. Pas de pluit.");
                                responses.Add("Il fais frais. Aucune pluit.");
                                responses.Add("Il fais frais.");
                            }
                        }
                        else if (weather.Temperature < 25)
                        {
                            if (weather.Rain > 0)
                            {
                                responses.Add("Il pleu.");
                            }
                            else
                            {
                                responses.Add("Temps idéal.");
                                responses.Add("Temps idéal. Aucune pluit");
                                responses.Add("Temps idéal. Pas de pluit");
                                responses.Add("Ni chaud, ni frois. Aucune pluit");
                                responses.Add("Ni chaud, ni frois. Pas de pluit");
                            }
                        }
                        else
                        {
                            if (weather.Rain > 0)
                            {
                                responses.Add("Il pleu, mais il fait très chaud.");
                                responses.Add("La chaleur et la plui.");
                            }
                            else
                            {
                                responses.Add("Chaleur étoufante.");
                                responses.Add("Il fait très chaud.");
                                responses.Add("Pas besoin de veste. Il fait très chaud.");
                            }
                        }

                        var temperatures = new[] 
                        { 
                            "{Temperature} degré.", 
                            "Température de {Temperature} degré."
                        };

                        data.Sentence = StringHelper.Format(temperatures, new { Temperature = (int)weather.Temperature }) + " " + StringHelper.Format(responses);

                        return true;
                    })
                    .Say(data, "{Sentence}")
                    .Start();
        }
    }
}
