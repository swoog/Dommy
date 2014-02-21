using Dommy.Business.Actions;
using Dommy.Business.Result;
using Dommy.Business.Scenarios;
using Dommy.Business.Syntax;
using Dommy.Business.Tools;
using Dommy.Business.Triggers;
using Ninject;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

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
            Tile tile;

            Scenario.Create(StringHelper.Format("Tile Time"))
                .TileTrigger(out tile, "Time", Color.Red)
                .TimeTrigger(DateTime.Now, TimeSpan.FromSeconds(10))
                .Action(() =>
                {
                    tile.Data = DateTime.Now;
                    return true;
                })
                .TileUpdate(tile)
                .Start();

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

                    value.Hour = String.Format(hourStr, hour);
                    value.Minute = String.Format(formatString, minute);

                    return true;
                })
                .Say(value, "Il est {Hour} {Minute}.", "{Hour} {Minute}.")
                .Start();
        }
    }
}