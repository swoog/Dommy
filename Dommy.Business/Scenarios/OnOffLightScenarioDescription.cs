﻿//-----------------------------------------------------------------------
// <copyright file="OnOffLightScenarioDescription.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Scenarios
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Drawing;
    using System.Linq;
    using Dommy.Business.Syntax;
    using Dommy.Business.Tools;

    /// <summary>
    /// Scenario description for switch on/off light.
    /// </summary>
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
        /// Instance of Eedomus helper.
        /// </summary>
        private EedomusHelper eedomusHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnOffLightScenarioDescription"/> class.
        /// </summary>
        /// <param name="eedomusHelper">Eedomus helper.</param>
        public OnOffLightScenarioDescription(EedomusHelper eedomusHelper)
        {
            this.eedomusHelper = eedomusHelper;
        }

        private void CreateTile()
        {
            Contract.Requires(0 < this.RoomNames.Count);

            Tile tile;

            Scenario.Create(StringHelper.Format("Tile lumière {Name}", this.RoomNames.First()))
                .TileTrigger(out tile, null, StringHelper.Format("Lumière {Name}", this.RoomNames.First()), TileColor.Victoria)
                .Action(() =>
                {
                    var actualState = this.eedomusHelper.CallService(Actions.EedomusApi.Local, Actions.EedomusAction.PeriphCaract, this.EedomusId) != "0";

                    this.eedomusHelper.CallService(Actions.EedomusApi.Local, Actions.EedomusAction.PeriphValue, this.EedomusId, "TOGGLE[0|100]");

                    tile.Data = !actualState;
                    return true;
                })
                .TileUpdate(tile)
                .Start();

            tile.Setembedded("Dommy.Business.Scenarios.OnOffLightScenarioTile.html");

            Scenario.Create(StringHelper.Format("Tile lumière {Name} Notification", this.RoomNames.First()))
                .TimeTrigger(DateTime.Now, TimeSpan.FromSeconds(30))
                .Action(() =>
                {
                    var newEtat = this.eedomusHelper.CallService(Actions.EedomusApi.Local, Actions.EedomusAction.PeriphCaract, this.EedomusId) == "100";

                    if (tile.Data == null || newEtat != (bool)tile.Data)
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
            Contract.Requires(0 < this.RoomNames.Count);
            var onOffResponse = new OnOffResponse();
            var sentences = (from s in new[] { "éteint la lumière {PrefixName}", "éteint {Name}" }
                             from r in this.RoomNames
                             select StringHelper.Format(s, r)).ToArray();

            Scenario.Create(StringHelper.Format("Eteint {Name}", this.RoomNames.First()))
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

        /// <summary>
        /// Create on scenario.
        /// </summary>
        private void CreateOn()
        {
            Contract.Requires(0 < this.RoomNames.Count);
            var onOffResponse = new OnOffResponse();
            var sentences = (from s in new[] { "allume la lumière {PrefixName}", "allume {Name}" }
                             from r in this.RoomNames
                             select StringHelper.Format(s, r)).ToArray();

            Scenario.Create(StringHelper.Format("Allume {Name}", this.RoomNames.First()))
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
            int roomNum = r.Next(this.RoomNames.Count);
            var room = this.RoomNames.ElementAt(roomNum);

            return StringHelper.Format(this.speech, new { StatusFemale = statusFemale, StatusMale = statusMale, Name = room.Name, PrefixName = room.PrefixName, StatusNeutre = statusNeutre });
        }

        /// <summary>
        /// On/off class response.
        /// </summary>
        private class OnOffResponse
        {
            /// <summary>
            /// Gets or sets sentence.
            /// </summary>
            public string Sentence { get; set; }
        }

        /// <summary>
        /// Gets or sets Eedomus sensor id.
        /// </summary>
        public string EedomusId { get; set; }

        /// <summary>
        /// Gets or sets names of the room.
        /// </summary>
        public ICollection<RoomName> RoomNames { get; set; }

        /// <summary>
        /// Create scenarios.
        /// </summary>
        public void Create()
        {
            this.CreateOn();
            this.CreateOff();
            this.CreateTile();
        }
    }
}
