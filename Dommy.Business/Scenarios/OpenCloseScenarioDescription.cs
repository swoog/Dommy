//-----------------------------------------------------------------------
// <copyright file="OpenCloseScenarioDescription.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Scenarios
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Dommy.Business.Syntax;
    using Dommy.Business.Tools;

    /// <summary>
    /// Scenario description for open/close.
    /// </summary>
    public class OpenCloseScenarioDescription : IScenarioDescription
    {
        /// <summary>
        /// Open speech.
        /// </summary>
        private string[] speechOpen = new[]
        {
            "Ouverture {PrefixName}.",
            "J'ai ouvert.",
            "J'ai ouvert {Name}.",
            "J'ouvre.",
            "J'ouvre {Name}.",
            "Tous de suite.",
            "Je le fais.",
            "Je m'exécute.",
            "Okay",
        };

        /// <summary>
        /// Close speech.
        /// </summary>
        private string[] speechClose = new[]
        {
            "Fermeture {PrefixName}.",
            "J'ai fermé.",
            "J'ai fermé {Name}.",
            "Je ferme.",
            "Je ferme {Name}.",
            "Tous de suite.",
            "Je le fais.",
            "Je m'exécute.",
            "Okay",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenCloseScenarioDescription"/> class.
        /// </summary>
        public OpenCloseScenarioDescription()
        {
        }

        /// <summary>
        /// Gets or sets Eedomus sensor id.
        /// </summary>
        public string EedomusId { get; set; }

        /// <summary>
        /// Gets or sets names of the element to open/close.
        /// </summary>
        public ICollection<ElementName> ElementNames { get; set; }

        /// <summary>
        /// Create scenarios.
        /// </summary>
        public void Create()
        {
            this.CreateOn();
            this.CreateOff();
        }

        /// <summary>
        /// Create off scenario.
        /// </summary>
        private void CreateOff()
        {
            Contract.Requires(0 < this.ElementNames.Count);
            var openCloseResponse = new OpenCloseResponse();
            var sentences = (from s in new[] { "ferme {Name}" }
                             from r in this.ElementNames
                             select StringHelper.Format(s, r)).ToArray();

            Scenario.Create(StringHelper.Format("Ferme {Name}", this.ElementNames.First()))
                .SpeechTrigger(sentences)
                .EedomusOnOff(this.EedomusId, false)
                .Action(() =>
                {
                    openCloseResponse.Sentence = this.CreateResponse(false);
                    return true;
                })
                .Say(openCloseResponse, "{Sentence}")
                .Start();
        }

        /// <summary>
        /// Create on scenario.
        /// </summary>
        private void CreateOn()
        {
            Contract.Requires(0 < this.ElementNames.Count);
            var openCloseResponse = new OpenCloseResponse();
            var sentences = (from s in new[] { "ouvre {Name}" }
                             from r in this.ElementNames
                             select StringHelper.Format(s, r)).ToArray();

            Scenario.Create(StringHelper.Format("Ouvre {Name}", this.ElementNames.First()))
                .SpeechTrigger(sentences)
                .EedomusOnOff(this.EedomusId, true)
                .Action(() =>
                {
                    openCloseResponse.Sentence = this.CreateResponse(true);
                    return true;
                })
                .Say(openCloseResponse, "{Sentence}")
                .Start();
        }

        /// <summary>
        /// Create speech response for open/close.
        /// </summary>
        /// <param name="status">Status (Open/Close)</param>
        /// <returns>Return random sentence.</returns>
        private string CreateResponse(bool status)
        {
            Random r = new Random();
            int elementNum = r.Next(this.ElementNames.Count);
            var element = this.ElementNames.ElementAt(elementNum);

            var speech = status ? this.speechOpen : this.speechClose;

            return StringHelper.Format(speech, element);
        }

        /// <summary>
        /// Open/Close class response.
        /// </summary>
        private class OpenCloseResponse
        {
            /// <summary>
            /// Gets or sets sentence.
            /// </summary>
            public string Sentence { get; set; }
        }
    }
}
