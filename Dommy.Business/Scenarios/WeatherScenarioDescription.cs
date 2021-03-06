//-----------------------------------------------------------------------
// <copyright file="WeatherScenarioDescription.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Scenarios
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Dommy.Business.Actions;
    using Dommy.Business.Syntax;
    using Dommy.Business.Tools;

    /// <summary>
    /// Describe scenarios for speak the weather.
    /// </summary>
    public class WeatherScenarioDescription : IScenarioDescription
    {
        /// <summary>
        /// Eedomus helper.
        /// </summary>
        private EedomusHelper eedomusHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherScenarioDescription"/> class.
        /// </summary>
        /// <param name="eedomusHelper">Eedomus helper.</param>
        public WeatherScenarioDescription(EedomusHelper eedomusHelper)
        {
            this.eedomusHelper = eedomusHelper;
            this.Mode = EedomusApi.Local;
        }

        /// <summary>
        /// Gets or sets Eedomus rain API id.
        /// </summary>
        public string EedomusRainId { get; set; }

        /// <summary>
        /// Gets or sets Eedomus temperature API id.
        /// </summary>
        public string EedomusTemperatureId { get; set; }

        /// <summary>
        /// Gets or sets Eedomus wind API id.
        /// </summary>
        public string EedomusWindId { get; set; }

        /// <summary>
        /// Gets or sets Eedomus mode.
        /// </summary>
        public EedomusApi Mode { get; set; }

        /// <summary>
        /// Create scenarios.
        /// </summary>
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
                        var weather = DommyCache.Get(
                            "WeatherAction",
                            TimeSpan.FromHours(1),
                            () =>
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

        /// <summary>
        /// Weather data class used to store the sentence to say.
        /// </summary>
        private class WeatherData
        {
            /// <summary>
            /// Gets or sets sentence to say.
            /// </summary>
            public string Sentence { get; set; }
        }
    }
}
