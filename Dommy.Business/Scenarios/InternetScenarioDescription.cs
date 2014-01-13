//-----------------------------------------------------------------------
// <copyright file="InternetScenarioDescription.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Scenarios
{
    using System;
    using Dommy.Business.Syntax;
    using Dommy.Business.Tools;

    /// <summary>
    /// Test internet scenarios.
    /// </summary>
    public class InternetScenarioDescription : IScenarioDescription
    {
        /// <summary>
        /// Speech logger.
        /// </summary>
        private ISpeechLogger speechLogger;

        /// <summary>
        /// Sites used to test internet.
        /// </summary>
        private string[] sites = new string[]
        {
            "http://mafreebox.fr:8095/fixed/1M",
            "http://www.google.fr",
            "http://www.bing.com",
            "http://www.clubic.com",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="InternetScenarioDescription"/> class.
        /// </summary>
        /// <param name="speechLogger">Speech logger.</param>
        public InternetScenarioDescription(ISpeechLogger speechLogger)
        {
            this.speechLogger = speechLogger;
        }

        /// <summary>
        /// Create scenarios.
        /// </summary>
        public void Create()
        {
            string[] infos = new string[]
            {
                "Taux de téléchargement est de {Taux} kilo octet par secondes",
                "Taux de téléchargement de {Taux} kilo octet par secondes",
                "Le taux de téléchargement est de {Taux} kilo octet par secondes",
                "{Taux} kilo octet par secondes"
            };

            Scenario.Create("Internet test")
                .SpeechTrigger("test internet", "est-ce que j'ai internet")
                .Say("encours", "début du test", "attend")
                .Action(() =>
                {
                    long globalSize = 0;
                    double time = 0;
                    foreach (var site in sites)
                    {
                        var request = System.Net.HttpWebRequest.Create(site);

                        var response = request.GetResponse();
                        var stream = response.GetResponseStream();
                        var b = new byte[1024 * 1024];
                        var readSize = 0;

                        var startDate = DateTime.Now;
                        do
                        {
                            readSize = stream.Read(b, 0, b.Length);
                            globalSize += readSize;
                        } 
                        while (readSize > 0);
                        var endDate = DateTime.Now;

                        time += endDate.Subtract(startDate).TotalSeconds;
                    }

                    var taux = Convert.ToInt32((globalSize / 1024) / time);
                    this.speechLogger.Say(Actor.Dommy, StringHelper.Format(infos, new { Taux = taux }));

                    return true;
                })
                .Start();
        }
    }
}
