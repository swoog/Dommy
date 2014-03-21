//-----------------------------------------------------------------------
// <copyright file="TimeScenarioDescription.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Scenarios
{
    using System;
    using System.Globalization;
    using Dommy.Business.Syntax;
    using Dommy.Business.Tools;

    /// <summary>
    /// Time scenario description.
    /// </summary>
    public class TimeScenarioDescription : IScenarioDescription
    {
        /// <summary>
        /// Create time scenario.
        /// </summary>
        public void Create()
        {
            var value = new TimeData();
            Tile tile;

            Scenario.Create(StringHelper.Format("Tile Time"))
                .TileTrigger(out tile, null, "Time", TileColor.EasternBlue)
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
                        formatString = string.Empty;
                    }
                    else
                    {
                        formatString = StringHelper.Format(minuteStr);
                    }

                    value.Hour = string.Format(CultureInfo.InvariantCulture, hourStr, hour);
                    value.Minute = string.Format(CultureInfo.InvariantCulture, formatString, minute);

                    return true;
                })
                .Say(value, "Il est {Hour} {Minute}.", "{Hour} {Minute}.")
                .Start();
        }

        /// <summary>
        /// Time data used for speech.
        /// </summary>
        private class TimeData
        {
            /// <summary>
            /// Gets or sets the hour.
            /// </summary>
            public string Hour { get; set; }

            /// <summary>
            /// Gets or sets minutes.
            /// </summary>
            public string Minute { get; set; }
        }
    }
}
