//-----------------------------------------------------------------------
// <copyright file="AboutAction.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Scenarios
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Dommy.Business.Result;
    using Dommy.Business.Tools;
    using Dommy.Model;
    using Dommy.Business.Scenarios;
    using Dommy.Business.Syntax;
    using Dommy.Business.Triggers;
    using Dommy.Business.Actions;

    /// <summary>
    /// Say information about application.
    /// </summary>
    public class AboutScenarioDescription : IScenarioDescription
    {
        public void Create()
        {
            Scenario.Create("Bonjour")
                        .NoPrefixSpeechTrigger("bonjour dommy", "salut dommy", "bonsoir dommy")
                        .If(() => DateTime.Now.Hour > 0 && DateTime.Now.Hour < 17,
                            s => s.Say("Bonjour", "Salut"),
                            s => s.Say("Bonsoir", "Salut")
                        )
                        .Start();

            Scenario.Create("Nom")
                .NoPrefixSpeechTrigger("Comment appelle tu", "Quel est ton nom")
                .Say("Je suis Dommy, l'aide vocale.")
                .Start();

            Scenario.Create("Qui")
                .SpeechTrigger("Qui est tu", "Qui ta créé", "Qui est ton créateur", "Comment appelle tu", "Quel est ton nom")
                .Say("Mon créateur est Aurélien GALTIER",
                    "J'ai été créé par Aurélien GALTIER",
                    "Aurélien GALTIER est mon créateur",
                    "Je suis né de l'idée de Aurélien GALTIER")
                .Start();
        }
    }
}
