using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dommy.Business.Result;
using Dommy.Business.Tools;
using Dommy.Business.Scenarios;
using Ninject;
using Dommy.Business.Triggers;
using Dommy.Business.Syntax;
using Dommy.Business.Actions;
using System.Globalization;

namespace Dommy.Business.Scenarios
{
    public class TimeScenarioDescription : IScenarioDescription
    {
        private class TimeData
        {
            public string Hour { get; set; }

            public string Minute { get; set; }
        }

        public void Create()
        {
            var value = new TimeData();

            Scenario.Create("Heure")
                .SpeechTrigger("Quel heure est-il", "Donne moi l'heure", "L'heure s'il te plait")
                .Action(() =>
                {
                    int hour = DateTime.Now.Hour;
                    int minute = DateTime.Now.Minute;

                    string hourStr;
                    var minuteStr = new[] { "{0}", "{0} minutes", "et {0} minutes" };

                    if (hour == 0)
                    {
                        hourStr = "minuit";
                        minuteStr = new[] { "{0}", "et {0} minutes" };
                    }
                    else if (hour == 12)
                    {
                        hourStr = "midi";
                        minuteStr = new[] { "{0}", "et {0} minutes" };
                    }
                    else
                    {
                        hourStr = "{0} heure";

                        minuteStr = new[] { "{0}", "{0} minutes", "et {0} minutes" };
                    }

                    string formatString;

                    if (minute == 0)
                    {
                        formatString = "";
                    }
                    else
                    {
                        formatString = StringHelper.Format(minuteStr);
                    }

                    value.Hour = String.Format(CultureInfo.InvariantCulture, hourStr, hour);
                    value.Minute = String.Format(CultureInfo.InvariantCulture, formatString, minute);

                    return true;
                })
                .Say(value, "Il est {Hour} {Minute}.", "{Hour} {Minute}.")
                .Start();
        }
    }
}
