//-----------------------------------------------------------------------
// <copyright file="OnOffLightScenarioDescription.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Scenarios
{
    using Dommy.Business.Syntax;
    using Dommy.Business.Tools;
    using System;
    using System.Diagnostics.Contracts;
    using System.Drawing;
    using System.Linq;

    public class OnOffLightScenarioDescription : IScenarioDescription
    {
        /// <summary>
        /// All speech.
        /// </summary>
        private string[] speech = new[]
        {
            "La lumière est {StatusFemale}.",
            "La lumière {PrefixName} est {StatusFemale}.",
            "{Name} est {StatusFemale}.",
            "J'ai {StatusMale} {Name}.",
            "J'ai {StatusMale}.",
            "J'{StatusNeutre}.",
            "J'ai {StatusMale} la lumière {PrefixName}.",
            "Tous de suite.",
            "Je le fais.",
            "Je m'exécute.",
            "Okay",
        };

        /// <summary>
        /// Gets or sets Eedomus sensor id.
        /// </summary>
        public string EedomusId { get; set; }

        /// <summary>
        /// Instance of Eedomus helper.
        /// </summary>
        private EedomusHelper eedomusHelper;

        /// <summary>
        /// Gets or sets names of the room.
        /// </summary>
        public RoomName[] RoomNames { get; set; }

        public OnOffLightScenarioDescription(EedomusHelper eedomusHelper)
        {
            this.eedomusHelper = eedomusHelper;
        }

        private class OnOffResponse
        {
            public string Sentence { get; set; }
        }

        public void Create()
        {
            CreateOn();
            CreateOff();
            CreateTile();
        }

        private void CreateTile()
        {
            Contract.Requires(0 < this.RoomNames.Length);

            Tile tile;

            Scenario.Create(StringHelper.Format("Tile {Name}", this.RoomNames[0]))
                .TileTrigger(out tile, null, StringHelper.Format("Lumière {Name}", this.RoomNames[0]), TileColor.Victoria)
                .If(
                    () => this.eedomusHelper.CallService(Actions.EedomusApi.Local, Actions.EedomusAction.PeriphValue, this.EedomusId) == "100",
                    sTrue => sTrue.EedomusOnOff(this.EedomusId, true),
                    sFalse => sFalse.EedomusOnOff(this.EedomusId, false))
                .Start();

            tile.View = System.IO.File.ReadAllText(new System.IO.FileInfo(@"..\..\..\Dommy.Business\Scenarios\OnOffLightScenarioTile.cshtml").FullName);

            Scenario.Create(StringHelper.Format("Tile {Name} Notification", this.RoomNames[0]))
                .TimeTrigger(DateTime.Now, TimeSpan.FromSeconds(30))
                .Action(() =>
                {
                    var newEtat = this.eedomusHelper.CallService(Actions.EedomusApi.Local, Actions.EedomusAction.PeriphValue, this.EedomusId) == "100";

                    if (tile.Data != null && newEtat != (bool)tile.Data)
                    {
                        tile.Data = newEtat;
                        return true;
                    }

                    return false;
                })
                .TileUpdate(tile)
                .Start();
        }

        private void CreateOff()
        {
            Contract.Requires(0 < this.RoomNames.Length);
            var onOffResponse = new OnOffResponse();
            var sentences = (from s in new[] { "éteint la lumière {PrefixName}", "éteint {Name}" }
                             from r in this.RoomNames
                             select StringHelper.Format(s, r)).ToArray();

            Scenario.Create(StringHelper.Format("Eteint {Name}", this.RoomNames[0]))
                .SpeechTrigger(sentences)
                .EedomusOnOff(this.EedomusId, false)
                .Action(() =>
                {
                    onOffResponse.Sentence = this.CreateResponse(false);
                    return true;
                })
                .Say(onOffResponse, "{Sentence}")
                .Start();
        }

        private void CreateOn()
        {
            Contract.Requires(0 < this.RoomNames.Length);
            var onOffResponse = new OnOffResponse();
            var sentences = (from s in new[] { "allume la lumière {PrefixName}", "allume {Name}" }
                             from r in this.RoomNames
                             select StringHelper.Format(s, r)).ToArray();

            Scenario.Create(StringHelper.Format("Allume {Name}", this.RoomNames[0]))
                .SpeechTrigger(sentences)
                .EedomusOnOff(this.EedomusId, true)
                .Action(() =>
                {
                    onOffResponse.Sentence = this.CreateResponse(true);
                    return true;
                })
                .Say(onOffResponse, "{Sentence}")
                .Start();
        }

        /// <summary>
        /// Create speech response for on/off light.
        /// </summary>
        /// <param name="status">Status (On/Off)</param>
        /// <returns>Return random sentence.</returns>
        private string CreateResponse(bool status)
        {
            string statusFemale = string.Empty;
            string statusMale = string.Empty;
            string statusNeutre = string.Empty;
            if (status)
            {
                statusFemale = "allumée";
                statusMale = "allumé";
                statusNeutre = "allume";
            }
            else
            {
                statusFemale = "éteinte";
                statusMale = "éteind";
                statusNeutre = "éteind";
            }

            Random r = new Random();
            int roomNum = r.Next(this.RoomNames.Length);
            var room = this.RoomNames[roomNum];

            return StringHelper.Format(this.speech, new { StatusFemale = statusFemale, StatusMale = statusMale, Name = room.Name, PrefixName = room.PrefixName, StatusNeutre = statusNeutre });
        }
    }
}