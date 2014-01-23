using Dommy.Business.Actions;
using Dommy.Business.Syntax;
using Dommy.Business.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Scenarios
{
    public class TemperatureScenarioDescription : IScenarioDescription
    {
        /// <summary>
        /// Instance of Eedomus helper.
        /// </summary>
        private EedomusHelper eedomusHelper;

        public string EedomusTemperatureId { get; set; }

        public EedomusApi Mode { get; set; }

        public TemperatureScenarioDescription(EedomusHelper eedomusHelper)
        {
            this.eedomusHelper = eedomusHelper;
            this.Mode = EedomusApi.Local;
        }

        private class TemperatureSentence
        {
            public string Sentence { get; set; }
        }

        public void Create()
        {
            var sentence = new TemperatureSentence() { Sentence = String.Empty };

            Scenario.Create("Temperature")
                .SpeechTrigger(
                    "quelle est la température interieur",
                    "donne moi la température",
                    "donne moi la température interieur",
                    "la température interieur")
                    .Action(() =>
                    {
                        var weather = DommyCache.Get("TemperatureAction", TimeSpan.FromMinutes(30), () =>
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
    }
}
