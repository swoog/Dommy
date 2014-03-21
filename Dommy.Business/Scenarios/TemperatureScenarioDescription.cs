//-----------------------------------------------------------------------
// <copyright file="TemperatureScenarioDescription.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Scenarios
{
    using System;
    using System.Globalization;
    using Dommy.Business.Actions;
    using Dommy.Business.Syntax;
    using Dommy.Business.Tools;

    /// <summary>
    /// Scenario description for say eedomus temperature.
    /// </summary>
    public class TemperatureScenarioDescription : IScenarioDescription
    {
        /// <summary>
        /// Instance of Eedomus helper.
        /// </summary>
        private EedomusHelper eedomusHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemperatureScenarioDescription"/> class.
        /// </summary>
        /// <param name="eedomusHelper">Eedomus helper.</param>
        public TemperatureScenarioDescription(EedomusHelper eedomusHelper)
        {
            this.eedomusHelper = eedomusHelper;
            this.Mode = EedomusApi.Local;
        }

        /// <summary>
        /// Gets or sets eedomus id of the temperature probe.
        /// </summary>
        public string EedomusTemperatureId { get; set; }

        /// <summary>
        /// Gets or sets mode of eedomus.
        /// </summary>
        public EedomusApi Mode { get; set; }

        /// <summary>
        /// Create temperature scenario.
        /// </summary>
        public void Create()
        {
            var sentence = new TemperatureSentence() { Sentence = string.Empty };

            Scenario.Create("Temperature")
                .SpeechTrigger(
                    "quelle est la température interieur",
                    "donne moi la température",
                    "donne moi la température interieur",
                    "la température interieur")
                    .Action(() =>
                    {
                        var weather = DommyCache.Get(
                            "TemperatureAction",
                            TimeSpan.FromMinutes(30),
                            () =>
                            {
                                var temperature = this.eedomusHelper.CallService(this.Mode, EedomusAction.PeriphCaract, this.EedomusTemperatureId);

                                return new Weather
                                {
                                    Temperature = Convert.ToDouble(temperature, CultureInfo.InvariantCulture.NumberFormat),
                                };
                            });

                        var temperatures = new[] 
                        { 
                            "{Temperature} degré.", 
                            "Température de {Temperature} degré."
                        };

                        sentence.Sentence = StringHelper.Format(temperatures, new { Temperature = (int)weather.Temperature });

                        return true;
                    })
                    .Say(sentence, "{Sentence}")
                    .Start();
        }

        /// <summary>
        /// Temperature sentence class.
        /// </summary>
        private class TemperatureSentence
        {
            /// <summary>
            /// Gets or sets sentence.
            /// </summary>
            public string Sentence { get; set; }
        }
    }
}
